using UnityEngine;
using System.Linq;

/// <summary>Sensörle en yakýn boyanmamýþ gülü arar, erken bitiþ riskine karþý korunmuþtur.</summary>
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
        // 1. Önce yerel sensör ile en yakýn gülü bulmayý dene
        IPaintable rose = ctx.Sensor.FindNearestUnpaintedRose();

        if (rose == null)
        {
            // 2. Sensör yerelde gül bulamadýysa, haritada hala boyanmamýþ baþka gül var mý global olarak kontrol et
            Rose[] allRoses = Object.FindObjectsByType<Rose>(FindObjectsSortMode.None);
            var remainingRose = allRoses?.FirstOrDefault(r => !r.IsPainted);

            if (remainingRose != null)
            {
                // Sahnede hala boyanmamýþ güller var ancak sensörün doðrudan görüþ alanýnda deðil.
                // Erken bitiþi önlemek için global olarak kalan ilk güle doðru yaklaþma durumuna geçiyoruz.
                Debug.Log("[Search] Yakýnda gül görünmüyor ancak haritada kalan boyanmamýþ gül var, yönlendiriliyor.");
                ctx.Behavior.ChangeState(new ApproachState(ctx, remainingRose));
            }
            else
            {
                // Gerçekten sahnedeki tüm güller boyandý, güvenle tamamlanma durumuna geçebiliriz.
                Debug.Log("[Search] Sahnede boyanmamýþ hiç gül kalmadý, aþama tamamlanýyor.");
                ctx.Behavior.ChangeState(new CompletedState(ctx));
            }
            return;
        }

        // Gül bulundu, yaklaþma durumuna geç
        Debug.Log("[Search] Gül bulundu");
        ctx.Behavior.ChangeState(new ApproachState(ctx, rose));
    }

    public void Exit() { }
}