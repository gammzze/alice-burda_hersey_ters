using System.Collections;
using UnityEngine;

// Arayüzü Ollama'sız dener: sahte gecikme sırasında "Düşünüyor..." gösterir, girişi kilitler.
public class DialogueUITest : MonoBehaviour
{
    [SerializeField] private DialogueUI ui;
    [SerializeField] private ThinkingIndicator thinking;
    [SerializeField] private float fakeDelay = 2.5f;

    private void OnEnable() => ui.MessageSubmitted += OnSubmitted;
    private void OnDisable() => ui.MessageSubmitted -= OnSubmitted;

    private void OnSubmitted(string text) => StartCoroutine(FakeAsk(text));

    private IEnumerator FakeAsk(string text)
    {
        ui.SetInteractable(false);
        ui.SetReply(string.Empty);
        thinking.Show();

        yield return new WaitForSeconds(fakeDelay);

        thinking.Hide();
        ui.SetReply("Sen dedin ki: " + text);
        ui.SetInteractable(true);
    }

    [ContextMenu("Lock")]
    private void Lock() => ui.SetInteractable(false);

    [ContextMenu("Unlock")]
    private void Unlock() => ui.SetInteractable(true);
}