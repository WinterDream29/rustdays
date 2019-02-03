using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Ui;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GettingResourceView : View
    {
        public UIGrid GridItems;
        public List<GettingResourceItemView> ItemsList;

        private int _amountShowed;
        private int _currentAmountShowed;

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            _amountShowed = 99;

            foreach (var gettingResourceItemView in ItemsList)
                gettingResourceItemView.Hide();
        }

        public void ShowItem(string spriteName, int amount, string itemName)
        {
            bool showed = false;
            foreach (var gettingResourceItemView in ItemsList)
            {
                if (!gettingResourceItemView.IsShowing)
                {
                    gettingResourceItemView.gameObject.name = _amountShowed + "_item";
                    gettingResourceItemView.Icon.spriteName = spriteName;
                    gettingResourceItemView.AmountLabel.text = "+" + amount;
                    gettingResourceItemView.NameLabel.text = Localization.Get(itemName);
                    gettingResourceItemView.Show();
                    _amountShowed--;
                    _currentAmountShowed++;
                    showed = true;
                    StartCoroutine(DelayHideItem(gettingResourceItemView));
                    break;
                }
            }
            if (!showed)
            {
                ItemsList[ItemsList.Count - 1].HideImmediately();
                ShowItem(spriteName, amount, itemName);
            }
            GridItems.Reposition();
        }

        private IEnumerator DelayHideItem(GettingResourceItemView item)
        {
            yield return new WaitForSeconds(2f);
            item.Hide();

            _currentAmountShowed--;
            if (_currentAmountShowed == 0)
                _amountShowed = 99;
        }
    }
}
