using System;

[Serializable]
public class NpcResponse
{
    public string reply;      // NPC'nin oyuncuya söylediði cümle
    public string mood;       // neutral | angry | happy | suspicious
    public int persuasion;    // 0-100: oyuncunun NPC'yi ikna seviyesi
    public string action;     // none | open_door | attack
}

// Geçerli deðerleri tek yerde tutuyoruz, baþka sýnýflar buradan okur.
public static class NpcMoods
{
    public const string Neutral = "neutral";
    public const string Angry = "angry";
    public const string Happy = "happy";
    public const string Suspicious = "suspicious";
}

public static class NpcActions
{
    public const string None = "none";
    public const string OpenDoor = "open_door";
    public const string Attack = "attack";
}
