using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Harvestable : Interactable
{
    List<Item> items;
    public override void Interact()
    {
        foreach (Item item in items)
            GameManager.game_manager.player_inventory.Add(item);

        Destroy(gameObject);
    }

    public void Setup(List<Item> items)
    {
        this.items = items;
    }
}