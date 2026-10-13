/// <summary>
/// Otonom ajanýn tek bir durumunu temsil eder (State deseni).
/// Her durum kendi sýnýfýnda yaþar ve yalnýzca kendi iþinden sorumludur.
/// </summary>
public interface IAgentState
{
    /// <summary>Duruma girildiðinde bir kez çaðrýlýr.</summary>
    void Enter();

    /// <summary>Durum aktifken her karede çaðrýlýr.</summary>
    void Tick();

    /// <summary>Durumdan çýkýlýrken bir kez çaðrýlýr.</summary>
    void Exit();
}