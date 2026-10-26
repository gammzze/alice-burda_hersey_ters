using System;
using UnityEngine;

public class DebugHatterScene : IHatterScene
{
    public void KnockOverCups() => Debug.Log("[Sahne] Fincanlar devrildi.");
    public void OpenHole() => Debug.Log("[Sahne] Delik açýldý.");
    public void HatterAttack() => Debug.Log("[Sahne] Þapkacý saldýrdý.");

    public void PlayLeaveSequence(Action onCompleted)
    {
        KnockOverCups();
        OpenHole();
        onCompleted?.Invoke();   // debug sahnede beklemeden tamamlanýr
    }
}