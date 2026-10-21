using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 06_Ending: bitiþ ekranýnýn düðmeleri. Oyun akýþý kararý vermez, sadece sahne açar/kapatýr.
public class EndingController : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private string menuSceneName = "00_MainMenu";

    private void Awake()
    {
        menuButton.onClick.AddListener(ReturnToMenu);
        quitButton.onClick.AddListener(Quit);
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
