using UnityEngine;

/// <summary>
/// Beyaz (boyanmamýþ) ya da kýrmýzý (boyanmýþ) gül.
/// Boyanýnca rengi deðiþir ve collider'ý kalkar.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Rose : MonoBehaviour, IPaintable
{
    [SerializeField] private Color unpaintedColor = Color.white;
    [SerializeField] private Color paintedColor = Color.red;

    private SpriteRenderer spriteRenderer;
    private Collider2D roseCollider;

    public bool IsPainted { get; private set; }
    public Vector2 Position => transform.position;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        roseCollider = GetComponent<Collider2D>();
        spriteRenderer.color = unpaintedColor;
    }

    [ContextMenu("Paint")]
    public void Paint()
    {
        if (IsPainted) return;
        IsPainted = true;
        spriteRenderer.color = paintedColor;
        roseCollider.enabled = false; // sensör artýk bu gülü görmez
        Debug.Log($"[Rose] {name} boyandý");
    }
}
