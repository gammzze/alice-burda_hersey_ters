using System;
using UnityEngine;

// Ollama'nın ham yanıtını NpcResponse'a çevirir.
// Boş, bozuk veya eksik alanlı yanıtı "geçersiz" olarak işaretler.
public class NpcResponseParser : IResponseParser
{
    private static readonly string[] RequiredKeys = { "reply", "mood", "persuasion", "action" };

    public ParseResult Parse(string rawJson)
    {
        if (string.IsNullOrWhiteSpace(rawJson))
            return ParseResult.Fail("Boş yanıt geldi.");

        try
        {
            OllamaEnvelope envelope = JsonUtility.FromJson<OllamaEnvelope>(rawJson);
            if (envelope == null || envelope.message == null ||
                string.IsNullOrWhiteSpace(envelope.message.content))
                return ParseResult.Fail("Zarfta içerik yok.");

            string content = StripCodeFence(envelope.message.content);

            foreach (string key in RequiredKeys)
            {
                if (!content.Contains("\"" + key + "\""))
                    return ParseResult.Fail("Eksik alan: " + key);
            }

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