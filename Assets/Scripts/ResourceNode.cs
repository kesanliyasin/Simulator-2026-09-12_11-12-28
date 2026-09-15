using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public ItemData resourceItem;
    public int amountGiven = 1;
    public float respawnTime = 15f;

    [Header("Highlight Ayarları")]
    public Color highlightColor = Color.yellow;

    private bool isAvailable = true;
    private Renderer rend;
    private Color originalColor;

    public bool IsAvailable => isAvailable;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
        }
    }

    public void Highlight()
    {
        if (rend != null) rend.material.color = highlightColor;
    }

    public void Unhighlight()
    {
        if (rend != null) rend.material.color = originalColor;
    }

    public void Gather(Inventory playerInventory)
    {
        if (!isAvailable) return;
        if (playerInventory == null || resourceItem == null)
        {
            Debug.LogWarning("ResourceNode: Inventory veya resourceItem eksik.");
            return;
        }

        playerInventory.AddItem(resourceItem, amountGiven);
        isAvailable = false;
        gameObject.SetActive(false);
        Invoke(nameof(Respawn), respawnTime);
    }

    void Respawn()
    {
        isAvailable = true;
        gameObject.SetActive(true);
    }
}