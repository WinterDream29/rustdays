using UnityEngine;

namespace Assets.Scripts.UI
{
    public class AdditionalStatsConstroller : MonoBehaviour
    {
        public UIGrid GridItems;
        public GameObject BreathObject;
        public UISprite BreathProgress;

        private GameManager _gameManager;
        private bool _initialized;

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;

            BreathObject.SetActive(false);

            _initialized = true;
        }

        public void SetBreath(bool active)
        {
            BreathObject.SetActive(active);
            GridItems.Reposition();
        }

        public void UpdateStats()
        {
            if (!_initialized)
                return;

            if (BreathObject.activeSelf)
                BreathProgress.fillAmount = 1 - _gameManager.PlayerModel.Breath/100f;
        }
    }
}
