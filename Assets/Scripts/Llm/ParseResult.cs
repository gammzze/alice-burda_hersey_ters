// Parser'ýn sonucu: geçerli mi, geçerliyse yanýt, deðilse hata nedeni.
public class ParseResult
{
    public bool IsValid;
    public NpcResponse Response;
    public string Error;

    public static ParseResult Ok(NpcResponse response)
    {
        return new ParseResult { IsValid = true, Response = response };
    }

    public static ParseResult Fail(string error)
    {
        return new ParseResult { IsValid = false, Error = error };
    }
}