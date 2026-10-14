using UnityEngine;
using UnityEngine.AI;

/// <summary>INavigator'ýn NavMeshAgent ile gerçek uygulamasý.</summary>
public class NavMeshAgentNavigator : INavigator
{
    private readonly NavMeshAgent agent;

    public NavMeshAgentNavigator(NavMeshAgent agent)
    {
        this.agent = agent;
    }

    public void MoveTo(Vector2 target)
    {
        agent.isStopped = false;
        agent.SetDestination(new Vector3(target.x, target.y, 0f));
    }

    public bool HasArrived =>
        !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.05f;

    public void Stop()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }
}