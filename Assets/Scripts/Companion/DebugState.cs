using UnityEngine;

/// <summary>Geçici test durumu. Durum makinesinin çalýþtýðýný doðrulamak için.</summary>
public class DebugState : IAgentState
{
    private int tickCount;

    public void Enter() { Debug.Log("[DebugState] Enter"); }

    public void Tick()
    {
        tickCount++;
        if (tickCount == 1) Debug.Log("[DebugState] Ýlk Tick çalýþtý");
    }

    public void Exit() { Debug.Log("[DebugState] Exit"); }
}