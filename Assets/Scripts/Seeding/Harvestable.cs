using System;
using System.Collections.Generic;
using UnityEngine;

internal class Harvestable : Interactable
{
    [field: SerializeField] List<Item> items = new();
    Action OnHarvest;
    internal override void Interact()
    {
        foreach (Item item in items)
            GameManager.GameManagerInstance.PlayerInventory.Add(item);
        OnHarvest?.Invoke();

        if (Tutorial.TutorialInstance != null)
            Tutorial.TutorialInstance.Harvested();

        Destroy(gameObject);
    }

    internal void Setup(List<Item> items, Action onHarvest)
    {
        this.items = items;
        this.OnHarvest = onHarvest;
    }
}