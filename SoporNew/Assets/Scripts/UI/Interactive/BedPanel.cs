using Assets.Scripts.Ui;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Assets.Scripts.UI.Interactive
{
    public class BedPanel : View
    {
        public GameObject SetSpawnButton;
        public GameObject DreamButton;
        public GameObject TmeUpButton;
        public GameObject TimeDownButton;
        public GameObject CloseObject;
        public UILabel SleepLabel;

        private float _sleepTime = 4;

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            SleepLabel.text = _sleepTime.ToString();

            UIEventListener.Get(SetSpawnButton).onClick += OnSetSpawn;
            UIEventListener.Get(DreamButton).onClick += OnDreamClick;
            UIEventListener.Get(TmeUpButton).onClick += OnTimeUpClick;
            UIEventListener.Get(TimeDownButton).onClick += OnTimeDownClick;
            UIEventListener.Get(CloseObject).onPress += OnCloseClick;
        }

        private void OnCloseClick(GameObject go, bool pressed)
        {
            if (pressed)
                Hide();
        }

        private void OnSetSpawn(GameObject go)
        {
            GameManager.Player.RespawnPoint.position = GameManager.Player.transform.position;
        }
        private void OnDreamClick(GameObject go)
        {
            StartCoroutine(Dream());
        }

        private IEnumerator Dream()
        {
            GameManager.DisplayManager.ShowPlayerDeadSplash();

            yield return new WaitForSeconds(0.8f);

            var currentHour = GameManager.World.Sky.Cycle.Hour;

            if (currentHour + _sleepTime > 24)
            {
                var t = 24 - currentHour;
                _sleepTime -= t;
                GameManager.World.Sky.Cycle.Hour = _sleepTime;
            }
            else
            {
                GameManager.World.Sky.Cycle.Hour = currentHour + _sleepTime;
            }

            GameManager.PlayerModel.ChangeHealth(_sleepTime * 5);
            GameManager.PlayerModel.ChangeThirst(-_sleepTime * 4);
            GameManager.PlayerModel.ChangeHunger(-_sleepTime * 3);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerDream);

            yield return new WaitForSeconds(1.2f);

            ShowRewardedAd();
        }

        private void OnTimeDownClick(GameObject go)
        {
            if (_sleepTime > 1.0f)
            {
                _sleepTime -= 1;
                SleepLabel.text = _sleepTime.ToString();
            }
        }
        private void OnTimeUpClick(GameObject go)
        {
            if (_sleepTime < 8.0f)
            {
                _sleepTime += 1;
                SleepLabel.text = _sleepTime.ToString();
            }
        }

        public void ShowRewardedAd()
        {
            if (!GameManager.IapManager.IsBuyNoAds && Advertisement.IsReady("rewardedVideo"))
            {
                var options = new ShowOptions {resultCallback = HandleShowResult};
                Advertisement.Show("rewardedVideo", options);
            }
            else
            {
                GameManager.DisplayManager.HidePlayerDeadSplash();
            }
        }

        private void HandleShowResult(ShowResult result)
        {
            GameManager.DisplayManager.HidePlayerDeadSplash();
            //switch (result)
            //{
            //    case ShowResult.Finished:
            //        break;
            //    case ShowResult.Skipped:
            //        break;
            //    case ShowResult.Failed:
            //        break;
            //}
        }
    }
}
