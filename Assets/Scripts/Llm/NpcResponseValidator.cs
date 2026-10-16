using System;
using UnityEngine;

// Modelin ürettiði deðerleri doðrular ve güvenli hale getirir.
public static class NpcResponseValidator
{
    // LeaveTable için gereken minimum toplam ikna seviyesi
    public const float LeaveTableThreshold = 0.7f;

    // Deðer/ölçek düzeltme. Eþik kontrolü yapmaz.
    public static NpcResponse Normalize(NpcResponse response)
    {
        if (response == null)
            return null;

        // Model bazen 0-100 ölçeðinde verir (örn. 70). 0-1'e çevir, sonra kýrp.
        if (response.persuasion > 1f && response.persuasion <= 100f)
            response.persuasion /= 100f;
        response.persuasion = Mathf.Clamp01(response.persuasion);

        response.mood = NormalizeMood(response.mood);
        response.action = NormalizeAction(response.action);
        return response;
    }

    // Normalize + eþik kontrolü. persuasion burada TOPLAM seviye olmalý.
    public static NpcResponse Validate(NpcResponse response)
    {
        if (response == null)
            return null;

        Normalize(response);

        if (response.action == NpcActions.LeaveTable &&
            response.persuasion < LeaveTableThreshold)
        {
            Debug.LogWarning("Validator: LeaveTable reddedildi, persuasion yetersiz (" +
                             response.persuasion.ToString("F2") + ").");
            response.action = NpcActions.None;
        }

        return response;
    }

    private static string NormalizeMood(string mood)
    {
        if (Same(mood, NpcMoods.Angry)) return NpcMoods.Angry;
        if (Same(mood, NpcMoods.Happy)) return NpcMoods.Happy;
        if (Same(mood, NpcMoods.Suspicious)) return NpcMoods.Suspicious;
        return NpcMoods.Neutral;
    }

    private static string NormalizeAction(string action)
    {
        if (Same(action, NpcActions.LeaveTable)) return NpcActions.LeaveTable;
        if (Same(action, NpcActions.Attack)) return NpcActions.Attack;
        return NpcActions.None;
    }

    private static bool Same(string a, string b) =>
        string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
}