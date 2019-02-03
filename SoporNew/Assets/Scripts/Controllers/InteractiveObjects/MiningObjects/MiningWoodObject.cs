using Assets.Scripts.Models;
using Assets.Scripts.Models.ResourceObjects;
using UnityEngine;

namespace Assets.Scripts.Controllers.InteractiveObjects.MiningObjects
{
    public class MiningWoodObject : MiningBaseObject
    {
        //public Animation DestroyAnimation;
        public BoxCollider InteractCollider;
        public GameObject TreeObject;
        public GameObject StumpObject;

        public override HolderObject GetResource()
        {
            var amount = GetMiningAmount();
            InitialAmount -= amount;
            return HolderObjectFactory.GetItem(typeof (WoodResource), amount);
        }

        public override void PlayerInteract(HolderObject interactObject, GameManager gameManager, Vector3? hitPosition = null)
        {
            base.PlayerInteract(interactObject, gameManager, hitPosition);

            var rand = Random.Range(0, 3);
            string soundName = WorldConsts.AudioConsts.TreeHit1;

            switch(rand)
            {
                case 0:
                    soundName = WorldConsts.AudioConsts.TreeHit1;
                    break;
                case 1:
                    soundName = WorldConsts.AudioConsts.TreeHit2;
                    break;
                case 2:
                    soundName = WorldConsts.AudioConsts.TreeHit3;
                    break;
            }
            SoundManager.PlaySFX(soundName);
        }

        public override void Destroy()
        {
            //InteractCollider.enabled = false;
            //DestroyAnimation.Play();
            TreeObject.SetActive(false);
            StumpObject.SetActive(true);
            SoundManager.PlaySFX(WorldConsts.AudioConsts.TreeFall, false, 0, 0.7f);
        }
    }
}
