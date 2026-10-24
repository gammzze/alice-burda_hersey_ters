using System;
using UnityEngine;

public enum StorySpeaker { Alice, Rabbit }

// Hikâyenin tek bir satırı: kim söylüyor, ne söylüyor.
[Serializable]
public class StoryLine
{
    public StorySpeaker speaker;
    [TextArea(2, 4)] public string text;

    public StoryLine(StorySpeaker speaker, string text)
    {
        this.speaker = speaker;
        this.text = text;
    }
}
