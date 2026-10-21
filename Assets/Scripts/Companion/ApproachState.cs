using UnityEngine;

/// <summary>Hedef güle NavMesh ile yürür.</summary>
public class ApproachState : IAgentState
{
    private readonly BotContext ctx;
    private readonly IPaintable target;

    // Takýlma ve zaman aþýmý korumasý için eklenen alanlar
    private float approachTimer = 0f;
    private const float MaxApproachTime = 6f; // Bir güle ulaþmak için tanýnan max süre (saniye)

    public ApproachState(BotContext ctx, IPaintable target)
    {
        this.ctx = ctx;
        this.target = target;
    }

    public void Enter()
    {
        approachTimer = 0f;
        Debug.Log("[Approach] Güle yürünüyor");
        ctx.Navigator.MoveTo(target.Position);
    }

    public void Tick()
    {
        // 1. Gül bu arada boyandýysa yeniden ara
        if (target.IsPainted)
        {
            ctx.Behavior.ChangeState(new SearchRoseState(ctx));
            return;
        }

        // Zamaný artýr
        approachTimer += Time.deltaTime;

        // 2. Zaman Aþýmý / Takýlma Korumasý
        if (approachTimer >= MaxApproachTime)
        {
            Debug.LogWarning($"[Approach] Zaman aþýmý! Gül hedefine ulaþýlamadý, takýlma sayýldý.");

            // Eðer BotContext içinde bir takýlma sayacý (StuckCount) tutuyorsan burada artýrabilirsin:
            // ctx.IncrementStuckCount(); 

            // Takýldýðý için bu gülü atlayýp tekrar arama durumuna dön
            ctx.Behavior.ChangeState(new SearchRoseState(ctx));
            return;
        }

        // 3. Hedefe Varma Kontrolü
        if (ctx.Navigator.HasArrived)
        {
            Debug.Log("[Approach] Güle varýldý");
            ctx.Behavior.ChangeState(new PaintState(ctx, target));
        }
    }

    public void Exit()
    {
        // Durumdan çýkarken navigatörün hedefe kilitli kalmasýný önlemek için 
        // Navigator yapýnda varsa durdurma/sýfýrlama komutu buraya eklenebilir.
    }
}