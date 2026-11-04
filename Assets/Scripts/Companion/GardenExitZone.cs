using UnityEngine;

public class GardenExitZone : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    private bool completed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckAndComplete(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckAndComplete(other);
    }

    private void CheckAndComplete(Collider2D other)
    {
        if (completed || !other.CompareTag(playerTag)) return;
        completed = true;

        Debug.Log("[GardenExit] Alice kapidan cikti.");

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.CompleteStage(GameStage.Garden);
        else
            Debug.LogError("[GardenExit] GameStateManager.Instance bulunamadi!");
    }
}