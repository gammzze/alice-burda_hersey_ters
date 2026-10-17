using UnityEngine;

public class FieldOfView2D : MonoBehaviour
{
    [Header("Görüþ Ayarlarý")]
    public float viewRadius = 5f;
    [Range(0, 360)]
    public float viewAngle = 90f;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public bool canSeeTarget = false;
    private Transform detectedTarget;

    public Transform DetectedTarget => detectedTarget;

    public bool CheckVision(Transform searchTarget)
    {
        canSeeTarget = false;
        detectedTarget = null;

        if (searchTarget == null) return false;

        Vector2 directionToTarget = (searchTarget.position - transform.position);
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget <= viewRadius)
        {
            Vector2 forward = transform.up;
            float angleBetweenTarget = Vector2.Angle(forward, directionToTarget);

            if (angleBetweenTarget <= viewAngle / 2f)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget.normalized, distanceToTarget, obstacleMask);

                if (hit.collider == null)
                {
                    canSeeTarget = true;
                    detectedTarget = searchTarget;
                    return true;
                }
            }
        }

        return false;
    }
}