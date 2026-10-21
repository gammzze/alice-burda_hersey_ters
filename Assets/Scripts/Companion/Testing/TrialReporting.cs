using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

public interface ITrialResultWriter
{
    void Write(string path, IReadOnlyList<TrialResult> results);
}

public class CsvTrialResultWriter : ITrialResultWriter
{
    public void Write(string path, IReadOnlyList<TrialResult> results)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        var ci = CultureInfo.InvariantCulture;
        var sb = new StringBuilder();
        sb.AppendLine("trial,startLabel,startX,startY,roseCount,paintedCount,success,duration,stuckCount");
        foreach (var r in results)
        {
            sb.AppendLine(string.Join(",",
                r.Trial, r.StartLabel,
                r.StartX.ToString("F2", ci), r.StartY.ToString("F2", ci),
                r.RoseCount, r.PaintedCount, r.Success ? 1 : 0,
                r.Duration.ToString("F2", ci), r.StuckCount));
        }
        File.WriteAllText(path, sb.ToString());
    }
}

/// <summary>Sonuçlardan README'ye yapýþtýrýlabilir Markdown tablosu üretir.</summary>
public static class TrialSummary
{
    public static string BuildMarkdown(IReadOnlyList<TrialResult> results)
    {
        var ci = CultureInfo.InvariantCulture;
        var sb = new StringBuilder();
        sb.AppendLine("| Baþlangýç | Deneme | Baþarý | Baþarý oraný | Ort. süre (sn, baþarýlý) | Toplam takýlma |");
        sb.AppendLine("|---|---|---|---|---|---|");

        foreach (var g in results.GroupBy(r => r.StartLabel))
            sb.AppendLine(Row(g.Key, g.ToList(), ci));

        sb.AppendLine(Row("**Toplam**", results.ToList(), ci));
        return sb.ToString();
    }

    private static string Row(string label, List<TrialResult> list, CultureInfo ci)
    {
        int ok = list.Count(r => r.Success);
        double rate = list.Count == 0 ? 0 : 100.0 * ok / list.Count;
        double avg = ok == 0 ? 0 : list.Where(r => r.Success).Average(r => r.Duration);
        int stuck = list.Sum(r => r.StuckCount);
        return $"| {label} | {list.Count} | {ok} | %{rate.ToString("F1", ci)} | {avg.ToString("F1", ci)} | {stuck} |";
    }
}