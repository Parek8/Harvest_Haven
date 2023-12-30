using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetInventoryItems : MonoBehaviour
{
    [field: SerializeField] ItemMenuSlot Slot;
    private void OnEnable()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        List<Item> _items = (List<Item>)GameManager.game_manager.player_inventory.GetAllItems();
        foreach (Item _item in _items)
        {
            GameObject _obj = Instantiate(Slot.gameObject, transform);
            _obj.GetComponent<ItemMenuSlot>().Init(_item);
        }    
    }
}