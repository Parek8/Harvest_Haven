using System.Collections.Generic;
using UnityEngine;


public class CraftingManager : MonoBehaviour
{
    public static CraftingManager craftingManager;
    [field: SerializeField] List<CraftingRecipe> _recipes = new();

    private void Awake()
    {
        if (craftingManager == null)
            craftingManager = this;
    }

    public Item FindRecipeResult(Item[] _recipe)
    {
        for (int i = 0; i < _recipes.Count; i++)
        {
            if (_recipes[i].CompareRecipes(_recipe))
                return _recipes[i].GetResultItem;
        }

        return null;
    }
}