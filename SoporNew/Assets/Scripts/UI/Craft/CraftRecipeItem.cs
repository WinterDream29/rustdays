using Assets.Scripts.Models;
using Assets.Scripts.Ui;
using UnityEngine;

namespace Assets.Scripts.UI.Craft
{
    public class CraftRecipeItem : View
    {
        public UILabel AmountLabel;
        public UISprite IconSprite;
        public UILabel NameLabel;

        public BaseObject Item { get; private set; }
        private int _needAmount;
        private Color _inactiveColor;
        private Color _activeColor;

        public void Init(BaseObject itemModel, int needAmount, int currentAmount, Color inactiveColor, Color activeColor)
        {
            Item = itemModel;
            _activeColor = activeColor;
            _inactiveColor = inactiveColor;

            _needAmount = needAmount;
            AmountLabel.text = needAmount + "/" + currentAmount;
            IconSprite.spriteName = itemModel.IconName;
            NameLabel.text = Localization.Get(itemModel.LocalizationName);

            AmountLabel.color = needAmount > currentAmount ? _inactiveColor : _activeColor;
        }

        public void UpdateView(int currentAmount)
        {
            AmountLabel.text = currentAmount + "/" + _needAmount;
            AmountLabel.color = _needAmount > currentAmount ? _inactiveColor : _activeColor;
        }
    }
}
