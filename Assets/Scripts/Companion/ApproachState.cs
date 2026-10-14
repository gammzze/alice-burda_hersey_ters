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
            // TODO (Adým 8): PaintState'e geçilecek. Þimdilik Idle'a dön.
            Debug.Log("[Approach] Güle varýldý");
            ctx.Behavior.ChangeState(new IdleState(ctx));
        }
    }

    public void Exit() { ctx.Navigator.Stop(); }
}