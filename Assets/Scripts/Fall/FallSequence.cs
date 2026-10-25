using UnityEngine;

// 01_Fall: Alice deliðe girer, sonra düþer. Katmanlar farklý hýzlarda yukarý kayar (parallax).
// Düþüþ bitince ekran kararýr ve Intro aþamasý tamamlanýr.
public class FallSequence : MonoBehaviour
{
    [Header("Nesneler")]
    [SerializeField] private Transform alice;
    [SerializeField] private Transform hole;
    [SerializeField] private Transform[] layers;
    [SerializeField] private float[] speeds;
    [SerializeField] private ScreenFader fader;

    [Header("Deliðe giriþ")]
    [SerializeField] private float enterDuration = 1.6f;
    [SerializeField] private float settleDuration = 0.5f;
    [SerializeField] private Vector2 fallPosition = Vector2.zero;
    [SerializeField] private float holeScrollSpeed = 8f;

    [Header("Düþüþ")]
    [SerializeField] private float fallDuration = 5f;
    [SerializeField] private float wrapTop = 7f;
    [SerializeField] private float wrapHeight = 14f;
    [SerializeField] private float swayAmount = 0.6f;
    [SerializeField] private float swaySpeed = 2f;

    private IGameFlow _flow;
    private Vector3 _startPosition;
    private Vector3 _baseScale;
    private Vector3 _holePosition;
    private float _time;
    private bool _done;

    private void Start()
    {
        _flow = GameFlowResolver.Resolve();
        _startPosition = alice.position;
        _baseScale = alice.localScale;
        _holePosition = hole != null ? hole.position : _startPosition;
    }

    private void Update()
    {
        if (_done) return;
        _time += Time.deltaTime;

        if (_time < enterDuration)
        {
            EnterHole(_time / enterDuration);
            return;
        }

        float fallTime = _time - enterDuration;
        ScrollLayers();
        ScrollHole();
        MoveAliceWhileFalling(fallTime);

        if (fallTime >= fallDuration) Finish();
    }

    private void EnterHole(float t)
    {
        float eased = t * t;   // hýzlanarak girer
        alice.position = Vector3.Lerp(_startPosition, _holePosition, eased);
        alice.localScale = Vector3.Lerp(_baseScale, _baseScale * 0.2f, eased);
        alice.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, 360f, eased));
    }

    private void MoveAliceWhileFalling(float fallTime)
    {
        Vector3 center = new Vector3(fallPosition.x, fallPosition.y, alice.position.z);

        if (fallTime < settleDuration)
        {
            float t = fallTime / settleDuration;
            alice.position = Vector3.Lerp(_holePosition, center, t);
            alice.localScale = Vector3.Lerp(_baseScale * 0.2f, _baseScale, t);
            alice.rotation = Quaternion.identity;
            return;
        }

        float sway = Mathf.Sin((fallTime - settleDuration) * swaySpeed);
        alice.position = center + new Vector3(sway * swayAmount, 0f, 0f);
        alice.localScale = _baseScale;
        alice.rotation = Quaternion.Euler(0f, 0f, sway * 12f);
    }

    private void ScrollLayers()
    {
        int count = Mathf.Min(layers.Length, speeds.Length);
        for (int i = 0; i < count; i++)
        {
            if (layers[i] == null) continue;
            Vector3 p = layers[i].position;
            p.y += speeds[i] * Time.deltaTime;
            if (p.y > wrapTop) p.y -= wrapHeight;
            layers[i].position = p;
        }
    }

    private void ScrollHole()
    {
        if (hole == null || !hole.gameObject.activeSelf) return;

        hole.position += Vector3.up * (holeScrollSpeed * Time.deltaTime);
        if (hole.position.y > wrapTop + 2f)
            hole.gameObject.SetActive(false);
    }

    private void Finish()
    {
        _done = true;
        if (fader != null) fader.FadeOut(CompleteIntro);
        else CompleteIntro();
    }

    private void CompleteIntro() => _flow.CompleteStage(GameStage.Intro);
}