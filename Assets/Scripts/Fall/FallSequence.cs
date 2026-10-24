using UnityEngine;

// 01_Fall: Alice deliðe düþer. Katmanlar farklý hýzlarda yukarý kayar (parallax),
// Alice saða sola sallanýr. Süre dolunca Intro aþamasý tamamlanýr.
public class FallSequence : MonoBehaviour
{
    [SerializeField] private Transform alice;
    [SerializeField] private Transform[] layers;
    [SerializeField] private float[] speeds;
    [SerializeField] private float wrapTop = 7f;
    [SerializeField] private float wrapHeight = 14f;
    [SerializeField] private float duration = 6f;
    [SerializeField] private float swayAmount = 0.6f;
    [SerializeField] private float swaySpeed = 2f;

    private float _time;
    private bool _done;
    private IGameFlow _flow;
    private Vector3 _alicePosition;

    private void Start()
    {
        _flow = GameFlowResolver.Resolve();
        _alicePosition = alice.position;
    }

    private void Update()
    {
        if (_done) return;
        _time += Time.deltaTime;

        int count = Mathf.Min(layers.Length, speeds.Length);
        for (int i = 0; i < count; i++)
        {
            if (layers[i] == null) continue;
            Vector3 p = layers[i].position;
            p.y += speeds[i] * Time.deltaTime;
            if (p.y > wrapTop) p.y -= wrapHeight;
            layers[i].position = p;
        }

        float sway = Mathf.Sin(_time * swaySpeed);
        alice.position = _alicePosition + new Vector3(sway * swayAmount, 0f, 0f);
        alice.rotation = Quaternion.Euler(0f, 0f, sway * 12f);

        if (_time >= duration)
        {
            _done = true;
            _flow.CompleteStage(GameStage.Intro);
        }
    }
}
