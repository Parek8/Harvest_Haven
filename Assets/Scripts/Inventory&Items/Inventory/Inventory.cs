using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    void Start()
    {
        Inventory_Slot[] tmp_array = FindObjectsByType<Inventory_Slot>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        foreach (Inventory_Slot slot in tmp_array)
        {
            slots.Add(slot);
            slot_ids.Add(slot.slot_index, slot);
        }
        slots.Sort((item1, item2) => item1.slot_index.CompareTo(item2.slot_index));


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
    private Inventory_Slot Find_First_Slot(Item item)
    {
        Inventory_Slot used_slot = slots.Find(slot => slot.Get_Item() == item);

        if (used_slot == null)
            used_slot = slots.Find(slot => slot.Get_Item() == null);

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
        InstantiateItem();
    }
    private void InstantiateItem()
    {
        int childCount = weaponPoint.childCount;
        for (int i = 0;  i < childCount; i++)
        {
            Destroy(weaponPoint.GetChild(i).gameObject);
        }
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
}