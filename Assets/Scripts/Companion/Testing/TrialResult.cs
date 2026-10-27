/// <summary>Tek bir denemenin sonucu.</summary>
public readonly struct TrialResult
{
    public int Trial { get; }
    public string StartLabel { get; }
    public float StartX { get; }
    public float StartY { get; }
    public int RoseCount { get; }
    public int PaintedCount { get; }
    public bool Success { get; }
    public float Duration { get; }
    public int StuckCount { get; }

    public TrialResult(int trial, string startLabel, float startX, float startY,
                       int roseCount, int paintedCount, bool success,
                       float duration, int stuckCount)
    {
        Trial = trial; StartLabel = startLabel; StartX = startX; StartY = startY;
        RoseCount = roseCount; PaintedCount = paintedCount; Success = success;
        Duration = duration; StuckCount = stuckCount;
    }
}