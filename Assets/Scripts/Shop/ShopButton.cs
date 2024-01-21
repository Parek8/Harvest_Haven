using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class ShopButton : MonoBehaviour
{
    [field: SerializeField] Image ItemIcon;
    [field: SerializeField] TMP_Text ItemName;
    [field: SerializeField] Button ItemAction;
    Item _item;
    internal void Buy()
    {
        GameManager.game_manager.player_inventory.Add(_item);
    }
    internal void Sell()
    {
        if (GameManager.game_manager.player_inventory.Remove(_item))
        {

        }
        else
            Debug.Log("Something went wrong while trading!");
    }

    internal void BoughtItem()
    {
        if (Tutorial.TutorialInstance != null)
            Tutorial.TutorialInstance.BoughtItem();
    }

    internal void SoldItem()
    {
        if (Tutorial.TutorialInstance != null)
            Tutorial.TutorialInstance.SoldItem();
    }
    internal void SetupButton(ShopButtonAction _buttonAction, Item _item)
    {
        this._item = _item;
        this.ItemIcon.sprite = _item.ItemIcon;
        this.ItemName.text = _item.name;
        ItemAction.transform.GetChild(0).GetComponent<TMP_Text>().text = (_buttonAction == ShopButtonAction.buy) ? "Buy" : "Sell";
        ItemAction.onClick.AddListener((_buttonAction == ShopButtonAction.buy) ? Buy : Sell);
        ItemAction.onClick.AddListener((_buttonAction == ShopButtonAction.buy) ? BoughtItem : SoldItem);
    }

    internal enum ShopButtonAction
    {
        buy,
        sell,
    }
}