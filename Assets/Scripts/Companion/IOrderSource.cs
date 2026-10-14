/// <summary>
/// Oyuncudan gelen emri saðlayan kaynak. Durumlar emrin nereden
/// geldiðini (klavye, UI, test) bilmez, yalnýzca bu arayüzü bilir.
/// </summary>
public interface IOrderSource
{
    /// <summary>Yeni bir emir bekliyor mu?</summary>
    bool HasPendingOrder { get; }

    /// <summary>Bekleyen emri tüketir (bayraðý sýfýrlar).</summary>
    void ConsumeOrder();
}