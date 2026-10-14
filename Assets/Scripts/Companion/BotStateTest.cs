using UnityEngine;

/// <summary>Geçici test scripti. Baðýmlýlýklarý kurar ve botu IdleState ile baþlatýr.</summary>
[RequireComponent(typeof(BrushBotAgent))]
[RequireComponent(typeof(DebugOrderSource))]
[RequireComponent(typeof(RaycastRoseSensor))]
public class BotStateTest : MonoBehaviour
{
    void Start()
    {
        var agent = GetComponent<BrushBotAgent>();

        var context = new BotContext(
            agent,
            GetComponent<RaycastRoseSensor>(),
            new NavMeshAgentNavigator(agent.NavAgent),
            GetComponent<DebugOrderSource>());

        agent.ChangeState(new IdleState(context));
    }
}