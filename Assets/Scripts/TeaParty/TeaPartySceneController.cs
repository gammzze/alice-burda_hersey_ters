using System.Collections;
using UnityEngine;

// 05_TeaParty sahnesinin görsel olaylarý. IHatterScene'in gerçek uygulamasý.
public class TeaPartySceneController : MonoBehaviour, IHatterScene
{
    [SerializeField] private Transform[] cups;
    [SerializeField] private GameObject hole;
    [SerializeField] private SpriteRenderer hatter;
    [SerializeField] private float knockDuration = 0.6f;

    private Vector3[] _cupPositions;
    private Quaternion[] _cupRotations;
    private Color _hatterColor;

    private void Awake()
    {
        _cupPositions = new Vector3[cups.Length];
        _cupRotations = new Quaternion[cups.Length];
        for (int i = 0; i < cups.Length; i++)
        {
            if (cups[i] == null) continue;
            _cupPositions[i] = cups[i].position;
            _cupRotations[i] = cups[i].rotation;
        }

        if (hatter != null) _hatterColor = hatter.color;
        if (hole != null) hole.SetActive(false);
    }

    public void KnockOverCups() => StartCoroutine(KnockRoutine());

    public void OpenHole()
    {
        if (hole != null) hole.SetActive(true);
    }

    public void HatterAttack()
    {
        if (hatter != null) StartCoroutine(FlashRoutine());
    }

    // Test için: sahneyi baþlangýç haline döndürür.
    public void ResetScene()
    {
        StopAllCoroutines();
        for (int i = 0; i < cups.Length; i++)
        {
            if (cups[i] == null) continue;
            cups[i].SetPositionAndRotation(_cupPositions[i], _cupRotations[i]);
        }
        if (hole != null) hole.SetActive(false);
        if (hatter != null) hatter.color = _hatterColor;
    }

    private IEnumerator KnockRoutine()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / knockDuration;
            for (int i = 0; i < cups.Length; i++)
            {
                if (cups[i] == null) continue;
                float side = i % 2 == 0 ? 1f : -1f;
                cups[i].rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, 90f * side, t));
                cups[i].position = _cupPositions[i] + new Vector3(0.3f * side * t, -0.3f * t, 0f);
            }
            yield return null;
        }
    }

    private IEnumerator FlashRoutine()
    {
        hatter.color = Color.red;
        yield return new WaitForSeconds(0.4f);
        hatter.color = _hatterColor;
    }
}
