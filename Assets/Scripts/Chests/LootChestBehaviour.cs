using System.Collections.Generic;
using UnityEngine;

public class LootChestBehaviour : MonoBehaviour
{
    [field: SerializeField] GameObject LootChestItemPrefab;

    private void Start()
    {
        int _randomItemCount = Random.Range(1, 5);

        List<Item> _allItems = GameManager.game_manager.AllLootableItems;

        for (int i = 0; i < _randomItemCount; i++)
        {
            ChestInventoryButton _object = Instantiate(LootChestItemPrefab, transform).GetComponent<ChestInventoryButton>();
            Item _item = _allItems[Random.Range(1, _allItems.Count - 1)];
            int _itemCount = (!_item.IsStackable) ? 1 : Random.Range(1, 10);
            _object.AssignItem(_item, _itemCount);
        }
    }
}