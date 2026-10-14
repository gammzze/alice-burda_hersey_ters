using UnityEngine;

/// <summary>Emri alýr ve tüketir. Þimdilik test için Idle'a döner.</summary>
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
        // TODO (Adým 7B): SearchRoseState'e geçilecek. Þimdilik test için Idle'a dön.
        ctx.Behavior.ChangeState(new IdleState(ctx));
    }

    public void Exit() { }
}