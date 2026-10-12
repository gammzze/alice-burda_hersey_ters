using UnityEngine;
using System;
using System.Threading.Tasks;

public interface ILlmClient
{
    Task<LlmResult> SendAsync(string systemPrompt, string userMessage);
}

public class LlmResult
{
    public bool Success;
    public string RawJson;
    public string Error;
}