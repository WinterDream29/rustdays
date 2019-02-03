using System;
using System.Collections;
using Assets.Scripts.Ui;
using UnityEngine;

namespace Assets.Scripts.UI.Interactive
{
    public class BarrelPanel : View
    {
        public GameObject DrinkButton;
        public UISprite BarrelFilled;
        public GameObject CloseButton;

        public Action<int> OnDrinkAction { get; set; }

        private int _currentFill;
        private int _fillAmount;

        public void Init(GameManager gameManager, int fillAmount, int currentFill)
        {
            base.Init(gameManager);

            _currentFill = currentFill;
            _fillAmount = fillAmount;

            BarrelFilled.fillAmount = currentFill / (float)fillAmount;

            UIEventListener.Get(DrinkButton).onClick += OnDrinkClick;
            UIEventListener.Get(CloseButton).onClick += OnCloseClick;
        }

        public IEnumerator ShowDelay(float delayTime, int fillAmount, int currentFill)
        {
            IsShowing = true;
            yield return new WaitForSeconds(delayTime);
            gameObject.SetActive(true);

            _currentFill = currentFill;
            _fillAmount = fillAmount;

            UpdateView();
        }

        public override void UpdateView()
        {
            base.UpdateView();

            BarrelFilled.fillAmount = _currentFill / (float)_fillAmount;
        }

        private void OnDrinkClick(GameObject go)
        {
            if(_currentFill == 0)
                return;

            var drinkAmount = 0;
            drinkAmount = _currentFill >= 20 ? 20 : _currentFill;
            _currentFill -= drinkAmount;
            
            BarrelFilled.fillAmount = _currentFill / (float)_fillAmount;

            if (OnDrinkAction != null)
                OnDrinkAction(drinkAmount);

            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerDrink);
        }

        private void OnCloseClick(GameObject go)
        {
            Hide();
        }
    }
}
