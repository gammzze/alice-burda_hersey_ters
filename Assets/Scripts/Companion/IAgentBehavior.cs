/// <summary>
/// Durum makinesinin sözleþmesi. Durumlar ajanýn somut sýnýfýný bilmez,
/// yalnýzca bu arayüz üzerinden durum deðiþtirir (Dependency Inversion).
/// </summary>
public interface IAgentBehavior
{
    /// <summary>Þu an aktif olan durum.</summary>
    IAgentState CurrentState { get; }

    /// <summary>Mevcut durumdan çýkar (Exit), yenisine girer (Enter).</summary>
    void ChangeState(IAgentState newState);
}