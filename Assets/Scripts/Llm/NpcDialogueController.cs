using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// Diyalog akýþý: mesaj -> prompt -> LLM -> parse -> doðrulama -> sonuç.
// Hata olursa 1-2 kez yeniden dener, olmazsa yedek yanýt döner.
// Baðýmlýlýklar dýþarýdan verilir (arayüze baðýmlýlýk, test edilebilirlik).
public class NpcDialogueController
{
    private readonly ILlmClient _client;
    private readonly HatterPromptBuilder _builder;
    private readonly ConversationHistory _history;
    private readonly int _maxRetries;

    public NpcDialogueController(ILlmClient client,
                                 HatterPromptBuilder builder,
                                 ConversationHistory history,
                                 int maxRetries = 1)
    {
        _client = client;
        _builder = builder;
        _history = history;
        _maxRetries = Mathf.Clamp(maxRetries, 0, 2);
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
                LlmResult result = await _client.SendAsync(_builder.Build(_history), cancellationToken);

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

                NpcResponse npc = NpcResponseValidator.Validate(parsed.Response);
                _history.AddAssistant(JsonUtility.ToJson(npc));
                return DialogueOutcome.Success(npc);
            }
            catch (Exception e)
            {
                lastError = e.Message;
                Debug.LogWarning($"Beklenmeyen hata (deneme {attempt + 1}): {lastError}");
            }
        }

        // Tüm denemeler baþarýsýz: baþarýsýz oyuncu mesajý geçmiþte kalmasýn.
        _history.RemoveLast();
        return DialogueOutcome.Fallback(lastError);
    }
}