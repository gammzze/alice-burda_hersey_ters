/// <summary>
/// Çevredeki gülleri algýlayan sensörün sözleþmesi.
/// Durumlar sensörün nasýl çalýþtýðýný (Raycast, vb.) bilmez.
/// </summary>
public interface IRoseSensor
{
    /// <summary>En yakýn boyanmamýþ gülü döndürür, yoksa null.</summary>
    IPaintable FindNearestUnpaintedRose();
}