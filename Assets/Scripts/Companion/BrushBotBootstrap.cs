using UnityEngine;

/// <summary>
/// Fýrça Bot'un baðýmlýlýklarýný bir araya getirir ve ilk durumu baþlatýr.
/// Somut sýnýflarý yalnýzca burada birleþtiririz, durumlar arayüz görür.
/// </summary>
[RequireComponent(typeof(BrushBotAgent))]
[RequireComponent(typeof(RaycastRoseSensor))]
[RequireComponent(typeof(OrderInput))]
public class BrushBotBootstrap : MonoBehaviour
{
    void Start()
    {
        var agent = GetComponent<BrushBotAgent>();

        var context = new BotContext(
            agent,
            GetComponent<RaycastRoseSensor>(),
            new NavMeshAgentNavigator(agent.NavAgent),
            GetComponent<OrderInput>());

        agent.ChangeState(new IdleState(context));
    }
}