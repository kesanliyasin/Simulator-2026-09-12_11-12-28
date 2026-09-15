using UnityEngine;
using UnityEngine.AI;

public class NpcAnimator : MonoBehaviour
{
    public string speedParameter = "Speed";

    private Animator animator;
    private NavMeshAgent agent;
    private bool hasWarned;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (animator == null)
        {
            WarnOnce("NpcAnimator: NPC veya child objelerinde Animator bulunamadi.");
        }

        if (agent == null)
        {
            WarnOnce("NpcAnimator: NPC'de NavMeshAgent bulunamadi.");
        }
    }

    private void Update()
    {
        if (animator == null || agent == null) return;

        float speed = 0f;
        if (agent.speed > 0f)
        {
            speed = Mathf.Clamp01(agent.velocity.magnitude / agent.speed);
        }

        animator.SetFloat(speedParameter, speed);
    }

    private void WarnOnce(string message)
    {
        if (hasWarned) return;

        hasWarned = true;
        Debug.LogWarning(message);
    }
}
