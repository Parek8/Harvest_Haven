using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe")]
internal class CraftingRecipe : ScriptableObject
{
    [field: Tooltip("Items are ordered like numpad: 9 8 7 | 6 5 4 | 3 2 1")]
    [field: SerializeField] Item[] Items = new Item[9];
    [field: SerializeField] Item ResultItem;

    internal IReadOnlyCollection<Item> GetItems => Items;
    internal Item GetResultItem => ResultItem;

    internal bool CompareRecipes(Item[] _compared)
    {

        if (_compared[0] != Items[0])
            return false;
        else if (_compared[1] != Items[1])
            return false;
        else if (_compared[2] != Items[2])
            return false;
        else if (_compared[3] != Items[3])
            return false;
        else if (_compared[4] != Items[4])
            return false;
        else if (_compared[5] != Items[5])
            return false;
        else if (_compared[6] != Items[6])
            return false;
        else if (_compared[7] != Items[7])
            return false;
        else if (_compared[8] != Items[8])
            return false;
        else
            return true;
    }
}