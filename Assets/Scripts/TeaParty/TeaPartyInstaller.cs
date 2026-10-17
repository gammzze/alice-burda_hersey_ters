using UnityEngine;

// 05_TeaParty sahnesinin baðýmlýlýklarýný tek yerde birleþtirir (composition root).
public class TeaPartyInstaller : MonoBehaviour
{
    public IGameFlow Flow { get; private set; }
    public GameActionFactory Actions { get; private set; }
    public NpcDialogueController Dialogue { get; private set; }

    private void Awake()
    {
        var rules = new DialogueRules();

        Flow = GameFlowResolver.Resolve();

        // Adým 2'de DebugHatterScene yerine gerçek sahne denetleyicisi gelecek.
        IHatterScene scene = new DebugHatterScene();

        Actions = new GameActionFactory(Flow, scene, rules);
        Dialogue = HatterDialogueFactory.Create(new OllamaClient(), rules: rules);
    }
}