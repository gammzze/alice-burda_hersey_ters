using System;
using UnityEngine;

// Ollama'dan gelen ham yanýtý (zarf) NpcResponse nesnesine çevirir.
// Bozuk, boþ veya beklenmedik bir yanýtta oyunu çökertmek yerine
// varsayýlan bir yanýt döner.
public static class NpcResponseParser
{
    public static NpcResponse Parse(string rawJson)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(rawJson))
                return Fallback("Boþ yanýt geldi.");

            // 1. Katman: Ollama zarfý
            OllamaEnvelope envelope = JsonUtility.FromJson<OllamaEnvelope>(rawJson);
            if (envelope == null || envelope.message == null ||
                string.IsNullOrWhiteSpace(envelope.message.content))
                return Fallback("Zarfta içerik yok.");

            // 2. Katman: modelin ürettiði JSON
            NpcResponse response = JsonUtility.FromJson<NpcResponse>(envelope.message.content);
            if (response == null || string.IsNullOrWhiteSpace(response.reply))
                return Fallback("Model geçerli bir yanýt üretmedi.");

            return response;
        }
        catch (Exception e)
        {
            Debug.LogWarning("NpcResponseParser hatasý: " + e.Message);
            return Fallback("Yanýt ayrýþtýrýlamadý.");
        }
    }

    // Hata durumunda oyunun devam etmesini saðlayan güvenli yanýt.
    private static NpcResponse Fallback(string reason)
    {
        Debug.LogWarning("Parser fallback: " + reason);
        return new NpcResponse
        {
            reply = "...",
            mood = NpcMoods.Neutral,
            persuasion = 0,
            action = NpcActions.None
        };
    }
}