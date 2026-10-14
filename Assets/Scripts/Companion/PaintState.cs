using UnityEngine;

/// <summary>Gülü boyar. Kýsa bir boyama süresi bekler, sonra yeni gül arar.</summary>
public class PaintState : IAgentState
{
    private const float PaintDuration = 1f;

    private readonly BotContext ctx;
    private readonly IPaintable target;
    private float elapsed;

    public PaintState(BotContext ctx, IPaintable target)
    {
        this.ctx = ctx;
        this.target = target;
    }

    public void Enter()
    {
        Debug.Log("[Paint] Boyama baþladý");
        ctx.Navigator.Stop();
        elapsed = 0f;
    }

    public void Tick()
    {
        elapsed += Time.deltaTime;
        if (elapsed < PaintDuration) return;

        target.Paint();
        ctx.Behavior.ChangeState(new SearchRoseState(ctx));
    }

    public void Exit() { }
}