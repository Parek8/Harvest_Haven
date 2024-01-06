using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

internal class Harvestable : Interactable
{
    [field: SerializeField] List<Item> items = new();
    internal override void Interact()
    {
        foreach (Item item in items)
            GameManager.game_manager.player_inventory.Add(item);

        Destroy(gameObject);
    }

    internal void Setup(List<Item> items)
    {
        this.items = items;
    }
}