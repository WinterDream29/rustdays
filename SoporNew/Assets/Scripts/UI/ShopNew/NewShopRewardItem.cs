using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.UI.ShopNew
{
    public class NewShopRewardItem : MonoBehaviour
    {
        public UISprite Icon;
        public UILabel AmountLabel;
        public UILabel DescriptionLabel;

        public void Init(string id, int amount)
        {
            if (id == "gold")
            {
                Icon.spriteName = "gold_icon";
                DescriptionLabel.text = Localization.Get("gold_name");
            }
            else
            {
                var item = BaseObjectFactory.GetItem(id);
                Icon.spriteName = item.IconName;
                DescriptionLabel.text = Localization.Get(item.LocalizationName);
            }

            AmountLabel.text = amount.ToString();
        }
    }
}
