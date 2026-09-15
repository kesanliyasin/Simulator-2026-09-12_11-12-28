using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, int> items = new Dictionary<string, int>();

    private string GetItemKey(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("Inventory: null item eklenemez.");
            return string.Empty;
        }

        if (!string.IsNullOrWhiteSpace(item.itemId))
            return item.itemId;

        if (!string.IsNullOrWhiteSpace(item.name))
            return item.name;

        return item.itemName;
    }

    public void AddItem(ItemData item, int amount = 1)
    {
        if (item == null)
        {
            Debug.LogWarning("Inventory: null item eklenemez.");
            return;
        }

        if (amount <= 0)
        {
            Debug.LogWarning("Inventory: miktar 0 veya daha küçük olamaz.");
            return;
        }

        string key = GetItemKey(item);

        if (items.ContainsKey(key))
            items[key] += amount;
        else
            items[key] = amount;

        Debug.Log(item.itemName + " eklendi. Toplam: " + items[key]);
    }

    public bool RemoveItem(ItemData item, int amount = 1)
    {
        if (item == null)
        {
            Debug.LogWarning("Inventory: null item kaldırılamaz.");
            return false;
        }

        if (amount <= 0)
        {
            Debug.LogWarning("Inventory: miktar 0 veya daha küçük olamaz.");
            return false;
        }

        string key = GetItemKey(item);

        if (!items.ContainsKey(key) || items[key] < amount) return false;
        items[key] -= amount;
        if (items[key] <= 0) items.Remove(key);
        return true;
    }
}