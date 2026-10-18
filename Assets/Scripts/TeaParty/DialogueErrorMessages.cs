// Teknik hata metnini oyuncuya gösterilecek anlaþýlýr mesaja çevirir.
public static class DialogueErrorMessages
{
    public const string ServerDown = "Þapkacý'ya ulaþýlamýyor. Ollama çalýþýyor mu? Tekrar dene.";
    public const string Timeout = "Þapkacý çok geç yanýt verdi. Tekrar dene.";
    public const string BadResponse = "Þapkacý'nýn yanýtý anlaþýlamadý. Tekrar dene.";
    public const string Generic = "Bir sorun oluþtu. Tekrar dene.";

    public static string ForError(string error)
    {
        if (string.IsNullOrEmpty(error)) return Generic;

        string e = error.ToLowerInvariant();

        if (e.Contains("cannot connect") || e.Contains("cannot resolve") || e.Contains("connection"))
            return ServerDown;

        if (e.Contains("timeout"))
            return Timeout;

        if (e.Contains("json") || e.Contains("eksik alan") || e.Contains("boþ yanýt") ||
            e.Contains("zarf") || e.Contains("reply"))
            return BadResponse;

        return Generic;
    }
}
