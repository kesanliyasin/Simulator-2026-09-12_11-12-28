using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public float gatherAnimationDuration = 1.2f;
    public LayerMask resourceLayer;

    private Inventory inventory;
    private ResourceNode hoveredTarget;
    private Camera cam;
    private CharacterAnimator characterAnimator;
    private PlayerMovement playerMovement;
    private ResourceNode pendingTarget;
    private bool isGathering = false;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        cam = Camera.main;
        characterAnimator = GetComponent<CharacterAnimator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        CheckHover();

        if (isGathering) return;

        if (HasManualMovementInput())
        {
            CancelPendingGather();
        }
        else if (pendingTarget != null)
        {
            if (!pendingTarget.IsAvailable)
            {
                CancelPendingGather();
            }
            else if (GetHorizontalDistance(pendingTarget.transform.position) <= interactionRange)
            {
                ResourceNode target = pendingTarget;
                CancelPendingGather();
                StartCoroutine(GatherRoutine(target));
            }
        }

        if (Input.GetMouseButtonDown(0) && hoveredTarget != null)
        {
            StartGathering(hoveredTarget);
        }
    }

    void CheckHover()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        ResourceNode newHover = null;

        if (Physics.Raycast(ray, out hit, 100f, resourceLayer))
        {
            ResourceNode node = hit.collider.GetComponent<ResourceNode>();
            if (node != null && node.IsAvailable)
            {
                newHover = node;
            }
        }

        if (hoveredTarget != null && hoveredTarget != newHover)
        {
            hoveredTarget.Unhighlight();
        }

        if (newHover != null && newHover != hoveredTarget)
        {
            newHover.Highlight();
        }

        hoveredTarget = newHover;
    }

    void StartGathering(ResourceNode node)
    {
        float distance = GetHorizontalDistance(node.transform.position);

        if (distance <= interactionRange)
        {
            CancelPendingGather();
            StartCoroutine(GatherRoutine(node));
        }
        else
        {
            pendingTarget = node;
            if (playerMovement != null)
            {
                playerMovement.MoveTo(node.transform.position, interactionRange);
            }
            else
            {
                Debug.LogWarning("PlayerInteraction: PlayerMovement component bulunamadı.");
            }
        }
    }

    bool HasManualMovementInput()
    {
        return Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f;
    }

    float GetHorizontalDistance(Vector3 targetPosition)
    {
        Vector3 offset = targetPosition - transform.position;
        offset.y = 0f;
        return offset.magnitude;
    }

    void CancelPendingGather()
    {
        pendingTarget = null;
        if (playerMovement != null)
        {
            playerMovement.CancelMoveTo();
        }
    }

    IEnumerator GatherRoutine(ResourceNode node)
    {
        isGathering = true;
        if (playerMovement != null)
        {
            playerMovement.SetMovementLocked(true);
        }

        if (characterAnimator != null)
        {
            characterAnimator.PlayGather();
        }

        yield return new WaitForSeconds(gatherAnimationDuration);

        // Animasyon bittikten sonra gerçek toplama gerçekleşir
        if (node != null && node.IsAvailable)
        {
            node.Gather(inventory);
        }

        isGathering = false;
        if (playerMovement != null)
        {
            playerMovement.SetMovementLocked(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}