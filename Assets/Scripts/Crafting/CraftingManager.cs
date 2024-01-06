using System.Collections.Generic;
using UnityEngine;


internal class CraftingManager : MonoBehaviour
{
    internal static CraftingManager craftingManager;
    [field: SerializeField] List<CraftingRecipe> _recipes = new();

    private void Awake()
    {
        if (craftingManager == null)
            craftingManager = this;
    }

    internal Item FindRecipeResult(Item[] _recipe)
    {
        for (int i = 0; i < _recipes.Count; i++)
        {
            if (_recipes[i].CompareRecipes(_recipe))
                return _recipes[i].GetResultItem;
        }

        return null;
    }
}