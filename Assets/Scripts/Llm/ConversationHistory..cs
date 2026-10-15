using System.Collections.Generic;

public class ConversationHistory
{
    private readonly List<ChatMessage> _messages = new List<ChatMessage>();

    public void AddUser(string text) => _messages.Add(new ChatMessage("user", text));
    public void AddAssistant(string text) => _messages.Add(new ChatMessage("assistant", text));

    // Son maxCount mesajý döner
    public IReadOnlyList<ChatMessage> Recent(int maxCount)
    {
        int start = System.Math.Max(0, _messages.Count - maxCount);
        return _messages.GetRange(start, _messages.Count - start);
    }

    public void Clear() => _messages.Clear();
}