//sadece test için, log yazar.
using UnityEngine;

public class DebugHatterScene : IHatterScene
{
    public void KnockOverCups() => Debug.Log("[Sahne] Fincanlar devrildi.");
    public void OpenHole() => Debug.Log("[Sahne] Delik açýldý.");
    public void HatterAttack() => Debug.Log("[Sahne] Þapkacý saldýrdý.");
}