using Assets.Scripts;
using Assets.Scripts.Models;
using Assets.Scripts.Ui;
using Assets.Scripts.UI.Craft;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuyResourcesDialog : View 
{
    public UILabel PriceLabel;
    public UILabel NotCheckItemsLabel;
    public GameObject BuyButton;
    public GameObject CloseButton;

    public Action NoManyAction { get; set; }

    private List<HolderObject> _notCheckItems;
    private int _price;
    private CraftSelectedItemView _selectedItemView;
    public override void Init(GameManager gameManager)
    {
        base.Init(gameManager);

        UIEventListener.Get(BuyButton).onClick += BuyButtonClick;
        UIEventListener.Get(CloseButton).onClick += CloseButtonClick;

        Hide();
    }

    public void Show(List<HolderObject> notCheckItems, CraftSelectedItemView selectedItemView)
    {
        _price = 0;
        _notCheckItems = notCheckItems;
        _selectedItemView = selectedItemView;

        var notCheck0 = Localization.Get(notCheckItems[0].Item.LocalizationName) + "  (" + notCheckItems[0].Amount + ")";
        var resString = string.Format(Localization.Get("no_resource_log"), notCheck0, string.Empty, string.Empty);
        if (notCheckItems.Count > 1)
        {
            var notCheck1 = Localization.Get(notCheckItems[1].Item.LocalizationName) + "  (" + notCheckItems[1].Amount + ")";
            resString = string.Format(Localization.Get("no_resource_log"), notCheck0, notCheck1, string.Empty);

            if (notCheckItems.Count > 2)
            {
                var notCheck2 = Localization.Get(notCheckItems[2].Item.LocalizationName) + "  (" + notCheckItems[2].Amount + ")";
                resString = string.Format(Localization.Get("no_resource_log"), notCheck0, notCheck1, notCheck2);
            }
        }

        NotCheckItemsLabel.text = resString;

        foreach (var item in notCheckItems)
        {
            _price += item.Amount * item.Item.ShopPrice;
        }

        PriceLabel.text = _price.ToString();

        if (_price > CurrencyManager.CurrentCurrency)
            PriceLabel.color = Color.red;
        else
            PriceLabel.color = Color.white;

        base.Show();
    }

    private void BuyButtonClick(GameObject go)
    {
        BuyItems();
    }

    private void CloseButtonClick(GameObject go)
    {
        Hide();
    }

    private void BuyItems()
    {
        if (PayPriceShopItem())
        {
            foreach (var curItem in _notCheckItems)
            {
                var isAdd = GameManager.PlayerModel.Inventory.AddItem(curItem);
                if (!isAdd)
                    GameManager.PlacementItemsController.DropItemToGround(GameManager, curItem);

                UpdateView();
                GameManager.Player.MainHud.CraftPanel.UpdateView();
                _selectedItemView.UpdateView();
                Hide();
            }
        }
        else
        {
            ShowShop();
        }
    }
    private bool PayPriceShopItem()
    {
        if (_price <= CurrencyManager.CurrentCurrency)
        {
            CurrencyManager.AddCurrency(-_price);
            return true;
        }
        else
            return false;
    }

    private void ShowShop()
    {
        if (NoManyAction != null)
            NoManyAction();
    }
}
