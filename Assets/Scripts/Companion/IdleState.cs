using UnityEngine;

/// <summary>Bot beklemede. Emir gelince ReceiveOrderState'e geçer.</summary>
public class IdleState : IAgentState
{
    private readonly IAgentBehavior behavior;
    private readonly IOrderSource orderSource;

    public IdleState(IAgentBehavior behavior, IOrderSource orderSource)
    {
        this.behavior = behavior;
        this.orderSource = orderSource;
    }

    public void Enter() { Debug.Log("[Idle] Emir bekleniyor"); }

    public void Tick()
    {
        if (orderSource.HasPendingOrder)
            behavior.ChangeState(new ReceiveOrderState(behavior, orderSource));
    }

    public void Exit() { }
}