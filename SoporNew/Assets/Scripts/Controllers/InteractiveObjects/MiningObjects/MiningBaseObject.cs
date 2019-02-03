using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers.InteractiveObjects.MiningObjects
{
    public abstract class MiningBaseObject : InteractiveObject
    {
        public int MinInitialAmount = 1;
        public int MaxInitialAmount = 10;
        public int MinMiningAmount = 1;
        public int MaxMiningAmount = 2;

        protected int InitialAmount;

        protected override void Init()
        {
            base.Init();
            InitialAmount = Random.Range(MinInitialAmount, MaxInitialAmount+1);
        }

        public abstract HolderObject GetResource();

        protected int GetMiningAmount()
        {
            var amount = Random.Range(MinMiningAmount, MaxMiningAmount + 1);
            amount += InteractObject.Item.MiningAdditionalAmount;
            if (amount < 0)
                amount = 0;

            return amount;
        }

        public override void PlayerInteract(HolderObject interactObject, GameManager gameManager, Vector3? hitPosition = null)
        {
            base.PlayerInteract(interactObject, gameManager, hitPosition);

            var item = GetResource();

            if(GameManager.PlayerModel.Inventory.AddItem(item))
                GameManager.Player.MainHud.ShowAddedResource(item.Item.IconName, item.Amount, item.Item.LocalizationName);
            else
                GameManager.Player.MainHud.ShowHudText(Localization.Get("no_place_in_inventory"), HudTextColor.Red);

            if (InitialAmount <= 0)
                Destroy();
        }
    }
}
