// NpcDialogueController'ýn sonucu.
// Yedek (fallback) ya da iptal edilmiþ sonuçta oyun durumu deðiþtirilmez.
public class DialogueOutcome
{
    public NpcResponse Response;
    public bool IsFallback;
    public bool IsCancelled;
    public string Error;

    // Eylem (LeaveTable vb.) yalnýzca gerçek ve geçerli yanýtta uygulanýr.
    public bool ShouldApplyAction => !IsFallback && !IsCancelled;

    public static DialogueOutcome Success(NpcResponse response)
    {
        return new DialogueOutcome { Response = response };
    }

    public static DialogueOutcome Fallback(string error)
    {
        return new DialogueOutcome
        {
            Response = FallbackResponses.Next(),
            IsFallback = true,
            Error = error
        };
    }

    public static DialogueOutcome Cancelled()
    {
        return new DialogueOutcome { IsCancelled = true, Error = "Ýptal edildi." };
    }
}