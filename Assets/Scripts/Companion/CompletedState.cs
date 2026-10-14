using UnityEngine;

/// <summary>Boyanacak gül kalmadý. Yeni emir gelirse yeniden arar.</summary>
public class CompletedState : IAgentState
{
    private readonly BotContext ctx;

    public CompletedState(BotContext ctx)
    {
        this.ctx = ctx;
    }

    public void Enter()
    {
        Debug.Log("[Completed] Tüm güller boyandý");
        ctx.Navigator.Stop();
    }

    public void Tick()
    {
        if (ctx.Orders.HasPendingOrder)
            ctx.Behavior.ChangeState(new ReceiveOrderState(ctx));
    }

    public void Exit() { }
}