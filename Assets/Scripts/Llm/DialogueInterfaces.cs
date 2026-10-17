using System.Collections.Generic;

// Controller'ýn baðýmlý olduðu soyutlamalar.
public interface IResponseParser
{
    ParseResult Parse(string rawJson);
}

public interface IResponseValidator
{
    NpcResponse Normalize(NpcResponse response);   // deðer/ölçek düzeltme
    NpcResponse Validate(NpcResponse response);    // Normalize + eþik kontrolü
}

public interface IPromptBuilder
{
    List<ChatMessage> Build(ConversationHistory history, float currentPersuasion);
}

public interface IPersuasionTracker
{
    float Current { get; }
    float Apply(float turnScore, string mood);
    void Reset();
}