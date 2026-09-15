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
        if (player == null || station == null || playerInventory == null) return;
        if (station.recipes == null || station.recipes.Length == 0) return;

        Vector3 offset = transform.position - player.position;
        offset.y = 0f;
        float distance = offset.magnitude;
        if (distance <= interactionRange && Input.GetKeyDown(KeyCode.C))
        {
            station.Craft(station.recipes[0], playerInventory);
        }
    }
}