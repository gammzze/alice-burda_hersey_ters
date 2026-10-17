using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// Test için: sýrayla önceden hazýrlanmýþ yanýtlarý döner. Ollama gerekmez.
public class FakeLlmClient : ILlmClient
{
    private readonly Queue<LlmResult> _results;

    public FakeLlmClient(params LlmResult[] results)
    {
        _results = new Queue<LlmResult>(results);
    }

    public Task<LlmResult> SendAsync(IReadOnlyList<ChatMessage> messages,
                                     CancellationToken cancellationToken = default)
    {
        LlmResult next = _results.Count > 0
            ? _results.Dequeue()
            : new LlmResult { Success = false, Error = "Fake: yanýt kalmadý." };
        return Task.FromResult(next);
    }
}
