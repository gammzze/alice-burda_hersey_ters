using UnityEngine;

// Ýkna seviyesini kod tarafýnda biriktirir.
// Model yalnýzca son mesajýn ikna gücünü (0-1) söyler; toplamý kod hesaplar.
public class PersuasionTracker
{
    public const float GainFactor = 0.4f;        // tur ikna gücü * 0.4
    public const float MaxIncreasePerTurn = 0.2f;
    public const float AngryPenalty = 0.1f;

    public float Current { get; private set; }

    public float Apply(float turnScore, string mood = null)
    {
        float delta = Mathf.Min(Mathf.Clamp01(turnScore) * GainFactor, MaxIncreasePerTurn);

        if (mood == NpcMoods.Angry)
            delta = -AngryPenalty;

        Current = Mathf.Clamp01(Current + delta);
        return Current;
    }

    public void Reset()
    {
        Current = 0f;
    }
}