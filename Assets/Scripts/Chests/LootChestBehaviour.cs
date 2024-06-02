using System.Collections.Generic;
using UnityEngine;

public class LootChestBehaviour : MonoBehaviour
{
    [field: SerializeField] GameObject LootChestItemPrefab;

    private void Start()
    {
        int _randomItemCount = Random.Range(1, 5);

        List<Item> AllItemsDictionary = GameManager.GameManagerInstance.AllLootableItems;

        for (int i = 0; i < _randomItemCount; i++)
        {
            ChestInventoryButton _object = Instantiate(LootChestItemPrefab, transform).GetComponent<ChestInventoryButton>();
            Item _item = AllItemsDictionary[Random.Range(1, AllItemsDictionary.Count - 1)];
            int _itemCount = (!_item.IsStackable) ? 1 : Random.Range(1, 10);
            _object.AssignItem(_item, _itemCount);
        }
    }
}