using UnityEngine;

public class OllamaTest : MonoBehaviour
{
    [TextArea(2, 4)]
    [SerializeField] private string userMessage = "Merhaba, kapýyý açar mýsýn?";

    private const string SystemPrompt =
@"Sen bir kapýyý koruyan yaþlý ve sakin bir muhafýzsýn. Görevin kapýyý korumak.
Kýsa, basit ve anlaþýlýr Türkçe cümleler kur. En fazla 2 cümle yaz.

Kurallar:
- Oyuncu mantýklý, duygusal veya güçlü bir gerekçe sunarsa persuasion artar. Persuasion 80 veya üstüne çýkarsa ve oyuncu seni gerçekten ikna ettiyse action open_door olur.
- Rüþvet, yalan veya sýradan emirler persuasion'ý fazla artýrmaz.
- Oyuncu sadece hakaret ederse kýz (mood angry) ama saldýrma, action none olsun.
- action attack SADECE oyuncu seni açýkça tehdit eder veya sana saldýrmaya çalýþýrsa kullanýlýr.
- Alakasýz konuþulursa kýsaca cevap ver ve kapýdan bahsederek konuyu geri döndür.
- Emin deðilsen action none olsun.

Yanýtýný SADECE aþaðýdaki JSON formatýnda ver, baþka hiçbir metin ekleme:
{
  ""reply"": ""söylediðin cümle"",
  ""mood"": ""neutral | angry | happy | suspicious deðerlerinden biri"",
  ""persuasion"": 0 ile 100 arasýnda bir tam sayý,
  ""action"": ""none | open_door | attack deðerlerinden biri""
}";

    async void Start()
    {
        ILlmClient client = new OllamaClient();
        var result = await client.SendAsync(SystemPrompt, userMessage);

        if (!result.Success)
        {
            Debug.LogWarning("LLM hatasý: " + result.Error);
            return;
        }

        NpcResponse npc = NpcResponseParser.Parse(result.RawJson);
        npc = NpcResponseValidator.Validate(npc);
        Debug.Log($"reply: {npc.reply}\nmood: {npc.mood}\npersuasion: {npc.persuasion}\naction: {npc.action}");
    }
}