using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [field: SerializeField] public int item_id { get; private set; } = 0;
    [field: SerializeField] public string item_name { get; private set; } = "";
    [field: SerializeField] public Sprite item_icon { get; private set; } = null;
    [field: Range(0.0f, 1.0f)] public float spawn_rate;
    //[field: SerializeField] public int count { get; private set; } = 0;
    [field: SerializeField] public GameObject item_prefab { get; private set; }
    [field: SerializeField] public bool is_eatable { get; private set; } = false;
    [field: SerializeField] public bool is_tool { get; private set; } = false;
    [field: SerializeField] public bool is_stackable { get; private set; } = false;
    [field: SerializeField] public Tool_Type tool_type { get; private set; }
    [field: SerializeField] public float tool_damage { get; private set; }
    [field: SerializeField] public PlantObject plantable_object { get; private set; }

    public Inventory_Slot AssignedSlot = null;
}