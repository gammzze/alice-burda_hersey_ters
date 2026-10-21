using UnityEngine;

// 05_TeaParty sahnesinin baðýmlýlýklarýný tek yerde birleþtirir (composition root).
public class TeaPartyInstaller : MonoBehaviour
{
    [SerializeField] private TeaPartySceneController sceneController;

    public IGameFlow Flow { get; private set; }
    public IHatterScene Scene { get; private set; }
    public GameActionFactory Actions { get; private set; }
    public NpcDialogueController Dialogue { get; private set; }
    public LlmWarmup Warmup { get; private set; }

    private void Awake()
    {
        var rules = new DialogueRules();

        Flow = GameFlowResolver.Resolve();

        if (sceneController != null)
        {
            Scene = sceneController;
        }
        else
        {
            Debug.LogWarning("TeaPartySceneController atanmamýþ, debug sahne kullanýlýyor.");
            Scene = new DebugHatterScene();
        }

        Actions = new GameActionFactory(Flow, Scene, rules);
        Dialogue = HatterDialogueFactory.Create(new OllamaClient(), rules: rules);

        // Isýnma isteði ilk model yüklemesini kapsayacaðý için daha uzun timeout.
        Warmup = new LlmWarmup(new OllamaClient(timeoutSeconds: 60));
    }
}