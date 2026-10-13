using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Fýrça Bot'un durum makinesi. Aktif durumu tutar ve her karede Tick() çaðýrýr.
/// Durum geçiþleri IAgentBehavior arayüzü üzerinden yapýlýr.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class BrushBotAgent : MonoBehaviour, IAgentBehavior
{
    public IAgentState CurrentState { get; private set; }

    public NavMeshAgent NavAgent { get; private set; }

    void Awake()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        NavAgent.updateRotation = false; // 2D için þart
        NavAgent.updateUpAxis = false;   // 2D için þart
    }

    void Update()
    {
        CurrentState?.Tick();
    }

    public void ChangeState(IAgentState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
}