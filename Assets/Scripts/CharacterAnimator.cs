using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        if (animator == null || playerMovement == null) return;

        float speed = playerMovement.CurrentMoveDirection.magnitude;
        animator.SetFloat("Speed", speed);
    }

    public void PlayGather()
    {
        if (animator != null)
        {
            animator.SetTrigger("Gather");
        }
    }
}