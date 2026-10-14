using UnityEngine;

/// <summary>
/// Ajanýn hareket sözleþmesi. Durumlar hareketin NavMesh ile mi baþka
/// bir þekilde mi yapýldýðýný bilmez.
/// </summary>
public interface INavigator
{
    /// <summary>Hedefe yürümeye baþlar.</summary>
    void MoveTo(Vector2 target);

    /// <summary>Hedefe vardý mý?</summary>
    bool HasArrived { get; }

    /// <summary>Hareketi durdurur.</summary>
    void Stop();
}