using UnityEngine;

public class OllamaTest : MonoBehaviour
{
    async void Start()
    {
        ILlmClient client = new OllamaClient();
        var result = await client.SendAsync(
            "Sen bir muhafýzsýn. Sadece JSON dön: {\"reply\":\"...\"}",
            "Merhaba");

        if (result.Success) Debug.Log(result.RawJson);
        else Debug.LogWarning("LLM hatasý: " + result.Error);
    }
}