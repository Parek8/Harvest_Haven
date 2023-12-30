using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingResultSlot : MonoBehaviour
{
    [field: SerializeField] List<CraftingSlot> _slots;
    [field: SerializeField] Image _resultImage;

    void FixedUpdate()
    {
        FindRecipe();
    }

    public void FindRecipe()
    {
        Item[] _items = new Item[9];
        for (int i = 0; i < _slots.Count; i++)
            _items[i] = _slots[i].Get_Item();

        Item _resultItem = CraftingManager.craftingManager.FindRecipeResult(_items);
        if (_resultItem != null)
            ShowResult(_resultItem);
        else
            ClearResult();
    }
    private void ShowResult(Item _result)
    {
        this._resultImage.sprite = _result.item_icon;
    }
    private void ClearResult()
    {
        this._resultImage.sprite = null;
    }
}