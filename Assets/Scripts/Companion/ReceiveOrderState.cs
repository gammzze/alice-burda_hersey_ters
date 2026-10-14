using UnityEngine;

/// <summary>Emri alýr ve tüketir. Þimdilik test için Idle'a döner.</summary>
public class ReceiveOrderState : IAgentState
{
    private readonly IAgentBehavior behavior;
    private readonly IOrderSource orderSource;

    public ReceiveOrderState(IAgentBehavior behavior, IOrderSource orderSource)
    {
        this.behavior = behavior;
        this.orderSource = orderSource;
    }

    public void Enter()
    {
        orderSource.ConsumeOrder();
        Debug.Log("[ReceiveOrder] Emir alýndý");
    }

    public void Tick()
    {
        // TODO: SearchRoseState'e geçilecek. Þimdilik test için Idle'a dön.
        behavior.ChangeState(new IdleState(behavior, orderSource));
    }

    public void Exit() { }
}