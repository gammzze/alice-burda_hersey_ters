using UnityEngine;

/// <summary>Bot beklemede. Emir gelince ReceiveOrderState'e geçer.</summary>
public class IdleState : IAgentState
{
    private readonly BotContext ctx;

    public IdleState(BotContext ctx)
    {
        this.ctx = ctx;
    }

    public void Enter() { Debug.Log("[Idle] Emir bekleniyor"); }

    public void Tick()
    {
        if (ctx.Orders.HasPendingOrder)
            ctx.Behavior.ChangeState(new ReceiveOrderState(ctx));
    }

    public void Exit() { }
}