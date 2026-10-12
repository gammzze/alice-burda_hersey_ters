using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float interactRadius = 1f;
    [SerializeField] private LayerMask interactableLayer;

    private Rigidbody2D rb;
    private Vector2 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"),
                            Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(interactKey)) TryInteract();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * speed;
    }

    private void TryInteract()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, interactableLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IInteractable>(out var target))
            {
                target.Interact();
                return;
            }
        }
    }
}