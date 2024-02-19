using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class CraftingResultSlot : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] List<CraftingSlot> _slots;
    [field: SerializeField] Image _resultImage;

    Item _result;
    Inventory _playerInventory;
    private void Start()
    {
        _playerInventory = GameManager.game_manager.player_inventory;
    }
    void FixedUpdate()
    {
        FindRecipe();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            Craft();
    }

    private void Craft()
    {
        if (HasSufficientMaterials())
        {
            UseMaterials();
            _playerInventory.Add(_result);

        }
    }

    private Dictionary<Item, uint> GetMaterials()
    {
        Item[] _items = GetRecipe();

        Dictionary<Item, uint> _materials = new();

        foreach (Item _item in _items)
        {
            if (_item != GameManager.game_manager.Null_Item)
            {
                if (_materials.ContainsKey(_item))
                    _materials[_item]++;
                else
                    _materials.Add(_item, 1);
            }
        }

        return _materials;
    }

    private bool HasSufficientMaterials()
    {
        Dictionary<Item, uint> _materials = GetMaterials();

        foreach (Item _material in _materials.Keys)
        {
            if (_playerInventory.GetItemCountInInventory(_material) < _materials[_material])
                return false;
        }

        return true;
    }

    private void UseMaterials()
    {
        Dictionary<Item, uint> _materials = GetMaterials();

        foreach (Item _material in _materials.Keys)
        {
            for (int i = 0; i < _materials[_material]; i++)
                _playerInventory.DecreaseItemCount(_material);
        }
    }
    internal void FindRecipe()
    {
        Item[] _items = GetRecipe();

        Item _resultItem = CraftingManager.craftingManager.FindRecipeResult(_items);
        if (_resultItem != null)
        {
            ShowResult(_resultItem);
            this._result = _resultItem;
        }
        else
            ClearResult();
    }

    private Item[] GetRecipe()
    {
        Item[] _items = new Item[9];
        for (int i = 0; i < _slots.Count; i++)
            _items[i] = _slots[i].Get_Item();

        return _items;
    }

    private void ShowResult(Item _result)
    {
        this._resultImage.sprite = _result.ItemIcon;
    }

    private void ClearResult()
    {
        this._result = null;
        this._resultImage.sprite = null;
    }

}