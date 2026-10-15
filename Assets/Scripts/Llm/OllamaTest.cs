using UnityEngine;

public class OllamaTest : MonoBehaviour
{
    [TextArea(2, 4)]
    [SerializeField] private string userMessage = "Merhaba Şapkacı, çay partisi nasıl gidiyor?";

    private NpcDialogueController _controller;
    private bool _busy;

    private void Awake()
    {
        _controller = new NpcDialogueController(
            new OllamaClient(),
            new HatterPromptBuilder(),
            new ConversationHistory(),
            maxRetries: 1);
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
        _busy = false;
    }
}