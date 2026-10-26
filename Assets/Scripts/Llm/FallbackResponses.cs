// LLM yanýt veremediðinde ya da eylemi reddedildiðinde gösterilen cümleler.
// action her zaman None: bu yanýtlar oyun durumunu deðiþtirmez.
public static class FallbackResponses
{
    private static readonly string[] Lines =
    {
        "Hmm, çay çok sýcaktý da ne dediðini duyamadým. Tekrar söyler misin?",
        "Fincaným dilimi tutuyor sandým! Bir daha dener misin?",
        "Bilmecelerim kafamý karýþtýrdý. Ne diyordun?",
        "Saat yine yanlýþ çalýyor, söylediðini kaçýrdým. Yine söyle bakalým."
    };

    // Model "kalkýyorum" dedi ama oyun ikna yetersiz bulup reddetti.
    private static readonly string[] NotConvincedLines =
    {
        "Hmm, ilginç... ama daha masadan kalkacak kadar ikna olmadým.",
        "Kulaða hoþ geliyor, fakat çayým daha bitmedi!",
        "Belki, belki... Ama önce beni biraz daha eðlendir."
    };

    private static int _index;
    private static int _notConvincedIndex;

    public static NpcResponse Next()
    {
        string line = Lines[_index % Lines.Length];
        _index++;
        return Build(line);
    }

    public static string NotConvinced()
    {
        string line = NotConvincedLines[_notConvincedIndex % NotConvincedLines.Length];
        _notConvincedIndex++;
        return line;
    }

    private static NpcResponse Build(string line)
    {
        return new NpcResponse
        {
            reply = line,
            mood = NpcMoods.Neutral,
            persuasion = 0f,
            action = NpcActions.None
        };
    }
}