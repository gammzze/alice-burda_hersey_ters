using UnityEngine;

public class EscapeGoal : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    private bool completed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (completed || !other.CompareTag(playerTag)) return;
        completed = true;

        Debug.Log("Escape tamamlandý!");

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.CompleteStage(GameStage.Escape);
        else
            Debug.LogError("[EscapeGoal] GameStateManager.Instance bulunamadý!");
    }
}