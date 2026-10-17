using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(FieldOfView2D))]
public class SoldierAI : MonoBehaviour
{
    public enum SoldierState { Patrol, Chase, Search }
    [Header("Durum")]
    public SoldierState currentState = SoldierState.Patrol;

    [Header("Devriye Noktalarý")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("Ayarlar")]
    public float chaseSpeed = 3.5f;
    public float patrolSpeed = 2f;
    public float searchDuration = 3f;
    private float searchTimer = 0f;

    private NavMeshAgent navAgent;
    private FieldOfView2D fov;
    private Transform playerTransform;

    private Vector3 startingPosition;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView2D>();

        // NavMeshAgent 2D ayarlarý (rotasyon kilitleri)
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;

        startingPosition = transform.position;

        // Sahnedeki oyuncuyu bul (PlayerController2D etiketine veya tipine göre)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        GotoNextPatrolPoint();
    }

    void Update()
    {
        // 1. Görüþ alanýný sürekli kontrol et
        bool seesPlayer = fov.CheckVision(playerTransform);

        switch (currentState)
        {
            case SoldierState.Patrol:
                PatrolTick(seesPlayer);
                break;
            case SoldierState.Chase:
                ChaseTick(seesPlayer);
                break;
            case SoldierState.Search:
                SearchTick(seesPlayer);
                break;
        }
    }

    void PatrolTick(bool seesPlayer)
    {
        navAgent.speed = patrolSpeed;

        if (seesPlayer)
        {
            Debug.Log("[SoldierAI] Alice görüldü! Kovalama baþlýyor.");
            currentState = SoldierState.Chase;
            return;
        }

        // Devriye noktasýna varýldý mý kontrolü
        if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
        {
            GotoNextPatrolPoint();
        }
    }

    void ChaseTick(bool seesPlayer)
    {
        navAgent.speed = chaseSpeed;

        if (seesPlayer)
        {
            // Oyuncuyu takip etmeye devam et
            navAgent.SetDestination(playerTransform.position);

            // Yakalanma kontrolü (Örn: Mesafe 1 birimden küçükse yakaladý demektir)
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= 1.2f)
            {
                TriggerCheckpointRespawn();
            }
        }
        else
        {
            // Gözden kaybetti, doðrudan arama durumuna geç
            Debug.Log("[SoldierAI] Hedef kaybedildi, aranýyor...");
            currentState = SoldierState.Search;
            searchTimer = searchDuration;
        }
    }

    void SearchTick(bool seesPlayer)
    {
        if (seesPlayer)
        {
            currentState = SoldierState.Chase;
            return;
        }

        searchTimer -= Time.deltaTime;
        if (searchTimer <= 0f)
        {
            // Aramadan vazgeç, tekrar devriyeye dön
            currentState = SoldierState.Patrol;
            GotoNextPatrolPoint();
        }
    }

    void GotoNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        navAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void TriggerCheckpointRespawn()
    {
        Debug.LogWarning("[SoldierAI] Alice yakalandý! Checkpoint noktasýna dönülüyor.");

        // Oyuncuyu son checkpoint pozisyonuna ýþýnla veya GameStateManager üzerinden yönet
        // Örnek basit sýfýrlama:
        if (playerTransform != null)
        {
            // Eðer varsa Checkpoint Manager çaðrýlabilir, þimdilik baþlangýç noktasýna veya checkpoint'e atýyoruz:
            playerTransform.position = startingPosition;
        }

        currentState = SoldierState.Patrol;
        GotoNextPatrolPoint();
    }
}