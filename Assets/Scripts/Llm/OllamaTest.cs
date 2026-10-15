using UnityEngine;

public class OllamaTest : MonoBehaviour
{
    [TextArea(2, 4)]
    [SerializeField] private string userMessage = "Merhaba Şapkacı, çay partisi nasıl gidiyor?";

    private NpcDialogueController _controller;
    private GameActionFactory _factory;
    private bool _busy;

    private void Awake()
    {
        _controller = new NpcDialogueController(
            new OllamaClient(),
            new HatterPromptBuilder(),
            new ConversationHistory(),
            maxRetries: 1);

        _factory = new GameActionFactory(new DebugGameFlow(), new DebugHatterScene());
    }

    // Play modunda: User Message'ı yaz, bileşenin ⋮ menüsünden "Send" seç.
    [ContextMenu("Send")]
    public async void Send()
    {
        if (_busy) return;
        _busy = true;

        DialogueOutcome outcome = await _controller.AskAsync(userMessage, destroyCancellationToken);

        if (outcome.IsCancelled) return;   // sahne kapandı

        NpcResponse npc = outcome.Response;
        string tag = outcome.IsFallback ? "YEDEK YANIT (durum değişmez)" : "LLM";
        Debug.Log($"[{tag}] Oyuncu: {userMessage}\nreply: {npc.reply}\nmood: {npc.mood}\npersuasion: {npc.persuasion:F2}\naction: {npc.action}\napply: {outcome.ShouldApplyAction}\nhata: {outcome.Error}");

        // Eylem yalnızca gerçek ve geçerli yanıtta uygulanır.
        if (outcome.ShouldApplyAction)
            _factory.Create(npc).Execute();

        _busy = false;
    }

    // Ollama'ya gitmeden eşik ve eylem kurallarını dener.
    [ContextMenu("Test Actions")]
    private void TestActions()
    {
        var cases = new[]
        {
            new NpcResponse { reply = "a", mood = "happy", persuasion = 0.9f, action = NpcActions.LeaveTable },
            new NpcResponse { reply = "b", mood = "happy", persuasion = 0.3f, action = NpcActions.LeaveTable },
            new NpcResponse { reply = "c", mood = "angry", persuasion = 0.0f, action = NpcActions.Attack },
            new NpcResponse { reply = "d", mood = "neutral", persuasion = 0.5f, action = NpcActions.None },
        };

        for (int i = 0; i < cases.Length; i++)
        {
            Debug.Log($"--- Test {i + 1}: {cases[i].action}, persuasion {cases[i].persuasion:F1}");
            _factory.Create(cases[i]).Execute();
        }
    }
}