using UnityEngine;

// Modelin ürettiði deðerleri doðrular ve güvenli hale getirir.
// Küçük modeller kurallara her zaman uymadýðý için son karar kodda verilir.
public static class NpcResponseValidator
{
    // open_door için gereken minimum ikna seviyesi
    public const int OpenDoorThreshold = 80;

    public static NpcResponse Validate(NpcResponse response)
    {
        if (response == null)
            return null;

        // persuasion 0-100 aralýðýna kýsýlýr
        response.persuasion = Mathf.Clamp(response.persuasion, 0, 100);

        // bilinmeyen mood deðeri neutral olur
        if (response.mood != NpcMoods.Neutral &&
            response.mood != NpcMoods.Angry &&
            response.mood != NpcMoods.Happy &&
            response.mood != NpcMoods.Suspicious)
        {
            response.mood = NpcMoods.Neutral;
        }

        // bilinmeyen action deðeri none olur
        if (response.action != NpcActions.None &&
            response.action != NpcActions.OpenDoor &&
            response.action != NpcActions.Attack)
        {
            response.action = NpcActions.None;
        }

        // ikna yetersizse kapý açýlmaz
        if (response.action == NpcActions.OpenDoor &&
            response.persuasion < OpenDoorThreshold)
        {
            Debug.LogWarning("Validator: open_door reddedildi, persuasion yetersiz (" +
                             response.persuasion + ").");
            response.action = NpcActions.None;
        }

        return response;
    }
}