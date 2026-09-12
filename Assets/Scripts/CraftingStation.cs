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
        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            if (!inv.items.ContainsKey(recipe.requiredItems[i]) ||
                inv.items[recipe.requiredItems[i]] < recipe.requiredAmounts[i])
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