using UnityEngine;
using System.Linq;

public class GardenGateController : MonoBehaviour
{
    private Rose[] rosesInGarden;
    private bool stageCompleted = false;

    void Start()
    {
        // Sahnedeki tüm Rose bileþenine sahip nesneleri bulur
        rosesInGarden = FindObjectsByType<Rose>(FindObjectsSortMode.None);

        if (rosesInGarden.Length == 0)
        {
            Debug.LogWarning("[GardenGateController] Sahnede hiç gül bulunamadý!");
        }
    }

    void Update()
    {
        if (stageCompleted || rosesInGarden == null || rosesInGarden.Length == 0)
            return;

        // Tüm güller boyandý mý kontrol et (Örn: IsPainted veya ilgili bayrak)
       
        bool allPainted = rosesInGarden.All(rose => rose.IsPainted); // Örnek özellik adý

        if (allPainted)
        {
            stageCompleted = true;
            TriggerGardenCompletion();
        }
    }

    void TriggerGardenCompletion()
    {
        Debug.Log("[Completed] Tüm güller boyandý! Kapý açýlýyor ve Garden aþamasý tamamlanýyor.");

        // GameStateManager üzerinden aþamayý tamamlama
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.CompleteStage(GameStage.Garden);
        }
        else
        {
            Debug.LogError("[GardenGateController] GameStateManager örneði bulunamadý!");
        }
    }
}