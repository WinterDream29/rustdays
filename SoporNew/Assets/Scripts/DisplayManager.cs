using Assets.Scripts.Ui;
using Assets.Scripts.UI.Screens;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class DisplayManager : MonoBehaviour
    {
        public UIRoot UiRoot;
        public SplashScreen SplashScreen;
        public View SleepDialog;
        public GameObject PlayerDeadSplash;

        public View CurrentInteractPanel { get; set; }

        private GameManager _gameManager;

        void Awake()
        {
            PlayerDeadSplash.SetActive(false);
        }

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void ShowSplash()
        {
            SplashScreen.Show();
        }

        public void ShowPlayerDeadSplash()
        {
            PlayerDeadSplash.SetActive(true);
            TweenAlpha.Begin(PlayerDeadSplash.gameObject, 0.5f, 1.0f);
        }

        public void HidePlayerDeadSplash()
        {
            StartCoroutine(DelayHidePlayerSplash());
        }

        private IEnumerator DelayHidePlayerSplash()
        {
            TweenAlpha.Begin(PlayerDeadSplash.gameObject, 0.5f, 0.0f);
            yield return new WaitForSeconds(0.5f);
            PlayerDeadSplash.SetActive(false);
        }
    }
}
