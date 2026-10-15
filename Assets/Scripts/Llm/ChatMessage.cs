using System;

[Serializable]
public class ChatMessage
{
    public string role;     // system | user | assistant
    public string content;

    public ChatMessage(string role, string content)
    {
        this.role = role;
        this.content = content;
    }
}