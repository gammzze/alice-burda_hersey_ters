using UnityEngine;

/// <summary>Geçici test scripti. BrushBotAgent'a DebugState verir.</summary>
[RequireComponent(typeof(BrushBotAgent))]
public class BotStateTest : MonoBehaviour
{
    void Start()
    {
        var agent = GetComponent<BrushBotAgent>();
        agent.ChangeState(new DebugState());
    }
}