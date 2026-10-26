using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Diyalog arayüzü: yalnýzca görüntü ve giriþ. Ýþ mantýðý içermez.
public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Button sendButton;
    [SerializeField] private TMP_Text replyText;

    // Oyuncu mesaj gönderdiðinde tetiklenir (boþ mesajlar buraya gelmez).
    public event Action<string> MessageSubmitted;

    private void Awake()
    {
        sendButton.onClick.AddListener(Submit);
        input.onSubmit.AddListener(_ => Submit());   // Enter tuþu
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void SetReply(string text)
    {
        replyText.text = text;
    }

    // Bekleme sýrasýnda giriþi kilitlemek için (Adým 4'te kullanýlacak).
    public void SetInteractable(bool value)
    {
        input.interactable = value;
        sendButton.interactable = value;
        if (value) input.ActivateInputField();
    }

    private void Submit()
    {
        if (!input.interactable) return;

        string text = input.text.Trim();
        if (string.IsNullOrEmpty(text)) return;

        input.text = string.Empty;
        MessageSubmitted?.Invoke(text);
    }
}