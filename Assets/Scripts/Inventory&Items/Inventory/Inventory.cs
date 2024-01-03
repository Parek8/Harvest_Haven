using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Inventory : MonoBehaviour
{
    [field: SerializeField] Transform weaponPoint;

    public List<Inventory_Slot> slots { get; private set; } = new();
    Dictionary<int, Inventory_Slot> slot_ids = new();

    Item _equipped_item;
    public Item Equipped_Item => _equipped_item;
    Action _slotChanged;
    Character_Stats _stats;
    void Start()
    {
        Inventory_Slot[] tmp_array = FindObjectsByType<Inventory_Slot>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        foreach (Inventory_Slot slot in tmp_array)
        {
            slots.Add(slot);
            slot_ids.Add(slot.slot_index, slot);
        }

        slots.Sort((item1, item2) => item1.slot_index.CompareTo(item2.slot_index));

        _stats = GetComponent<Character_Stats>();
        LoadInventory();
        Clear_Item();
    }
    public void Add(Item item)
    {
        if (item != null)
        {
            Inventory_Slot slot = Find_First_Slot(item);
            slot.Assign_Item(item);
        }
        else
            Debug.LogError("Picked up null!");
    }

    public bool Remove(Item item)
    {
        if (item != null && ContainsItem(item))
        {
            slots.Find(slot => slot.Get_Item() == item).DropItem();
            return true;
        }
        return false;
    }
    
    public bool ContainsItem(Item item)
    {
        Inventory_Slot _occupiedSlot = slots.Find(slot => slot.Get_Item() == item);

        return (_occupiedSlot != null);
    }
    private Inventory_Slot Find_First_Slot(Item item)
    {
        Inventory_Slot used_slot;

        if (item.is_stackable)
            used_slot = slots.Find(slot => slot.Get_Item() == item && slot.IsAvailable);
        else
            used_slot = slots.Find(slot => slot.Get_Item() == null && slot.IsAvailable);

        if (used_slot == null)
            used_slot = slots.Find(slot => slot.Get_Item() == null && slot.IsAvailable);

        return used_slot;
    }
    public Inventory_Slot Return_Closest_Slot()
    {
        float min_distance = float.MaxValue;
        Inventory_Slot closest_slot = slots[0];

        foreach (Inventory_Slot slot in slots)
        {
            float slot_distance = slot.Return_Distance_From_Mouse();
            if (slot_distance < min_distance)
            {
                min_distance = slot_distance;
                closest_slot = slot;

            }
        }
        return closest_slot;
    }
    public void Equip(Item item)
    {
        _equipped_item = item;
        _slotChanged?.Invoke();
        if (item != null)
        {
            InstantiateItem();
            _stats.Change_State(item.tool_type);
        }
        else
            _stats.Change_State(Tool_Type.NULL);
    }
    private void InstantiateItem()
    {
        int childCount = weaponPoint.childCount;
        for (int i = 0;  i < childCount; i++)
        {
            Destroy(weaponPoint.GetChild(i).gameObject);
        }
        if(_equipped_item.item_prefab != null)
            Instantiate(_equipped_item.item_prefab, weaponPoint);
    }

    public void Clear_Item()
    {
        _equipped_item = GameManager.game_manager.Null_Item;
    }    

    public void AddToSlotChangedAction(Action action)
    {
        _slotChanged += action;
    }
    public void RemoveFromSlotChangedAction()
    {
        _slotChanged -= Clear_Item;
    }
    private void LoadInventory()
    {
        List<InventoryEntry> _inv = (List<InventoryEntry>)GameManager.game_manager.saveManager.LoadInventory();

        foreach(InventoryEntry _entry in _inv)
        {
            slots[_entry.SlotID].Assign_Item(_entry.Item, _entry.ItemCount);
        }
    }

    public bool IsEquippedItemTool()
    {
        if (_equipped_item == null) return false;

        return (_equipped_item.is_tool);
    }

    public bool IsEquippedFood()
    {
        if (_equipped_item == null) return false;

        return (_equipped_item.is_eatable);
    }

    public IReadOnlyCollection<Item> GetAllItems()
    {
        List<Item> _items = new List<Item>();

        foreach (Inventory_Slot _slot in slots)
            if (_slot.Get_Item() != null)
                _items.Add(_slot.Get_Item());

        return _items;
    }

    public IReadOnlyCollection<Item> GetAllSmeltableItems()
    {
        List<Item> _items = new List<Item>();

        foreach (Inventory_Slot _slot in slots)
            if (_slot.Get_Item() != null && _slot.Get_Item().is_smeltable)
                _items.Add(_slot.Get_Item());

        return _items;
    }

    public IReadOnlyCollection<Item> GetAllSmeltingFuel()
    {
        List<Item> _items = new List<Item>();

        foreach (Inventory_Slot _slot in slots)
            if (_slot.Get_Item() != null && _slot.Get_Item().is_fuel)
                _items.Add(_slot.Get_Item());

        return _items;
    }

    public int GetItemCountInInventory(Item _item)
    {
        List<Inventory_Slot> _slots = slots.FindAll(_slot => _slot.Get_Item() == _item);
        int _count = 0;
        foreach (Inventory_Slot _slot in _slots)
            _count += _slot.ItemCount;
        return _count;
    }

    public void DecreaseItemCount(Item _item)
    {
        Find_First_Slot(_item).DecreaseCount();
    }
}