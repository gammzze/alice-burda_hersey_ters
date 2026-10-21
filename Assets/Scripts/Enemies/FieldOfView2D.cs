using UnityEngine;

public class FieldOfView2D : MonoBehaviour, IVisionSensor
{
    [Header("Görüþ Ayarlarý")]
    [SerializeField] private float viewRadius = 5f;
    [SerializeField, Range(0f, 360f)] private float viewAngle = 90f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    public bool TrySeeTarget(Vector2 facing, out Transform target)
    {
        target = null;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, viewRadius, targetMask);
        if (hit == null) return false;

        Vector2 origin = transform.position;
        Vector2 toTarget = (Vector2)hit.bounds.center - origin;
        float distance = toTarget.magnitude;

        if (distance > 0.01f && Vector2.Angle(facing, toTarget) > viewAngle * 0.5f)
            return false;

        if (Physics2D.Raycast(origin, toTarget.normalized, distance, obstacleMask))
            return false;

        target = hit.transform;
        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
#endif
}