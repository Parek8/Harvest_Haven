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

            _savedContent += $"{_slot.slot_index}:{_slot.Get_Item().item_id};";
        }

        string _path = Directory.GetCurrentDirectory() + _inventorySavePath;
        if (File.Exists(_path))
        {
            StreamWriter _w = new StreamWriter(_path);

            _w.WriteLine(_savedContent);
        }
        else
        {
            File.Create(_savedContent);
            SaveInventory();
        }
    }

    public IReadOnlyDictionary<int, Item> LoadInventory()
    {
        Dictionary<int, Item> _allItems = GameManager.game_manager._allItems;

        string _path = Directory.GetCurrentDirectory() + _inventorySavePath;
        if (File.Exists(_path))
        {
            StreamReader _r = new StreamReader(_path);

            string _content = _r.ReadLine();
            string[] _slots = _content.Split(';');

            for (int i = 0; i < _slots.Length; i++)
            {
                string _slot = _slots[i];

                string[] _slotPair = _slot.Split(":");
                int _slotId = Convert.ToInt32(_slotPair[0]);
                int _itemId = Convert.ToInt32(_slotPair[1]);


            }
        }

        return _allItems;
    }
    private void OnApplicationQuit()
    {
        string _path = Directory.GetCurrentDirectory() + _inventorySavePath;

        Debug.Log(_path);
        SaveInventory();
        Debug.Log(_path+"asdasdasdasd");
    }
}
