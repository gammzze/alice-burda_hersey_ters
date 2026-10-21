using UnityEngine;

public class CheckpointRespawn : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform checkpoint;
    [SerializeField] private SoldierAI[] soldiers;

    private void OnEnable()
    {
        foreach (var s in soldiers) s.PlayerCaught += HandleCaught;
    }

    private void OnDisable()
    {
        foreach (var s in soldiers) s.PlayerCaught -= HandleCaught;
    }

    private void HandleCaught(SoldierAI _)
    {
        player.position = checkpoint.position;

        if (player.TryGetComponent(out Rigidbody2D rb))
            rb.linearVelocity = Vector2.zero;

        foreach (var s in soldiers) s.ResetToSpawn();
    }
}