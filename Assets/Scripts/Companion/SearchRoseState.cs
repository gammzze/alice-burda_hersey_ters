using UnityEngine;

/// <summary>Sensörle en yakýn boyanmamýþ gülü arar.</summary>
public class SearchRoseState : IAgentState
{
    private readonly BotContext ctx;

    public SearchRoseState(BotContext ctx)
    {
        this.ctx = ctx;
    }

    public void Enter() { Debug.Log("[Search] Gül aranýyor"); }

    public void Tick()
    {
        IPaintable rose = ctx.Sensor.FindNearestUnpaintedRose();

        if (rose == null)
        {
            Debug.Log("[Search] Gül bulunamadý");
            ctx.Behavior.ChangeState(new CompletedState(ctx));
            return;
        }

        Debug.Log("[Search] Gül bulundu");
        ctx.Behavior.ChangeState(new ApproachState(ctx, rose));
    }

    public void Exit() { }
}