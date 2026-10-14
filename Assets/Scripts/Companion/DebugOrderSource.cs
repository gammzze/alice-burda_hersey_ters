using UnityEngine;

/// <summary>Geçici test emir kaynaðý. Space tuþu emir verir.</summary>
public class DebugOrderSource : MonoBehaviour, IOrderSource
{
    public bool HasPendingOrder { get; private set; }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) HasPendingOrder = true;
    }

    public void ConsumeOrder() { HasPendingOrder = false; }
}