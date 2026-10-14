using UnityEngine;

/// <summary>Emri alýr, tüketir ve gül aramaya geçer.</summary>
public class ReceiveOrderState : IAgentState
{
    private readonly BotContext ctx;

    public ReceiveOrderState(BotContext ctx)
    {
        this.ctx = ctx;
    }

    public void Enter()
    {
        ctx.Orders.ConsumeOrder();
        Debug.Log("[ReceiveOrder] Emir alýndý");
    }

    public void Tick()
    {
        ctx.Behavior.ChangeState(new SearchRoseState(ctx));
    }

    public void Exit() { }
}