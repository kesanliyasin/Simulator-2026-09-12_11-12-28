using UnityEngine;
using UnityEngine.AI;

public class ShopNpcController : MonoBehaviour
{
    private enum NpcState
    {
        GoingToShop,
        WaitingAtShop,
        Leaving,
        WaitingForDay,
        Idle
    }

    [Header("Hedefler")]
    public Transform shopPoint;
    public Transform exitPoint;

    [Header("Davranis")]
    public float waitDuration = 5f;
    public bool repeatVisit = true;
    public bool onlyVisitDuringDay = true;
    public DayNightCycle dayNightCycle;

    private NavMeshAgent agent;
    private NpcState state;
    private float waitTimer;
    private bool hasWarnedAboutSetup;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogWarning("ShopNpcController: NPC'de NavMeshAgent componenti bulunamadi.");
            enabled = false;
            return;
        }

        if (dayNightCycle == null)
        {
            dayNightCycle = FindFirstObjectByType<DayNightCycle>();
        }

        if (onlyVisitDuringDay && dayNightCycle == null)
        {
            WarnOnce("ShopNpcController: DayNightCycle atanmamis, NPC zaman kontrolu yapamiyor.");
        }

        if (IsNightTime())
        {
            state = NpcState.WaitingForDay;
        }
        else
        {
            GoToShop();
        }
    }

    private void Update()
    {
        if (agent == null || !agent.isOnNavMesh) return;

        if (IsNightTime() && (state == NpcState.GoingToShop || state == NpcState.WaitingAtShop))
        {
            if (exitPoint != null)
            {
                GoToExit();
            }
            else
            {
                SetWaitingForDay();
            }

            return;
        }

        if (!IsNightTime() && state == NpcState.WaitingForDay)
        {
            GoToShop();
        }

        switch (state)
        {
            case NpcState.GoingToShop:
                if (HasReachedDestination())
                {
                    state = NpcState.WaitingAtShop;
                    waitTimer = waitDuration;
                    agent.isStopped = true;
                }
                break;

            case NpcState.WaitingAtShop:
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    if (exitPoint != null)
                    {
                        GoToExit();
                    }
                    else
                    {
                        state = NpcState.Idle;
                    }
                }
                break;

            case NpcState.Leaving:
                if (HasReachedDestination())
                {
                    if (repeatVisit)
                    {
                        if (IsNightTime())
                        {
                            SetWaitingForDay();
                        }
                        else
                        {
                            GoToShop();
                        }
                    }
                    else
                    {
                        state = NpcState.Idle;
                        agent.isStopped = true;
                    }
                }
                break;
        }
    }

    private bool IsNightTime()
    {
        return onlyVisitDuringDay && dayNightCycle != null && dayNightCycle.IsNight;
    }

    private void SetWaitingForDay()
    {
        state = NpcState.WaitingForDay;
        agent.isStopped = true;
    }

    private void GoToShop()
    {
        if (!TrySetDestination(shopPoint)) return;

        state = NpcState.GoingToShop;
        agent.isStopped = false;
    }

    private void GoToExit()
    {
        if (!TrySetDestination(exitPoint)) return;

        state = NpcState.Leaving;
        agent.isStopped = false;
    }

    private bool TrySetDestination(Transform target)
    {
        if (target == null)
        {
            WarnOnce("ShopNpcController: Hedef noktasi atanmamis.");
            return false;
        }

        if (!agent.isOnNavMesh)
        {
            WarnOnce("ShopNpcController: NPC NavMesh uzerinde degil.");
            return false;
        }

        if (!agent.SetDestination(target.position))
        {
            WarnOnce("ShopNpcController: Hedefe NavMesh yolu bulunamadi.");
            return false;
        }

        return true;
    }

    private bool HasReachedDestination()
    {
        if (agent.pathPending) return false;
        if (agent.pathStatus == NavMeshPathStatus.PathInvalid) return false;

        return agent.remainingDistance <= agent.stoppingDistance;
    }

    private void WarnOnce(string message)
    {
        if (hasWarnedAboutSetup) return;

        hasWarnedAboutSetup = true;
        Debug.LogWarning(message);
    }
}
