// LLM yanýt veremediðinde gösterilen yedek cümleler.
// action her zaman None: yedek yanýt oyun durumunu deðiþtirmez.
public static class FallbackResponses
{
    private static readonly string[] Lines =
    {
        "Hmm, çay çok sýcaktý da ne dediðini duyamadým. Tekrar söyler misin?",
        "Fincaným dilimi tutuyor sandým! Bir daha dener misin?",
        "Bilmecelerim kafamý karýþtýrdý. Ne diyordun?",
        "Saat yine yanlýþ çalýyor, söylediðini kaçýrdým. Yine söyle bakalým."
    };

    private static int _index;

    public static NpcResponse Next()
    {
        string line = Lines[_index % Lines.Length];
        _index++;
        return new NpcResponse
        {
            reply = line,
            mood = NpcMoods.Neutral,
            persuasion = 0f,
            action = NpcActions.None
        };
    }
}
