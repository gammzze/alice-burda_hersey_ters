using System;
using UnityEngine;

#pragma warning disable CS0067
public class DebugGameFlow : IGameFlow
{
    public GameStage Current { get; private set; } = GameStage.TeaParty;
    public event Action<GameStage> OnStageChanged;

    public void CompleteStage(GameStage stage)
    {
        Debug.Log("[Flow] CompleteStage: " + stage);
    }

    public void EndGame()
    {
        Debug.Log("[Flow] EndGame");
    }
}
#pragma warning restore CS0067