using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour, IGameFlow
{
    public static IGameFlow Instance { get; private set; }

    [Header("Debug: sahneyi tek baþýna test etmek için")]
    [SerializeField] private GameStage startStage = GameStage.Intro;

    public GameStage Current { get; private set; }
    public event Action<GameStage> OnStageChanged;

    private static readonly string[] SceneNames =
    {
        "01_Fall", "02_Story", "03_RoseGarden",
        "04_Escape", "05_TeaParty", "06_Ending"
    };

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        Current = startStage;
        DontDestroyOnLoad(gameObject);
    }

    public void CompleteStage(GameStage stage)
    {
        if (stage != Current || Current == GameStage.Ending) return;
        GoTo((GameStage)((int)Current + 1));
    }

    public void EndGame()
    {
        if (Current != GameStage.Ending) GoTo(GameStage.Ending);
    }

    private void GoTo(GameStage next)
    {
        Current = next;
        OnStageChanged?.Invoke(next);
        SceneManager.LoadSceneAsync(SceneNames[(int)next]);
    }
}