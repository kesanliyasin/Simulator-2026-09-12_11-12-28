using UnityEngine;

public class CraftingInteraction : MonoBehaviour
{
    public float interactionRange = 3f;
    private CraftingStation station;
    private Transform player;
    private Inventory playerInventory;

    void Start()
    {
        station = GetComponent<CraftingStation>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerInventory = playerObj.GetComponent<Inventory>();
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= interactionRange && Input.GetKeyDown(KeyCode.C))
        {
            if (station.recipes.Length > 0)
            {
                station.Craft(station.recipes[0], playerInventory);
            }
        }
    }
}