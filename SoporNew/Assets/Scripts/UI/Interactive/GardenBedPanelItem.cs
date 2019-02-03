using System;
using Assets.Scripts.Models;
using Assets.Scripts.Ui;
using UnityEngine;

namespace Assets.Scripts.UI.Interactive
{
    public class GardenBedPanelItem : View
    {
        public UISprite Icon;
        public UILabel NameLabel;
        public UILabel AmountLabel;
        public BoxCollider Collider;

        private Action<GardenBedPanelItem, HolderObject> _onClickAction;
        public HolderObject ItemHolder { get; private set; }
        public void Set(HolderObject item, Action<GardenBedPanelItem, HolderObject> onClickAction)
        {
            ItemHolder = item;

            UpdateView();

            _onClickAction = onClickAction;

            UIEventListener.Get(Collider.gameObject).onClick += OnClickItem;
        }

        public override void UpdateView()
        {
            base.UpdateView();

            NameLabel.text = Localization.Get(ItemHolder.Item.LocalizationName);
            AmountLabel.text = ItemHolder.Amount.ToString();
            Icon.spriteName = ItemHolder.Item.IconName;
        }

        private void OnClickItem(GameObject go)
        {
            if (_onClickAction != null)
                _onClickAction(this, ItemHolder);
        }

        void OnDestroy()
        {
            _onClickAction = null;
        }
    }
}
