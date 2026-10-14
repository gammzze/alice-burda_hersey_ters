using System;

// Ollama /api/chat yanýtýnýn dýþ zarfý.
// Sadece ihtiyacýmýz olan alanlarý tanýmlýyoruz, geri kalaný JsonUtility yok sayar.
[Serializable]
public class OllamaEnvelope
{
    public OllamaMessage message;
}

[Serializable]
public class OllamaMessage
{
    public string role;
    public string content;   // modelin ürettiði JSON metni burada
}
