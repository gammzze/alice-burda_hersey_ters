using System.Linq;
using UnityEngine;

public class GardenGateController : MonoBehaviour
{
    [Header("Kapi Bileþenleri")]
    [SerializeField] private GameObject gateBlocker; // Kapýyý kapatan fiziksel engel
    [SerializeField] private GameObject exitZone;    // Kapý açýlýnca aktif olan çýkýþ alaný (Trigger)

    private Rose[] rosesInGarden;
    private bool gateOpened;

    private void Start()
    {
        // Sahnede bulunan tüm Rose bileþenlerini bul
        RefreshRoseList();

        // Baþlangýçta çýkýþ alanýný kapalý tut
        if (exitZone != null)
        {
            exitZone.SetActive(false);
        }
        else
        {
            Debug.LogError("[GardenGate] exitZone atanmadý! Inspector üzerinden atama yapýn.");
        }
    }

    private void Update()
    {
        if (gateOpened) return;

        // Liste boþ veya sýfýrsa tekrar sahneden çekmeyi dene
        if (rosesInGarden == null || rosesInGarden.Length == 0)
        {
            RefreshRoseList();
            if (rosesInGarden == null || rosesInGarden.Length == 0) return;
        }

        // Sahnede boyanmamýþ gül kalmadýysa kapýyý aç
        if (rosesInGarden.All(rose => rose != null && rose.IsPainted))
        {
            OpenGate();
        }
    }

    /// <summary>
    /// Kapýyý açar, engeli kaldýrýr ve çýkýþ trigger'ýný aktif eder.
    /// Dýþarýdaki Search/Rose kontrolcülerinden de çaðrýlabilir.
    /// </summary>
    public void OpenGate()
    {
        if (gateOpened) return;

        gateOpened = true;
        Debug.Log("[Gate] Tüm güller boyandý, kapý açýldý. Alice çýkýþa yürümeli.");

        // Kapý engelini kaldýr
        if (gateBlocker != null)
        {
            gateBlocker.SetActive(false);
        }

        // Çýkýþ alanýný aktif et
        if (exitZone != null)
        {
            exitZone.SetActive(true);
        }
    }

    public void RefreshRoseList()
    {
        rosesInGarden = FindObjectsByType<Rose>(FindObjectsSortMode.None);
        if (rosesInGarden.Length == 0)
        {
            Debug.LogWarning("[GardenGate] Sahnede gül bulunamadý!");
        }
    }
}