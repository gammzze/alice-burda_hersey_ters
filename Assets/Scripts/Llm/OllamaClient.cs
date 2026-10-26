using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class OllamaClient : ILlmClient
{
    private readonly string _url;
    private readonly string _model;
    private readonly int _timeoutSeconds;

    public OllamaClient(string model = "gemma3:4b",
                        string url = "http://localhost:11434/api/chat",
                        int timeoutSeconds = 20)
    {
        _model = model;
        _url = url;
        _timeoutSeconds = timeoutSeconds;
    }

    public async Task<LlmResult> SendAsync(IReadOnlyList<ChatMessage> messages,
                                       CancellationToken cancellationToken = default)
    {
        try
        {
            string body = BuildBody(messages);

            using (var req = new UnityWebRequest(_url, "POST"))
            {
                req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");
                req.timeout = _timeoutSeconds;

                var op = req.SendWebRequest();
                while (!op.isDone)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        req.Abort();
                        return new LlmResult { Success = false, Error = "Ýstek iptal edildi." };
                    }
                    await Task.Yield();   // frame'i bloklamaz
                }

                if (req.result != UnityWebRequest.Result.Success)
                    return new LlmResult { Success = false, Error = req.error };

                return new LlmResult { Success = true, RawJson = req.downloadHandler.text };
            }
        }
        catch (Exception e)
        {
            // Sözleþme: istemci exception fýrlatmaz, hatayý LlmResult olarak döner.
            return new LlmResult { Success = false, Error = "Ýstemci hatasý: " + e.Message };
        }
    }

    private string BuildBody(IReadOnlyList<ChatMessage> messages)
    {
        var payload = new ChatRequest
        {
            model = _model,
            stream = false,
            format = "json",
            messages = new List<ChatMessage>(messages).ToArray()
        };
        return JsonUtility.ToJson(payload);
    }

    [Serializable]
    private class ChatRequest
    {
        public string model;
        public bool stream;
        public string format;
        public ChatMessage[] messages;
    }
}