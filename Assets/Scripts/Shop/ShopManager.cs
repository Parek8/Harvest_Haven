using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Talkable
{
    [field: SerializeField] List<Item> BuyableItems;
    [field: SerializeField] List<Item> SoldableItems;

    [field: SerializeField] GameObject BuyWindow;
    [field: SerializeField] GameObject SellWindow;
    [field: SerializeField] GameObject ShopButton;

    private void Start()
    {
        foreach (Item buy in BuyableItems)
            Instantiate(ShopButton, BuyWindow.transform).GetComponent<ShopButton>().SetupButton(global::ShopButton.ShopButtonAction.buy, buy);

        foreach (Item sell in SoldableItems)
            Instantiate(ShopButton, SellWindow.transform).GetComponent<ShopButton>().SetupButton(global::ShopButton.ShopButtonAction.sell, sell);
    }
}