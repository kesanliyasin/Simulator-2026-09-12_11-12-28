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
    private bool isGathering = false;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        cam = Camera.main;
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    void Update()
    {
        CheckHover();

        if (Input.GetMouseButtonDown(0) && hoveredTarget != null && !isGathering)
        {
            TryGather(hoveredTarget);
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

    void TryGather(ResourceNode node)
    {
        float distance = Vector3.Distance(transform.position, node.transform.position);

        if (distance <= interactionRange)
        {
            StartCoroutine(GatherRoutine(node));
        }
        else
        {
            Debug.Log("Çok uzak, yaklaşman lazım! Mesafe: " + distance);
        }
    }

    IEnumerator GatherRoutine(ResourceNode node)
    {
        isGathering = true;

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
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}