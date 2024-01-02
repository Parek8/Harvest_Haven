using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [field: SerializeField] Image ItemIcon;
    [field: SerializeField] TMP_Text ItemName;
    [field: SerializeField] Button ItemAction;
    Item _item;
    public void Buy()
    {
        GameManager.game_manager.player_inventory.Add(_item);
    }
    public void Sell()
    {
        if (GameManager.game_manager.player_inventory.Remove(_item))
        {

        }
        else
            Debug.Log("Something went wrong while trading!");
    }

    public void SetupButton(ShopButtonAction _buttonAction, Item _item)
    {
        this._item = _item;
        this.ItemIcon.sprite = _item.item_icon;
        this.ItemName.text = _item.name;
        ItemAction.transform.GetChild(0).GetComponent<TMP_Text>().text = (_buttonAction == ShopButtonAction.buy) ? "Buy" : "Sell";
        ItemAction.onClick.AddListener((_buttonAction == ShopButtonAction.buy) ? Buy : Sell);
    }

    public enum ShopButtonAction
    {
        buy,
        sell,
    }
}