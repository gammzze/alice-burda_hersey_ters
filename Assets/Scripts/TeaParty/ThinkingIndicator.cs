using System.Collections;
using TMPro;
using UnityEngine;

// "Düþünüyor..." göstergesi. Yalnýzca görüntü: gösterir, gizler, noktalarý döndürür.
public class ThinkingIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private string baseText = "Düþünüyor";
    [SerializeField] private float interval = 0.4f;

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    private void OnEnable()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        int dots = 0;
        while (true)
        {
            label.text = baseText + new string('.', dots);
            dots = (dots + 1) % 4;
            yield return new WaitForSeconds(interval);
        }
    }
}
