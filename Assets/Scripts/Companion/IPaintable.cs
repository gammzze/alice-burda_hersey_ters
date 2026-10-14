using UnityEngine;

/// <summary>
/// Boyanabilen bir nesne (gül). Sensör ve durumlar somut Rose sýnýfýný
/// bilmez, yalnýzca bu arayüzü bilir.
/// </summary>
public interface IPaintable
{
    /// <summary>Boyanmýþ mý?</summary>
    bool IsPainted { get; }

    /// <summary>Dünya konumu (botun yaklaþacaðý nokta).</summary>
    Vector2 Position { get; }

    /// <summary>Nesneyi boyar.</summary>
    void Paint();
}