using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour, IGameFlow
{
    public static IGameFlow Instance { get; private set; }

    [Header("Debug & Test Ayarlarý")]
    [Tooltip("Sahneyi editörde tek baþýna test ederken hangi aþamada baþlasýn?")]
    [SerializeField] private GameStage startStage = GameStage.Intro;

    public GameStage Current { get; private set; }
    public event Action<GameStage> OnStageChanged;

    // Build Settings listesiyle birebir ayný sýralama
    private static readonly string[] SceneNames =
    {
        "01_Fall",       // GameStage.Intro (0)
        "02_Story",      // GameStage.Story (1)
        "03_RoseGarden", // GameStage.Garden (2)
        "04_Escape",     // GameStage.Escape (3)
        "05_TeaParty",   // GameStage.TeaParty (4)
        "06_Ending"      // GameStage.Ending (5)
    };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        Instance = null;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Current = startStage;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if ((object)Instance != this) return;

        if (scene.name == "00_MainMenu")
        {
            Current = GameStage.Intro;
        }
        else
        {
            for (int i = 0; i < SceneNames.Length; i++)
            {
                if (SceneNames[i] == scene.name)
                {
                    Current = (GameStage)i;
                    break;
                }
            }
        }
    }

    public void CompleteStage(GameStage stage)
    {
        Debug.Log($"[Flow] CompleteStage({stage}) çaðrýldý, Aktif Aþama: {Current}");

        if (Current == GameStage.Ending) return;

        // Doðrudan hedef sahne adýný güvenli þekilde yükle
        if (stage == GameStage.Garden)
        {
            GoToStage(GameStage.Escape, "04_Escape");
        }
        else
        {
            int nextIndex = (int)stage + 1;
            if (nextIndex < SceneNames.Length)
            {
                GoToStage((GameStage)nextIndex, SceneNames[nextIndex]);
            }
        }
    }

    public void EndGame()
    {
        if (Current != GameStage.Ending)
        {
            GoToStage(GameStage.Ending, "06_Ending");
        }
    }

    private void GoToStage(GameStage nextStage, string sceneName)
    {
        Current = nextStage;
        OnStageChanged?.Invoke(nextStage);

        Debug.Log($"[Flow] Sahne yükleniyor: {sceneName}");
        SceneManager.LoadSceneAsync(sceneName);
    }
}