using System;
using System.Text;
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

    public async Task<LlmResult> SendAsync(string systemPrompt, string userMessage)
    {
        string body = BuildBody(systemPrompt, userMessage);

        using (var req = new UnityWebRequest(_url, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.timeout = _timeoutSeconds;

            var op = req.SendWebRequest();
            while (!op.isDone)
                await Task.Yield();   // frame'i bloklamaz

            if (req.result != UnityWebRequest.Result.Success)
            {
                return new LlmResult { Success = false, Error = req.error };
            }

            return new LlmResult { Success = true, RawJson = req.downloadHandler.text };
        }
    }

    private string BuildBody(string systemPrompt, string userMessage)
    {
        var payload = new ChatRequest
        {
            model = _model,
            stream = false,
            format = "json",
            messages = new[]
            {
                new ChatMessage { role = "system", content = systemPrompt },
                new ChatMessage { role = "user", content = userMessage }
            }
        };
        return JsonUtility.ToJson(payload);
    }

    [Serializable] private class ChatMessage { public string role; public string content; }
    [Serializable]
    private class ChatRequest
    {
        public string model;
        public bool stream;
        public string format;
        public ChatMessage[] messages;
    }
}
