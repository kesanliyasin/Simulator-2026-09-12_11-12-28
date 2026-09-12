using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();

    public void AddItem(ItemData item, int amount = 1)
    {
        if (items.ContainsKey(item))
            items[item] += amount;
        else
            items[item] = amount;

        Debug.Log(item.itemName + " eklendi. Toplam: " + items[item]);
    }

    public bool RemoveItem(ItemData item, int amount = 1)
    {
        if (!items.ContainsKey(item) || items[item] < amount) return false;
        items[item] -= amount;
        if (items[item] <= 0) items.Remove(item);
        return true;
    }
}