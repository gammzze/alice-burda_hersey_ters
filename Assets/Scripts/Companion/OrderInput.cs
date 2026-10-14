using UnityEngine;

/// <summary>
/// Oyuncunun Fýrça Bot'a verdiði emri tuþtan okur.
/// Tuþ Inspector'dan deðiþtirilebilir.
/// </summary>
public class OrderInput : MonoBehaviour, IOrderSource
{
    [SerializeField] private KeyCode orderKey = KeyCode.Space;

    public bool HasPendingOrder { get; private set; }

    void Update()
    {
        if (Input.GetKeyDown(orderKey)) HasPendingOrder = true;
    }

    public void ConsumeOrder() { HasPendingOrder = false; }
}