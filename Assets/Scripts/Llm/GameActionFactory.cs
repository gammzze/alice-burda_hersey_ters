// NpcResponse -> IGameAction. Karar eþiði kodda:
// LeaveTable yalnýzca persuasion >= eþik ise üretilir, aksi halde NoneAction.
public class GameActionFactory
{
    private readonly IGameFlow _flow;
    private readonly IHatterScene _scene;
    private readonly DialogueRules _rules;

    public GameActionFactory(IGameFlow flow, IHatterScene scene, DialogueRules rules)
    {
        _flow = flow;
        _scene = scene;
        _rules = rules;
    }

    public IGameAction Create(NpcResponse response)
    {
        if (response == null)
            return new NoneAction();

        if (response.action == NpcActions.LeaveTable &&
            response.persuasion >= _rules.LeaveTableThreshold)
            return new LeaveTableAction(_flow, _scene);

        if (response.action == NpcActions.Attack)
            return new AttackAction(_scene);

        return new NoneAction();
    }
}