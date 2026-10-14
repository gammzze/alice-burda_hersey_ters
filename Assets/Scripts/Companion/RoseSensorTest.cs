using UnityEngine;

/// <summary>Geçici test. Sensörün bulduðu gülü her saniye Console'a yazar.</summary>
[RequireComponent(typeof(RaycastRoseSensor))]
public class RoseSensorTest : MonoBehaviour
{
    private IRoseSensor sensor;
    private float timer;

    void Awake() { sensor = GetComponent<IRoseSensor>(); }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 1f) return;
        timer = 0f;

        IPaintable rose = sensor.FindNearestUnpaintedRose();
        if (rose == null) Debug.Log("[Sensor] Gül yok");
        else Debug.Log($"[Sensor] En yakýn gül: {((Component)rose).name}");
    }
}