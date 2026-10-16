using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// Diyalog akýþý: mesaj -> prompt -> LLM -> parse -> doðrulama -> sonuç.
// Hata olursa 1-2 kez yeniden dener, olmazsa yedek yanýt döner.
public class NpcDialogueController
{
    private readonly ILlmClient _client;
    private readonly HatterPromptBuilder _builder;
    private readonly ConversationHistory _history;
    private readonly PersuasionTracker _tracker;
    private readonly int _maxRetries;

    public NpcDialogueController(ILlmClient client,
                                 HatterPromptBuilder builder,
                                 ConversationHistory history,
                                 int maxRetries = 1,
                                 PersuasionTracker tracker = null)
    {
        _client = client;
        _builder = builder;
        _history = history;
        _maxRetries = Mathf.Clamp(maxRetries, 0, 2);
        _tracker = tracker ?? new PersuasionTracker();
    }

    public async Task<DialogueOutcome> AskAsync(string playerMessage,
                                                CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(playerMessage))
            return DialogueOutcome.Fallback("Boþ mesaj, istek gönderilmedi.");

        _history.AddUser(playerMessage);
        string lastError = "Bilinmeyen hata.";

        for (int attempt = 0; attempt <= _maxRetries; attempt++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _history.RemoveLast();
                return DialogueOutcome.Cancelled();
            }

            try
            {
                LlmResult result = await _client.SendAsync(_builder.Build(_history, _tracker.Current), cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    _history.RemoveLast();
                    return DialogueOutcome.Cancelled();
                }

                if (!result.Success)
                {
                    lastError = result.Error;
                    Debug.LogWarning($"LLM hatasý (deneme {attempt + 1}): {lastError}");
                    continue;
                }

                ParseResult parsed = NpcResponseParser.Parse(result.RawJson);
                if (!parsed.IsValid)
                {
                    lastError = parsed.Error;
                    Debug.LogWarning($"Geçersiz yanýt (deneme {attempt + 1}): {lastError}");
                    continue;
                }

                // 1) deðerleri düzelt, 2) toplam ikna seviyesini kodda biriktir,
                // 3) eþik kontrolü TOPLAM seviyeye bakar.
                NpcResponse npc = NpcResponseValidator.Normalize(parsed.Response);
                float turnScore = npc.persuasion;
                npc.persuasion = _tracker.Apply(turnScore, npc.mood);
                npc = NpcResponseValidator.Validate(npc);

                // Geçmiþe modelin kendi verdiði tur puaný yazýlýr, toplam deðil.
                var stored = new NpcResponse
                {
                    reply = npc.reply,
                    mood = npc.mood,
                    persuasion = turnScore,
                    action = npc.action
                };
                _history.AddAssistant(JsonUtility.ToJson(stored));
                return DialogueOutcome.Success(npc);
            }
            catch (Exception e)
            {
                lastError = e.Message;
                Debug.LogWarning($"Beklenmeyen hata (deneme {attempt + 1}): {lastError}");
            }
        }

        _history.RemoveLast();
        return DialogueOutcome.Fallback(lastError);
    }
}