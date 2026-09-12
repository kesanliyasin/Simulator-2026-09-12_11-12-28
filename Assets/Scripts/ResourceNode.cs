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
        originalColor = rend.material.color;
    }

    public void Highlight()
    {
        rend.material.color = highlightColor;
    }

    public void Unhighlight()
    {
        rend.material.color = originalColor;
    }

    public void Gather(Inventory playerInventory)
    {
        if (!isAvailable) return;

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