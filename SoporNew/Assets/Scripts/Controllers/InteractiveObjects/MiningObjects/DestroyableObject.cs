using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers.InteractiveObjects.MiningObjects
{
    public class DestroyableObject : InteractiveObject
    {
        public GameObject MainObject;
        public int Hp;

        public int CurrentHp { get; set; }
        public BaseObject ItemModel { get; set; }
        public bool Initialized { get; set; }

        protected override void Init()
        {
            base.Init();
            if (Initialized)
                return;
            CurrentHp = Hp;
        }

        public override void PlayerInteract(HolderObject interactObject, GameManager gameManager, Vector3? hitPosition = null)
        {
            base.PlayerInteract(interactObject, gameManager, hitPosition);

            CurrentHp -= interactObject.Item.Damage;
            if (CurrentHp > 0)
                GameManager.Player.MainHud.ShowHudText("[" + CurrentHp + "/" + Hp + "]", HudTextColor.Yellow);
            if (CurrentHp <= 0)
            {
                CurrentHp = 0;

                if (ItemModel != null)
                {
                    if (ItemModel.AddDestroyReward)
                    {
                        foreach (var holderObject in ItemModel.CraftRecipe)
                        {
                            var placeObject = HolderObjectFactory.GetItem(holderObject.Item.GetType(), holderObject.Amount / 2);
                            GameManager.PlacementItemsController.DropItemToGround(GameManager, placeObject);
                        }
                    }
                }

                GameManager.PlacementItemsController.RemovePlacedItem(MainObject);
                MainObject.SetActive(false);
                SoundManager.PlaySFX(WorldConsts.AudioConsts.Destroy);
            }
        }

        public void ChangeHealth(GameManager gameManager, int damage)
        {
            CurrentHp -= damage;
            if (CurrentHp <= 0)
            {
                CurrentHp = 0;

                gameManager.PlacementItemsController.RemovePlacedItem(MainObject);
                MainObject.SetActive(false);
                SoundManager.PlaySFX(WorldConsts.AudioConsts.Destroy);
            }
        }
    }
}