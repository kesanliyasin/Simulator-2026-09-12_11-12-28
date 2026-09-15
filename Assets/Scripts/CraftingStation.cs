using UnityEngine;

[System.Serializable]
public class Recipe
{
    public string recipeName;
    public ItemData[] requiredItems;
    public int[] requiredAmounts;
    public ItemData resultItem;
    public int resultAmount = 1;
}

public class CraftingStation : MonoBehaviour
{
    public Recipe[] recipes;

    public bool CanCraft(Recipe recipe, Inventory inv)
    {
        if (recipe == null || inv == null) return false;
        if (recipe.requiredItems == null || recipe.requiredAmounts == null) return false;
        if (recipe.requiredItems.Length != recipe.requiredAmounts.Length) return false;

        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            var requiredItem = recipe.requiredItems[i];
            if (requiredItem == null) return false;

            string key = string.IsNullOrWhiteSpace(requiredItem.itemId)
                ? requiredItem.name
                : requiredItem.itemId;

            if (!inv.items.ContainsKey(key) || inv.items[key] < recipe.requiredAmounts[i])
            {
                return false;
            }
        }
        return true;
    }

    public bool Craft(Recipe recipe, Inventory inv)
    {
        if (!CanCraft(recipe, inv)) return false;

        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            inv.RemoveItem(recipe.requiredItems[i], recipe.requiredAmounts[i]);
        }

        inv.AddItem(recipe.resultItem, recipe.resultAmount);
        Debug.Log(recipe.recipeName + " üretildi!");
        return true;
    }
}