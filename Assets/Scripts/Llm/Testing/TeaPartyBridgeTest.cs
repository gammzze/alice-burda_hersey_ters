using UnityEngine;

// Köprüyü Ollama'sýz dener.
public class TeaPartyBridgeTest : MonoBehaviour
{
    [ContextMenu("Log Flow")]
    private void LogFlow()
    {
        var installer = FindFirstObjectByType<TeaPartyInstaller>();
        Debug.Log($"Flow: {installer.Flow.GetType().Name} | Current: {installer.Flow.Current}");
    }

    [ContextMenu("Fire LeaveTable")]
    private void FireLeaveTable()
    {
        var installer = FindFirstObjectByType<TeaPartyInstaller>();
        var response = new NpcResponse
        {
            reply = "test",
            mood = NpcMoods.Happy,
            persuasion = 0.9f,
            action = NpcActions.LeaveTable
        };
        installer.Actions.Create(response).Execute();
    }
}
