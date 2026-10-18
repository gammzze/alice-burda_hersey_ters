using UnityEngine;

public class EscapeGoal : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private MonoBehaviour gameFlowSource; // IGameFlow uygulayan obje

    private IGameFlow gameFlow;
    private bool completed;

    private void Awake()
    {
        gameFlow = gameFlowSource as IGameFlow;
        if (gameFlow == null)
            Debug.LogWarning($"{name}: gameFlowSource IGameFlow uygulamýyor. Bölüm bitiþi tetiklenmeyecek.", this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (completed || !other.CompareTag(playerTag)) return;
        completed = true;
        Debug.Log("Escape tamamlandý!");
        gameFlow?.CompleteStage(GameStage.Escape);
    }
}