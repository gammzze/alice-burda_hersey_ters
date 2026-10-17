using System;

[Serializable]
public class NpcResponse
{
    public string reply;      // Þapkacý'nýn söylediði cümle
    public string mood;       // neutral, angry, happy, suspicious
    public float persuasion;  // 0.0 - 1.0 arasý ikna seviyesi
    public string action;     // None, LeaveTable, Attack
}

public static class NpcMoods
{
    public const string Neutral = "neutral";
    public const string Angry = "angry";
    public const string Happy = "happy";
    public const string Suspicious = "suspicious";
}

public static class NpcActions
{
    public const string None = "None";
    public const string LeaveTable = "LeaveTable";
    public const string Attack = "Attack";
}
