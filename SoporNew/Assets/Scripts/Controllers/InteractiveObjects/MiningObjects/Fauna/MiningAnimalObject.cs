using Assets.Scripts.Controllers.Fauna;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Food;
using UnityEngine;

namespace Assets.Scripts.Controllers.InteractiveObjects.MiningObjects.Fauna
{
    public class MiningAnimalObject : MiningBaseObject
    {
        public Animal Animal;
        public Controllers.Fauna.Enemy Enemy;
        public override HolderObject GetResource()
        {
            var amount = UnityEngine.Random.Range(MinMiningAmount, MaxMiningAmount);
            InitialAmount -= amount;
            return HolderObjectFactory.GetItem(typeof(Meat), amount);
        }

        public override void PlayerInteract(HolderObject interactObject, GameManager gameManager, Vector3? hitPosition = null)
        {
            base.PlayerInteract(interactObject, gameManager, hitPosition);

            if (gameObject.activeInHierarchy && hitPosition != null)
            {
                if(Animal != null)
                    StartCoroutine(Animal.BloodSplat(hitPosition));
                if (Enemy != null)
                    StartCoroutine(Enemy.BloodSplat(hitPosition));
            }
            SoundManager.PlaySFX(WorldConsts.AudioConsts.DamageEnemy);
        }

        public override void Destroy()
        {
            if(Animal != null)
                Animal.gameObject.SetActive(false);
            if (Enemy != null)
                Enemy.gameObject.SetActive(false);
        }
    }
}
