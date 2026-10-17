using UnityEngine;

// Arayüzü Ollama'sız dener: yazılan mesajı geri gösterir.
public class DialogueUITest : MonoBehaviour
{
    [SerializeField] private DialogueUI ui;

    private void OnEnable() => ui.MessageSubmitted += OnSubmitted;
    private void OnDisable() => ui.MessageSubmitted -= OnSubmitted;

    private void OnSubmitted(string text)
    {
        ui.SetReply("Sen dedin ki: " + text);
    }

    [ContextMenu("Lock")]
    private void Lock() => ui.SetInteractable(false);

    [ContextMenu("Unlock")]
    private void Unlock() => ui.SetInteractable(true);
}