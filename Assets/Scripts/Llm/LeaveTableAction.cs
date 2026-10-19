using UnityEngine;

// Þapkacý masadan kalkar. Sahne sekansý bitince TeaParty aþamasý tamamlanýr.
public class LeaveTableAction : IGameAction
{
    private readonly IGameFlow _flow;
    private readonly IHatterScene _scene;
    private bool _executed;

    public LeaveTableAction(IGameFlow flow, IHatterScene scene)
    {
        _flow = flow;
        _scene = scene;
    }

    public void Execute()
    {
        if (_executed) return;   // iki kez çalýþmasýn
        _executed = true;

        _scene.PlayLeaveSequence(OnSequenceCompleted);
    }

    private void OnSequenceCompleted()
    {
        _flow.CompleteStage(GameStage.TeaParty);
        Debug.Log("LeaveTableAction: TeaParty tamamlandý.");
    }
}
