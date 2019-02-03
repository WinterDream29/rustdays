using UnityEngine;

namespace Assets.Scripts.Controllers.UsableObjects
{
    public class TorchOnWallInteractive : UsableObject, IConverter
    {
        public GameObject FireObject;
        public ParticleSystem FireParticles;
        public AudioSource Sound;

        public bool IsBurning { get; private set; }
        public bool AutoStart { get; set; }

        protected override void Init()
        {
            base.Init();

            FireObject.SetActive(false);

            if (AutoStart)
                Fire();
        }

        public override void Use(GameManager gameManager)
        {
            if (IsBurning)
            {
                SnuffOut();
            }
            else
            {
                Fire();
            }
        }

        private void SnuffOut()
        {
            FireParticles.Stop();
            FireObject.SetActive(false);
            Sound.Stop();
            IsBurning = false;
        }

 
        public void Fire()
        {
            FireObject.SetActive(true);
            FireParticles.Play();
            Sound.Play();
            IsBurning = true;
        }
    }
}
