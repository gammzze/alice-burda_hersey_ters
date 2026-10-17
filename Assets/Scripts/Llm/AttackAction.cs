// Þapkacý saldýrýr. Þimdilik sadece sahneye bildirir, oyun bitmez.
public class AttackAction : IGameAction
{
    private readonly IHatterScene _scene;

    public AttackAction(IHatterScene scene)
    {
        _scene = scene;
    }

    public void Execute()
    {
        _scene.HatterAttack();
    }
}
