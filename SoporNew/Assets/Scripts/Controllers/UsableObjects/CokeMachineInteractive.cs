using UnityEngine;

namespace Assets.Scripts.Controllers.UsableObjects
{
    public class CokeMachineInteractive : UsableObject
    {
        public GameObject CokePrefab;
        public Transform CokeSpawnPosition;

        protected override void Init()
        {
            base.Init();

            var gmGo = GameObject.FindWithTag("GameManager");
            if (gmGo != null)
                GameManager = gmGo.GetComponent<GameManager>();
        }

        public override void Use(GameManager gameManager)
        {
            base.Use(gameManager);

            if (CurrencyManager.CurrentCurrency < 60)
            {
                GameManager.Player.MainHud.ShowHudText(Localization.Get("no_money"), HudTextColor.Red);
            }
            else
            {
                SpawnCoke();
                CurrencyManager.AddCurrency(-60);
            }
        }

        private void SpawnCoke()
        {
            Instantiate(CokePrefab, CokeSpawnPosition.position, Quaternion.identity);
            SoundManager.PlaySFX(WorldConsts.AudioConsts.CokeMachine);
        }
    }
}