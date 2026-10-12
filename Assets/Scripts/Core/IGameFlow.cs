using System;

public interface IGameFlow
{
    GameStage Current { get; }
    event Action<GameStage> OnStageChanged;
    void CompleteStage(GameStage stage);
    void EndGame();
}