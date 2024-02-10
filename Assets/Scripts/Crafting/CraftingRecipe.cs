using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Recipe")]
internal class CraftingRecipe : ScriptableObject
{
    [field: Tooltip("Items are ordered like numpad: 7 8 9 | 4 5 6 | 1 2 3")]
    [field: SerializeField] Item[] Items = new Item[9];
    [field: SerializeField] Item ResultItem;

    internal IReadOnlyCollection<Item> GetItems => Items;
    internal Item GetResultItem => ResultItem;

    internal bool CompareRecipes(Item[] _compared)
    {

        if (_compared[0] != Items[0])
        {
            Debug.Log("1 Item is wrong!");
            return false;
        }
        else if (_compared[1] != Items[1])
        {
            Debug.Log("2 Item is wrong!");
            return false;
        }
        else if (_compared[2] != Items[2])
        {
            Debug.Log("3 Item is wrong!");
            return false;
        }
        else if (_compared[3] != Items[3])
        {
            Debug.Log("4 Item is wrong!");
            return false;
        }
        else if (_compared[4] != Items[4])
        {
            Debug.Log("5 Item is wrong!");
            return false;
        }
        else if (_compared[5] != Items[5])
        {
            Debug.Log("6 Item is wrong!");
            return false;
        }
        else if (_compared[6] != Items[6])
        {
            Debug.Log("7 Item is wrong!");
            return false;
        }
        else if (_compared[7] != Items[7])
        {
            Debug.Log("8 Item is wrong!");
            return false;
        }
        else if (_compared[8] != Items[8])
        {
            Debug.Log("9 Item is wrong!");
            return false;
        }
        else
        {
            Debug.Log("Alles Gut");
            return true;
        }
    }
}