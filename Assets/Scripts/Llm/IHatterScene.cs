using System;

// Çay partisi sahnesinin eylemler için sunduðu olaylar.
public interface IHatterScene
{
    void KnockOverCups();   // fincanlar devrilir
    void OpenHole();        // delik açýlýr
    void HatterAttack();    // Þapkacý saldýrýr

    // Fincanlar devrilir, delik açýlýr; sekans bitince onCompleted çaðrýlýr.
    void PlayLeaveSequence(Action onCompleted);
}
