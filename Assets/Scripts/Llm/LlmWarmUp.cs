using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// Modeli belleðe yüklemek için arka planda kýsa bir istek atar.
// Sonucu kullanýlmaz; baþarýsýz olursa oyun etkilenmez.
public class LlmWarmup
{
    private readonly ILlmClient _client;

    public LlmWarmup(ILlmClient client)
    {
        _client = client;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var messages = new List<ChatMessage> { new ChatMessage("user", "Merhaba") };
        LlmResult result = await _client.SendAsync(messages, cancellationToken);

        if (result.Success)
            Debug.Log("Isýnma isteði tamamlandý, model hazýr.");
        else
            Debug.LogWarning("Isýnma isteði baþarýsýz (oyun etkilenmez): " + result.Error);
    }
}