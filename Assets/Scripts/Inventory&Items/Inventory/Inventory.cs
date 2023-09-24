using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [field: SerializeField] public List<Inventory_Slot> slots { get; private set; }
    Dictionary<int, Inventory_Slot> slot_ids = new();

    void Start()
    {
        Inventory_Slot[] tmp_array = FindObjectsByType<Inventory_Slot>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        foreach (Inventory_Slot slot in tmp_array)
        {
            slots.Add(slot);
            slot_ids.Add(slot.slot_index, slot);
        }
        slots.Sort((item1, item2) => item1.slot_index.CompareTo(item2.slot_index));
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
    private Inventory_Slot Return_Closest_Slot()
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
}