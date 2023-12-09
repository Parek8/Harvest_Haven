using System;

[Serializable]
public enum Rarities
{
    common,
    uncommon,
    rare,
    epic,
    legendary
}
[Serializable]

public enum Scenes
{
    Loading,
    Overworld,
    Main_Menu,
}
[Serializable]

public enum Tool_Type
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
[Serializable]
public enum KeybindNames
{
    forward = 119,
    backward = 115,
    left_strafe = 97,
    right_strafe = 100,
    sprint = 304,
    jump = 32,
    left_attack = 323,
    interact = 101,
    inventory = 9,
    slot_1 = 49,
    slot_2 = 50,
    slot_3 = 51,
    slot_4 = 52,
    slot_5 = 53,
    slot_6 = 54,
    slot_7 = 55,
    slot_8 = 56,
    slot_9 = 57,
}

public enum PlayerState
{
    seeding,
    normal,
    watering,
}