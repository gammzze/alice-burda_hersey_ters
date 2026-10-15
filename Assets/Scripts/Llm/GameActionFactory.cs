// NpcResponse -> IGameAction. Karar eþiði kodda:
// LeaveTable yalnýzca persuasion >= eþik ise üretilir, aksi halde NoneAction.
public class GameActionFactory
{
    private readonly IGameFlow _flow;
    private readonly IHatterScene _scene;

    public GameActionFactory(IGameFlow flow, IHatterScene scene)
    {
        _flow = flow;
        _scene = scene;
    }

    public IGameAction Create(NpcResponse response)
    {
        if (response == null)
            return new NoneAction();

        if (response.action == NpcActions.LeaveTable &&
            response.persuasion >= NpcResponseValidator.LeaveTableThreshold)
            return new LeaveTableAction(_flow, _scene);

        if (response.action == NpcActions.Attack)
            return new AttackAction(_scene);

        return new NoneAction();
    }
}
