using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Fýrça Bot'u tekrarlanabilir koþullarda defalarca dener ve sonuçlarý CSV'ye yazar.
/// Botun kodunu deðiþtirmez: BotContext'i kendisi kurar, emri ScriptedOrderSource verir.
/// </summary>
public class AgentTestRunner : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private BrushBotAgent bot;
    [SerializeField] private GameObject rosePrefab;

    [Header("Deneme ayarlarý")]
    [SerializeField] private int trialCount = 50;
    [SerializeField] private int minRoses = 1;
    [SerializeField] private int maxRoses = 5;
    [SerializeField] private float timeoutSeconds = 60f;
    [SerializeField, Range(1f, 5f)] private float timeScale = 3f;
    [SerializeField] private int randomSeed = 12345; // ayný seed = tekrarlanabilir test

    [Header("Baþlangýç konumu")]
    [Tooltip("Doluysa denemeler bu noktalar arasýnda sýrayla döner. Boþsa rastgele.")]
    [SerializeField] private Transform[] fixedStartPoints;
    [SerializeField] private Vector2 areaCenter = Vector2.zero;
    [SerializeField] private Vector2 areaSize = new Vector2(10f, 10f);

    [Header("Yerleþim ve takýlma")]
    [SerializeField] private float minRoseSpacing = 1.2f;
    [SerializeField] private float minDistanceFromBot = 1.5f;
    [SerializeField] private float stuckWindow = 2f;
    [SerializeField] private float stuckDistance = 0.05f;

    private readonly List<TrialResult> results = new List<TrialResult>();
    private ITrialResultWriter writer;
    private ScriptedOrderSource orders;
    private BotContext context;

    private IEnumerator Start()
    {
        writer = new CsvTrialResultWriter();
        orders = new ScriptedOrderSource();
        yield return null; // NavMesh ve Awake'ler otursun

        context = new BotContext(
            bot,
            bot.GetComponent<RaycastRoseSensor>(),
            new NavMeshAgentNavigator(bot.NavAgent),
            orders);

        Random.InitState(randomSeed);
        Time.timeScale = timeScale;

        for (int i = 1; i <= trialCount; i++)
            yield return RunTrial(i);

        Time.timeScale = 1f;
        SaveAndReport();
    }

    private IEnumerator RunTrial(int index)
    {
        // 1) Baþlangýç konumu
        string label;
        Vector2 start = PickStart(index, out label);
        bot.NavAgent.Warp(new Vector3(start.x, start.y, bot.transform.position.z));
        bot.NavAgent.ResetPath();

        // 2) Güller
        int wanted = Random.Range(minRoses, maxRoses + 1);
        List<Rose> roses = SpawnRoses(wanted, start);

        // 3) Botu sýfýrla ve emir ver
        orders.ConsumeOrder();
        bot.ChangeState(new IdleState(context));
        yield return null;
        orders.IssueOrder();

        // 4) Bekle: baþarý, zaman aþýmý, takýlma ölçümü
        float startTime = Time.time;
        float lastCheck = startTime;
        Vector2 lastPos = bot.transform.position;
        int stuck = 0;
        bool success = false;

        while (true)
        {
            yield return null;

            if (roses.Count > 0 && roses.All(r => r.IsPainted)) { success = true; break; }
            if (Time.time - startTime >= timeoutSeconds) break;

            if (Time.time - lastCheck >= stuckWindow)
            {
                Vector2 pos = bot.transform.position;
                if (bot.CurrentState is ApproachState &&
                    Vector2.Distance(pos, lastPos) < stuckDistance)
                    stuck++;
                lastPos = pos;
                lastCheck = Time.time;
            }
        }

        float duration = Time.time - startTime;
        int painted = roses.Count(r => r.IsPainted);

        results.Add(new TrialResult(index, label, start.x, start.y,
                                    roses.Count, painted, success, duration, stuck));
        Debug.Log($"[Test] #{index} {label} gül={roses.Count} baþarý={success} süre={duration:F1}s takýlma={stuck}");

        // 5) Temizlik
        bot.ChangeState(new IdleState(context));
        context.Navigator.Stop();
        foreach (var r in roses) if (r != null) Destroy(r.gameObject);
        yield return null;
    }

    private Vector2 PickStart(int index, out string label)
    {
        if (fixedStartPoints != null && fixedStartPoints.Length > 0)
        {
            int k = (index - 1) % fixedStartPoints.Length;
            label = $"Sabit-{k + 1}";
            return fixedStartPoints[k].position;
        }

        label = "Rastgele";
        return RandomNavMeshPoint(null) ?? (Vector2)bot.transform.position;
    }

    private List<Rose> SpawnRoses(int count, Vector2 botPos)
    {
        var placed = new List<Rose>();
        var positions = new List<Vector2>();

        for (int i = 0; i < count; i++)
        {
            for (int attempt = 0; attempt < 40; attempt++)
            {
                Vector2? p = RandomNavMeshPoint(null);
                if (p == null) continue;
                if (Vector2.Distance(p.Value, botPos) < minDistanceFromBot) continue;
                if (positions.Any(o => Vector2.Distance(o, p.Value) < minRoseSpacing)) continue;

                var go = Instantiate(rosePrefab, p.Value, Quaternion.identity);
                placed.Add(go.GetComponent<Rose>());
                positions.Add(p.Value);
                break;
            }
        }
        return placed;
    }

    private Vector2? RandomNavMeshPoint(object unused)
    {
        for (int i = 0; i < 30; i++)
        {
            var candidate = new Vector3(
                areaCenter.x + Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                areaCenter.y + Random.Range(-areaSize.y / 2f, areaSize.y / 2f), 0f);
            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 0.5f, NavMesh.AllAreas))
                return new Vector2(hit.position.x, hit.position.y);
        }
        return null;
    }

    private void SaveAndReport()
    {
        string dir = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "docs", "results"));
        string file = Path.Combine(dir, $"agent_test_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv");
        writer.Write(file, results);

        Debug.Log($"[Test] BÝTTÝ. CSV: {file}\n{TrialSummary.BuildMarkdown(results)}");
    }
}