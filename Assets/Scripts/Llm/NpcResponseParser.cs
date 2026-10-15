using System;
using UnityEngine;

// Ollama'nın ham yanıtını NpcResponse'a çevirir.
// Boş, bozuk veya eksik alanlı yanıtı "geçersiz" olarak işaretler;
// çağıran taraf geçersiz yanıtta oyun durumunu değiştirmez.
public static class NpcResponseParser
{
    private static readonly string[] RequiredKeys = { "reply", "mood", "persuasion", "action" };

    public static ParseResult Parse(string rawJson)
    {
        if (string.IsNullOrWhiteSpace(rawJson))
            return ParseResult.Fail("Boş yanıt geldi.");

        try
        {
            // 1. Katman: Ollama zarfı
            OllamaEnvelope envelope = JsonUtility.FromJson<OllamaEnvelope>(rawJson);
            if (envelope == null || envelope.message == null ||
                string.IsNullOrWhiteSpace(envelope.message.content))
                return ParseResult.Fail("Zarfta içerik yok.");

            string content = StripCodeFence(envelope.message.content);

            // JsonUtility eksik alanı sessizce varsayılan değere çevirir,
            // bu yüzden dört anahtarın varlığını ayrıca kontrol ediyoruz.
            foreach (string key in RequiredKeys)
            {
                if (!content.Contains("\"" + key + "\""))
                    return ParseResult.Fail("Eksik alan: " + key);
            }

            // 2. Katman: modelin ürettiği JSON
            NpcResponse response = JsonUtility.FromJson<NpcResponse>(content);
            if (response == null)
                return ParseResult.Fail("Yanıt ayrıştırılamadı.");

            if (string.IsNullOrWhiteSpace(response.reply))
                return ParseResult.Fail("reply alanı boş.");

            return ParseResult.Ok(response);
        }
        catch (Exception e)
        {
            return ParseResult.Fail("Bozuk JSON: " + e.Message);
        }
    }

    // Model bazen JSON'u ```json ... ``` bloğu içinde verir.
    private static string StripCodeFence(string text)
    {
        string t = text.Trim();
        if (!t.StartsWith("```")) return t;

        int firstBrace = t.IndexOf('{');
        int lastBrace = t.LastIndexOf('}');
        if (firstBrace >= 0 && lastBrace > firstBrace)
            return t.Substring(firstBrace, lastBrace - firstBrace + 1);
        return t;
    }
}