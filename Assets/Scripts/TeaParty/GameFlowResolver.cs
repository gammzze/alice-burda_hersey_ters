using UnityEngine;

// Sahneye baðlanacak IGameFlow'u seçer.
// Gerçek GameStateManager varsa onu kullanýr; yoksa (sahne tek baþýna açýldýysa) debug akýþý.
public static class GameFlowResolver
{
    public static IGameFlow Resolve()
    {
        if (GameStateManager.Instance != null)
            return GameStateManager.Instance;

        Debug.LogWarning("GameStateManager bulunamadý, debug akýþý kullanýlýyor (sahne tek baþýna açýlmýþ).");
        return new DebugGameFlow();
    }
}