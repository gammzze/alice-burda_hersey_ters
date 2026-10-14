using UnityEngine;

/// <summary>
/// Gülleri OverlapCircle (yakýn çevre) ve 8 yönlü Raycast ile algýlar.
/// Yalnýzca algýlar; hareket ve boyama bu sýnýfýn iþi deðildir.
/// </summary>
public class RaycastRoseSensor : MonoBehaviour, IRoseSensor
{
    [SerializeField] private float detectionRadius = 6f;
    [SerializeField] private float rayRange = 12f;
    [SerializeField] private LayerMask roseMask;
    [SerializeField] private LayerMask obstacleMask;

    private const int RayCount = 8;

    public IPaintable FindNearestUnpaintedRose()
    {
        Vector2 origin = transform.position;
        IPaintable best = null;
        float bestDistance = float.MaxValue;

        // 1) Yakýn çevre: daire içindeki güller (duvar arkasýndakiler elenir)
        Collider2D[] nearby = Physics2D.OverlapCircleAll(origin, detectionRadius, roseMask);
        foreach (Collider2D col in nearby)
        {
            if (!col.TryGetComponent<IPaintable>(out var rose) || rose.IsPainted) continue;

            Vector2 toRose = rose.Position - origin;
            if (!HasLineOfSight(origin, toRose)) continue;

            Consider(rose, toRose.magnitude, ref best, ref bestDistance);
        }

        // 2) Uzak görüþ: 8 yönlü ýþýn (koridorlar için)
        for (int i = 0; i < RayCount; i++)
        {
            Vector2 direction = DirectionFromIndex(i);
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayRange, roseMask | obstacleMask);

            if (hit.collider == null) continue;
            if (!hit.collider.TryGetComponent<IPaintable>(out var rose) || rose.IsPainted) continue;

            Consider(rose, hit.distance, ref best, ref bestDistance);
        }

        return best;
    }

    private bool HasLineOfSight(Vector2 origin, Vector2 toTarget)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, toTarget.normalized, toTarget.magnitude, obstacleMask);
        return hit.collider == null;
    }

    private static void Consider(IPaintable rose, float distance, ref IPaintable best, ref float bestDistance)
    {
        if (distance >= bestDistance) return;
        best = rose;
        bestDistance = distance;
    }

    private static Vector2 DirectionFromIndex(int index)
    {
        float angle = index * (360f / RayCount) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    // Scene penceresinde sensörü görmek için (Bot seçiliyken). Rapor ekran görüntüsü için de iþe yarar.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.cyan;
        for (int i = 0; i < RayCount; i++)
            Gizmos.DrawRay(transform.position, DirectionFromIndex(i) * rayRange);
    }
}