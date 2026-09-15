using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Kimlik")]
    public string itemId;

    [Header("Bilgi")]
    public string itemName;
    public Sprite icon;
    public int sellPrice;

    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(itemId))
        {
            itemId = name;
        }
    }
}