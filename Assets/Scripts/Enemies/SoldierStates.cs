using UnityEngine;

public class PatrolState : ISoldierState
{
    private readonly SoldierAI ai;
    private int index;
    private float waitTimer;

    public PatrolState(SoldierAI ai) => this.ai = ai;

    public void Enter()
    {
        ai.Agent.isStopped = false;
        ai.Agent.speed = ai.PatrolSpeed;
        waitTimer = 0f;
        GoToCurrentPoint();
    }

    public void Tick()
    {
        if (!ai.IsIgnoring && ai.Sensor.TrySeeTarget(ai.Facing, out _))
        {
            ai.ChangeState(ai.Chase);
            return;
        }

        if (ai.PatrolPoints.Length == 0 || !ai.HasReachedDestination()) return;

        waitTimer += Time.deltaTime;
        if (waitTimer < ai.PatrolWaitTime) return;

        waitTimer = 0f;
        index = (index + 1) % ai.PatrolPoints.Length;
        GoToCurrentPoint();
    }

    public void Exit() { }

    private void GoToCurrentPoint()
    {
        if (ai.PatrolPoints.Length == 0) return;
        ai.Agent.SetDestination(ai.PatrolPoints[index].position);
    }
}

public class ChaseState : ISoldierState
{
    private readonly SoldierAI ai;
    private float lostTimer;

    public ChaseState(SoldierAI ai) => this.ai = ai;

    public void Enter()
    {
        ai.Agent.isStopped = false;
        ai.Agent.speed = ai.ChaseSpeed;
        lostTimer = 0f;
    }

    public void Tick()
    {
        if (ai.Sensor.TrySeeTarget(ai.Facing, out Transform target))
        {
            lostTimer = 0f;
            ai.LastKnownPosition = target.position;
            ai.Agent.SetDestination(target.position);

            float distance = Vector2.Distance(ai.transform.position, target.position);
            if (distance <= ai.CatchDistance)
                ai.NotifyCaught();
            return;
        }

        lostTimer += Time.deltaTime;
        if (lostTimer >= ai.LoseSightTime)
            ai.ChangeState(ai.Search);
    }

    public void Exit() { }
}

public class SearchState : ISoldierState
{
    private readonly SoldierAI ai;
    private float timer;

    public SearchState(SoldierAI ai) => this.ai = ai;

    public void Enter()
    {
        timer = 0f;
        ai.Agent.isStopped = false;
        ai.Agent.speed = ai.PatrolSpeed;
        ai.Agent.SetDestination(ai.LastKnownPosition);
    }

    public void Tick()
    {
        if (!ai.IsIgnoring && ai.Sensor.TrySeeTarget(ai.Facing, out _))
        {
            ai.ChangeState(ai.Chase);
            return;
        }

        if (!ai.HasReachedDestination()) return;

        timer += Time.deltaTime;
        if (timer >= ai.SearchDuration)
            ai.ChangeState(ai.Patrol);
    }

    public void Exit() { }
}