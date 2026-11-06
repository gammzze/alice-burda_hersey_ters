using UnityEngine;

// Dokümanda her sahne tek baþýna test edilebilmeli. Butonlar varsayýlan olarak GÝZLÝDÝR;
// editörde ve development build'de F9 ile açýlýp kapanýr. Son build'de hiç derlenmez.
public class TeaPartyDebugButtons : MonoBehaviour
{
    [SerializeField] private TeaPartyInstaller installer;
    [SerializeField] private TeaPartySceneController sceneController;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private bool _visible;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9)) _visible = !_visible;
    }

    private void OnGUI()
    {
        if (!_visible) return;

        GUILayout.BeginArea(new Rect(10, 10, 220, 200));
        if (GUILayout.Button("Debug: LeaveTable")) Fire(NpcActions.LeaveTable, 0.9f);
        if (GUILayout.Button("Debug: Attack")) Fire(NpcActions.Attack, 0f);
        if (GUILayout.Button("Debug: Reset")) sceneController.ResetScene();
        GUILayout.EndArea();
    }

    private void Fire(string action, float persuasion)
    {
        var response = new NpcResponse
        {
            reply = "debug",
            mood = NpcMoods.Neutral,
            persuasion = persuasion,
            action = action
        };
        installer.Actions.Create(response).Execute();
    }
#endif
}