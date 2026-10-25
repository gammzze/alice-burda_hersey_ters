using System;
using System.Collections;
using UnityEngine;

// Ekraný karartýp açar. Sahne geçiþlerinden önce FadeOut, sahne açýlýrken FadeIn kullanýlýr.
[RequireComponent(typeof(CanvasGroup))]
public class ScreenFader : MonoBehaviour
{
    [SerializeField] private float fadeInSeconds = 0.8f;
    [SerializeField] private float fadeOutSeconds = 0.8f;
    [SerializeField] private bool fadeInOnStart = true;

    private CanvasGroup _group;

    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();
        _group.alpha = fadeInOnStart ? 1f : 0f;
        _group.blocksRaycasts = fadeInOnStart;
    }

    private void Start()
    {
        if (fadeInOnStart) FadeIn();
    }

    public void FadeIn(Action onDone = null) => StartCoroutine(Fade(1f, 0f, fadeInSeconds, onDone));
    public void FadeOut(Action onDone = null) => StartCoroutine(Fade(0f, 1f, fadeOutSeconds, onDone));

    private IEnumerator Fade(float from, float to, float seconds, Action onDone)
    {
        _group.blocksRaycasts = true;   // geçiþ sýrasýnda týklamalarý engelle

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / Mathf.Max(0.01f, seconds);
            _group.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        _group.alpha = to;
        _group.blocksRaycasts = to > 0.5f;
        onDone?.Invoke();
    }
}
