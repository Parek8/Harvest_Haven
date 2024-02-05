using System.Collections.Generic;
using UnityEngine;

public class LootChestBehaviour : MonoBehaviour
{
    [field: SerializeField] GameObject LootChestItemPrefab;

    private void Start()
    {
        int _randomItemCount = Random.Range(1, 5);

        List<Item> _allItems = GameManager.game_manager.all_items;

        for (int i = 0; i < _randomItemCount; i++)
        {
            ChestInventoryButton _object = Instantiate(LootChestItemPrefab, transform).GetComponent<ChestInventoryButton>();
            _object.AssignItem(_allItems[Random.Range(1, _allItems.Count - 1)], Random.Range(1, 10));
        }
    }
}