using UnityEngine;

// UI ile diyalog mantýðýný baðlar. Kendi baþýna karar vermez:
// mesajý controller'a iletir, sonucu arayüze ve eylem fabrikasýna yönlendirir.
public class TeaPartyController : MonoBehaviour
{
    [SerializeField] private TeaPartyInstaller installer;
    [SerializeField] private TeaPartySceneController sceneController;
    [SerializeField] private DialogueUI ui;
    [SerializeField] private ThinkingIndicator thinking;
    [SerializeField] private ErrorPanel errorPanel;

    private bool _busy;

    private async void Start()
    {
        ui.MessageSubmitted += OnMessageSubmitted;
        sceneController.LeaveSequenceStarted += OnLeaveSequenceStarted;
        sceneController.SceneReset += OnSceneReset;

        // Arka planda modeli ýsýt; oyuncu beklemez.
        await installer.Warmup.RunAsync(destroyCancellationToken);
    }

    private void OnDestroy()
    {
        if (ui != null) ui.MessageSubmitted -= OnMessageSubmitted;
        if (sceneController != null)
        {
            sceneController.LeaveSequenceStarted -= OnLeaveSequenceStarted;
            sceneController.SceneReset -= OnSceneReset;
        }
    }

    private void OnLeaveSequenceStarted()
    {
        errorPanel.Hide();
        ui.Hide();
    }

    // Yalnýzca debug Reset düðmesi için: arayüzü geri açar.
    private void OnSceneReset()
    {
        ui.Show();
        ui.SetReply(string.Empty);
        ui.SetInteractable(true);
        thinking.Hide();
        errorPanel.Hide();
        _busy = false;
    }

    private async void OnMessageSubmitted(string text)
    {
        if (_busy) return;
        _busy = true;

        ui.SetInteractable(false);
        ui.SetReply(string.Empty);
        errorPanel.Hide();
        thinking.Show();

        DialogueOutcome outcome = await installer.Dialogue.AskAsync(text, destroyCancellationToken);

        // Sahne kapandýysa arayüze dokunma.
        if (outcome.IsCancelled) return;

        thinking.Hide();

        NpcResponse npc = outcome.Response;
        ui.SetReply(npc.reply);

        if (outcome.IsFallback)
            errorPanel.Show(DialogueErrorMessages.ForError(outcome.Error));

        Debug.Log($"[{(outcome.IsFallback ? "YEDEK" : "LLM")}] mood: {npc.mood} | persuasion: {npc.persuasion:F2} | action: {npc.action} | apply: {outcome.ShouldApplyAction}");

        // Eylem yalnýzca gerçek ve geçerli yanýtta uygulanýr.
        if (outcome.ShouldApplyAction)
        {
            IGameAction action = installer.Actions.Create(npc);
            action.Execute();

            // Sekans baþladý: giriþ kilitli kalýr, panel sekansýn içinde kapanýr.
            if (action is LeaveTableAction) return;
        }

        ui.SetInteractable(true);
        _busy = false;
    }
}