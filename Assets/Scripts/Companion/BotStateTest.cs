using UnityEngine;

/// <summary>Geçici test scripti. BrushBotAgent'ý IdleState ile baþlatýr.</summary>
[RequireComponent(typeof(BrushBotAgent))]
[RequireComponent(typeof(DebugOrderSource))]
public class BotStateTest : MonoBehaviour
{
    void Start()
    {
        var agent = GetComponent<BrushBotAgent>();
        var orders = GetComponent<DebugOrderSource>();
        agent.ChangeState(new IdleState(agent, orders));
    }
}