using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(FieldOfView2D))]
public class SoldierAI : MonoBehaviour
{
    [Header("Devriye")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float patrolWaitTime = 1f;

    [Header("Kovalama")]
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float catchDistance = 0.6f;
    [SerializeField] private float loseSightTime = 0.5f;

    [Header("Arama")]
    [SerializeField] private float searchDuration = 3f;

    [Header("Respawn sonrasý")]
    [SerializeField] private float graceSeconds = 2f;

    [Header("Görsel (opsiyonel)")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    public event Action<SoldierAI> PlayerCaught;

    public NavMeshAgent Agent { get; private set; }
    public IVisionSensor Sensor { get; private set; }
    public Vector2 Facing { get; private set; } = Vector2.right;
    public Vector3 LastKnownPosition { get; set; }

    public Transform[] PatrolPoints => patrolPoints;
    public float PatrolSpeed => patrolSpeed;
    public float PatrolWaitTime => patrolWaitTime;
    public float ChaseSpeed => chaseSpeed;
    public float CatchDistance => catchDistance;
    public float LoseSightTime => loseSightTime;
    public float SearchDuration => searchDuration;
    public bool IsIgnoring => Time.time < ignoreUntil;

    public ISoldierState Patrol { get; private set; }
    public ISoldierState Chase { get; private set; }
    public ISoldierState Search { get; private set; }

    private ISoldierState current;
    private Vector3 spawnPosition;
    private bool hasCaught;
    private float ignoreUntil;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        Sensor = GetComponent<IVisionSensor>();
        spawnPosition = transform.position;

        Patrol = new PatrolState(this);
        Chase = new ChaseState(this);
        Search = new SearchState(this);

        SnapToNavMesh();
    }

    private void Start()
    {
        if (!Agent.isOnNavMesh)
        {
            Debug.LogError($"{name}: NavMesh üzerinde deðil. Askeri NavMesh alanýnýn içine yerleþtir ve Bake et.", this);
            enabled = false;
            return;
        }
        ChangeState(Patrol);
    }

    private void Update()
    {
        UpdateFacing();
        current?.Tick();
    }

    public void ChangeState(ISoldierState next)
    {
        current?.Exit();
        current = next;
        current.Enter();
    }

    public bool HasReachedDestination()
    {
        return !Agent.pathPending &&
               Agent.remainingDistance <= Agent.stoppingDistance + 0.1f;
    }

    public void NotifyCaught()
    {
        if (hasCaught || IsIgnoring) return;
        hasCaught = true;
        PlayerCaught?.Invoke(this);
    }

    public void ResetToSpawn()
    {
        hasCaught = false;
        ignoreUntil = Time.time + graceSeconds;
        Agent.Warp(spawnPosition);
        Agent.ResetPath();
        ChangeState(Patrol);
    }

    private void SnapToNavMesh()
    {
        if (Agent.isOnNavMesh) return;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            Agent.Warp(hit.position);
    }

    private void UpdateFacing()
    {
        if (Agent.velocity.sqrMagnitude < 0.01f) return;

        Facing = Agent.velocity.normalized;
        if (spriteRenderer != null)
            spriteRenderer.flipX = Facing.x < 0f;
    }
}