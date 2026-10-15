using UnityEngine;

public class OllamaTest : MonoBehaviour
{
    [TextArea(2, 4)]
    [SerializeField] private string userMessage = "Merhaba Şapkacı, çay partisi nasıl gidiyor?";

    private readonly ILlmClient _client = new OllamaClient();
    private readonly HatterPromptBuilder _builder = new HatterPromptBuilder();
    private readonly ConversationHistory _history = new ConversationHistory();
    private bool _busy;

    // Play modunda: User Message'ı yaz, bileşenin ⋮ menüsünden "Send" seç.
    [ContextMenu("Send")]
    public async void Send()
    {
        if (_busy || string.IsNullOrWhiteSpace(userMessage)) return;
        _busy = true;

        _history.AddUser(userMessage);
        var result = await _client.SendAsync(_builder.Build(_history));

        if (!result.Success)
        {
            Debug.LogWarning("LLM hatası: " + result.Error);
            _busy = false;
            return;
        }

        ParseResult parsed = NpcResponseParser.Parse(result.RawJson);
        if (!parsed.IsValid)
        {
            Debug.LogWarning("Geçersiz yanıt, oyun durumu değişmedi: " + parsed.Error);
            _busy = false;
            return;
        }

        NpcResponse npc = NpcResponseValidator.Validate(parsed.Response);
        _history.AddAssistant(JsonUtility.ToJson(npc));

        Debug.Log($"Oyuncu: {userMessage}\nreply: {npc.reply}\nmood: {npc.mood}\npersuasion: {npc.persuasion:F2}\naction: {npc.action}");
        _busy = false;
    }

    // Ollama'ya gitmeden parser'ı bozuk girdilerle dener.
    [ContextMenu("Test Bad Json")]
    private void TestBadJson()
    {
        string[] cases =
        {
            "",
            "abc",
            "{\"message\":{\"role\":\"assistant\",\"content\":\"{bozuk json\"}}",
            "{\"message\":{\"role\":\"assistant\",\"content\":\"{\\\"mood\\\":\\\"happy\\\",\\\"persuasion\\\":0.5,\\\"action\\\":\\\"None\\\"}\"}}",
            "{\"message\":{\"role\":\"assistant\",\"content\":\"{\\\"reply\\\":\\\"Merhaba\\\",\\\"mood\\\":\\\"happy\\\",\\\"persuasion\\\":0.5,\\\"action\\\":\\\"None\\\"}\"}}"
        };

        for (int i = 0; i < cases.Length; i++)
        {
            ParseResult r = NpcResponseParser.Parse(cases[i]);
            Debug.Log($"Test {i + 1}: IsValid={r.IsValid} {r.Error}");
        }
    }
}