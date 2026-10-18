using System.Collections;
using UnityEngine;

// Arayüzü Ollama'sız dener: sahte gecikme, "Düşünüyor..." ve sahte hatalar.
public class DialogueUITest : MonoBehaviour
{
    public enum Simulation { None, ServerDown, Timeout, BadResponse }

    [SerializeField] private DialogueUI ui;
    [SerializeField] private ThinkingIndicator thinking;
    [SerializeField] private ErrorPanel errorPanel;
    [SerializeField] private float fakeDelay = 2.5f;
    [SerializeField] private Simulation simulate = Simulation.None;

    private void OnEnable() => ui.MessageSubmitted += OnSubmitted;
    private void OnDisable() => ui.MessageSubmitted -= OnSubmitted;

    private void OnSubmitted(string text) => StartCoroutine(FakeAsk(text));

    private IEnumerator FakeAsk(string text)
    {
        ui.SetInteractable(false);
        ui.SetReply(string.Empty);
        errorPanel.Hide();
        thinking.Show();

        yield return new WaitForSeconds(fakeDelay);

        thinking.Hide();

        if (simulate == Simulation.None)
        {
            ui.SetReply("Sen dedin ki: " + text);
        }
        else
        {
            // Gerçek akıştaki gibi: Şapkacı yedek bir cümle söyler, ayrıca hata uyarısı çıkar.
            ui.SetReply(FallbackResponses.Next().reply);
            errorPanel.Show(DialogueErrorMessages.ForError(FakeError(simulate)));
        }

        ui.SetInteractable(true);
    }

    private static string FakeError(Simulation s)
    {
        switch (s)
        {
            case Simulation.ServerDown: return "Cannot connect to destination host";
            case Simulation.Timeout: return "Request timeout";
            case Simulation.BadResponse: return "Eksik alan: reply";
            default: return string.Empty;
        }
    }

    [ContextMenu("Lock")]
    private void Lock() => ui.SetInteractable(false);

    [ContextMenu("Unlock")]
    private void Unlock() => ui.SetInteractable(true);
}