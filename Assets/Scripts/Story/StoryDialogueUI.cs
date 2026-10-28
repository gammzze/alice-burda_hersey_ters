using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Sabit metin ara sahnesinin arayüzü: yalnýzca konuþan kiþinin balonunu gösterir.
public class StoryDialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject aliceBubble;
    [SerializeField] private TMP_Text aliceText;
    [SerializeField] private GameObject rabbitBubble;
    [SerializeField] private TMP_Text rabbitText;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text buttonLabel;

    public event Action ContinueClicked;

    private void Awake()
    {
        continueButton.onClick.AddListener(() => ContinueClicked?.Invoke());
    }

    public void ShowLine(StorySpeaker speaker, string text)
    {
        bool alice = speaker == StorySpeaker.Alice;

        aliceBubble.SetActive(alice);
        rabbitBubble.SetActive(!alice);

        if (alice) aliceText.text = text;
        else rabbitText.text = text;
    }

    public void SetButtonLabel(string text) => buttonLabel.text = text;
    public void SetInteractable(bool value) => continueButton.interactable = value;
}