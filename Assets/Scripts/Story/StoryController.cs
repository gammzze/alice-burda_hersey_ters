using UnityEngine;

// 02_Story: Alice ile Beyaz Tavþan'ýn sabit metin konuþmasý. Bu bir LLM NPC deðildir.
// Son satýrdan sonra Story aþamasýný tamamlar.
public class StoryController : MonoBehaviour
{
    [SerializeField] private StoryDialogueUI ui;

    [SerializeField]
    private StoryLine[] lines =
    {
        new StoryLine(StorySpeaker.Rabbit, "Ah, sonunda geldin! Geç kaldým, geç kaldým... ama seni bulmayý baþardým."),
        new StoryLine(StorySpeaker.Alice, "Neredeyim ben? Az önce uzun bir deliðe düþtüm. Sen de kimsin?"),
        new StoryLine(StorySpeaker.Rabbit, "Ben Beyaz Tavþan. Seni buraya ben çaðýrdým, Alice. Harikalar Diyarý'nda bir sorun var."),
        new StoryLine(StorySpeaker.Alice, "Ne sorunu? Beni neden çaðýrdýn?"),
        new StoryLine(StorySpeaker.Rabbit, "Zaman durdu! Çýlgýn Þapkacý çay masasýndan kalkmýyor, eve dönüþ deliði de tam onun masasýnýn altýnda."),
        new StoryLine(StorySpeaker.Alice, "O zaman Þapkacý'ya gidip kalkmasýný söylerim."),
        new StoryLine(StorySpeaker.Rabbit, "O kadar kolay deðil. Önce Kraliçe'nin gül bahçesinden geçmelisin, kapý dikenli güller boyanmadan açýlmaz. Sonra askerlerden kaçmalýsýn."),
        new StoryLine(StorySpeaker.Rabbit, "Fýrça Bot sana yardým edecek. Ona emir ver, o gülleri bulup boyasýn!"),
        new StoryLine(StorySpeaker.Alice, "Peki, deneyeceðim!")
    };

    private int _index;
    private IGameFlow _flow;

    private void Start()
    {
        _flow = GameFlowResolver.Resolve();
        ui.ContinueClicked += OnContinue;
        ShowCurrent();
    }

    private void OnDestroy()
    {
        if (ui != null) ui.ContinueClicked -= OnContinue;
    }

    private void ShowCurrent()
    {
        StoryLine line = lines[_index];
        ui.ShowLine(line.speaker, line.text);

        bool last = _index == lines.Length - 1;
        ui.SetButtonLabel(last ? "Gül Bahçesine Git" : "Devam");
    }

    private void OnContinue()
    {
        if (_index < lines.Length - 1)
        {
            _index++;
            ShowCurrent();
            return;
        }

        ui.SetInteractable(false);   // çift týklamayla iki kez tamamlanmasýn
        _flow.CompleteStage(GameStage.Story);
    }
}