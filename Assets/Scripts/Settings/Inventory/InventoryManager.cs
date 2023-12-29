using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] string _inventorySavePath = "/saves/playerInventory.cfg";
    public void SaveInventory()
    {
        string _savedContent = "";
        List<Inventory_Slot> _slots = GameManager.game_manager.player_inventory.slots;

        for (int i = 0; i < _slots.Count; i++)
        {
            Inventory_Slot _slot = _slots[i];
            if (!_slot.Is_Empty())
                _savedContent += $"{_slot.slot_index}:{_slot.Get_Item().item_id}:{_slot.ItemCount};";
        }

        string _path = Directory.GetCurrentDirectory() + _inventorySavePath;
        if (File.Exists(_path))
        {
            StreamWriter _w = new StreamWriter(_path);

            _w.WriteLine(_savedContent);
            _w.Close();
        }
        else
        {
            File.Create(_path).Close();
            SaveInventory();
        }
    }

    public IReadOnlyCollection<InventoryEntry> LoadInventory()
    {
        Dictionary<int, Item> _allItems = GameManager.game_manager._allItems;
        List<InventoryEntry> _newItems = new();

        string _path = Directory.GetCurrentDirectory() + _inventorySavePath;
        if (File.Exists(_path))
        {
            StreamReader _r = new StreamReader(_path);

            string _content = _r.ReadLine();
            if (_content != null)
            {
                string[] _slots = _content.Split(';');

                for (int i = 0; i < _slots.Length - 1; i++)
                {
                    string _slot = _slots[i];

                    string[] _slotPair = _slot.Split(":");
                    int _slotId = Convert.ToInt32(_slotPair[0]);
                    int _itemId = Convert.ToInt32(_slotPair[1]);
                    int _itemCount = Convert.ToInt32(_slotPair[2]);

                    _newItems.Add(new InventoryEntry(_itemCount, _slotId, _allItems[_itemId]));
                }
            }
            _r.Close();
        }

        return _newItems;
    }
    private void OnApplicationQuit()
    {
        string _path = Directory.GetCurrentDirectory() + _inventorySavePath;

        SaveInventory();
    }
}
public struct InventoryEntry
{
    private int _slotId;
    private int _itemCount;
    private Item _item;

    public int SlotID => _slotId;
    public int ItemCount => _itemCount;
    public Item Item => _item;

    public InventoryEntry(int _count, int _slotId, Item _item)
    {
        this._slotId = _slotId;
        this._itemCount = _count;
        this._item = _item;
    }
}