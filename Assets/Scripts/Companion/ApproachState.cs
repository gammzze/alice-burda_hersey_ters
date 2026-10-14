using UnityEngine;

/// <summary>Hedef güle NavMesh ile yürür.</summary>
public class ApproachState : IAgentState
{
    private readonly BotContext ctx;
    private readonly IPaintable target;

    public ApproachState(BotContext ctx, IPaintable target)
    {
        this.ctx = ctx;
        this.target = target;
    }

    public void Enter()
    {
        Debug.Log("[Approach] Güle yürünüyor");
        ctx.Navigator.MoveTo(target.Position);
    }

    public void Tick()
    {
        // Gül bu arada boyandýysa yeniden ara
        if (target.IsPainted)
        {
            ctx.Behavior.ChangeState(new SearchRoseState(ctx));
            return;
        }

        if (ctx.Navigator.HasArrived)
        {
            Debug.Log("[Approach] Güle varýldý");
            ctx.Behavior.ChangeState(new PaintState(ctx, target));
        }
    }

    public void Exit() { }
}