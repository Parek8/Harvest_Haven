using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
internal class Item : ScriptableObject
{
    // ---NEW---
    [field: Header("Global Item Info")]
    [field: SerializeField] public int ItemID { get; private set; } = 0;
    [field: SerializeField] public string ItemName { get; private set; } = "";
    [field: SerializeField] public string ItemDescription { get; private set; } = "";
    [field: SerializeField] public List<ItemTags> ItemDescriptionTags { get; private set; } = new();
    [field: SerializeField] public Sprite ItemIcon { get; private set; }
    [field: Range(0.0f, 1.0f)] public float SpawnRate;
    [field: SerializeField] public GameObject ItemPrefab { get; private set; }

    [field: Header("Item Functionalities")]
    public bool IsEatable = false;
    public bool IsTool = false;
    public bool IsStackable = false;
    public bool IsFuel = false;
    public bool IsSmeltable = false;
    public bool IsPlantable = false;

    [field: Header("Item Specific Variables")]
    [field: SerializeField, SerializeFieldOnCondition("IsSmeltable", true, ComparisonType.Equals)] public Item SmeltItem { get; private set; }
    [field: SerializeField, SerializeFieldOnCondition("IsTool", true, ComparisonType.Equals)] public ToolTypes ToolType { get; private set; }
    [field: SerializeField, SerializeFieldOnCondition("IsTool", true, ComparisonType.Equals)] public float ToolDamage { get; private set; }
    [field: SerializeField, SerializeFieldOnCondition("IsPlantable", true, ComparisonType.Equals)] public PlantObject PlantableObject { get; private set; }
    [field: SerializeField, SerializeFieldOnCondition("IsEatable", true, ComparisonType.Equals)] public float FoodRegen { get; private set; }

    internal Inventory_Slot AssignedSlot = null;
    internal enum ToolTypes
    {
        sword,
        axe,
        pickaxe,
        hoe,
        fishing_rod,
        watering_can,
        building_hammer,
        bare_hands,
        food,
        seeds,
        building,
        other,
        NULL,
    }
    internal enum Rarities
    {
        common,
        uncommon,
        rare,
        epic,
        legendary
    }
    internal enum ItemTags
    {
        Crafting,
        Cooking,
        Smelting,
        Fuel,
        Food,
        Tool,
        Seeds,
        EasterEgg,
    }
}