using System.Collections;
using TMPro;
using UnityEngine;

// Oyuncuya hata mesajý gösterir ve birkaç saniye sonra kendiliðinden kapanýr.
// Yalnýzca görüntü: hatanýn ne olduðuna karar vermez.
public class ErrorPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private float autoHideSeconds = 5f;

    private Coroutine _hideRoutine;

    public void Show(string message)
    {
        label.text = message;
        gameObject.SetActive(true);

        if (_hideRoutine != null) StopCoroutine(_hideRoutine);
        _hideRoutine = StartCoroutine(HideAfterDelay());
    }

    public void Hide()
    {
        if (_hideRoutine != null) StopCoroutine(_hideRoutine);
        _hideRoutine = null;
        gameObject.SetActive(false);
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(autoHideSeconds);
        _hideRoutine = null;
        gameObject.SetActive(false);
    }
}