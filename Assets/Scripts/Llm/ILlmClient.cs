using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILlmClient
{
    Task<LlmResult> SendAsync(IReadOnlyList<ChatMessage> messages);
}

public class LlmResult
{
    public bool Success;
    public string RawJson;
    public string Error;
}