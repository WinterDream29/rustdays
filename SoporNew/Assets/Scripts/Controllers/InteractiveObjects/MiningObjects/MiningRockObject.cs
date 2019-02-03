using Assets.Scripts.Models;
using Assets.Scripts.Models.ResourceObjects;
using UnityEngine;

namespace Assets.Scripts.Controllers.InteractiveObjects.MiningObjects
{
    public class MiningRockObject : MiningBaseObject 
    {
        public override HolderObject GetResource()
        {
            var amount = GetMiningAmount();

            InitialAmount -= GetMiningAmount();

            var precent = Random.Range(0, 100);
            if(precent < 30)
                return HolderObjectFactory.GetItem(typeof(MetalOre), amount);

            return HolderObjectFactory.GetItem(typeof(StoneResource), amount);
        }

        public override void PlayerInteract(HolderObject interactObject, GameManager gameManager, Vector3? hitPosition = null)
        {
            base.PlayerInteract(interactObject, gameManager, hitPosition);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.RockHit);
        }

        public override void Destroy()
        {
            gameObject.SetActive(false);
            SoundManager.PlaySFX(WorldConsts.AudioConsts.RockDestroy);
        }
    }
}
