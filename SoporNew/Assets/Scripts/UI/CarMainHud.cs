using System;
using System.Collections;
using Assets.Scripts.Models.Food;
using Assets.Scripts.UI.Craft;
using Assets.Scripts.UI.Interactive;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CarMainHud : MonoBehaviour
    {
        public GameObject OutButton;
        public GameObject GasButton;
        public GameObject FuelingButton;
        public GameObject LigthButton;
        public CarInteractive CurrentCar;
        public UISprite TankFillSprite;

        private GameManager _gameManager;

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;

            OutButton.SetActive(false);
            UIEventListener.Get(OutButton).onClick += OnOutClick;
            UIEventListener.Get(GasButton).onClick += OnGasClick;
            UIEventListener.Get(FuelingButton).onClick += OnFuelingClick;
            UIEventListener.Get(LigthButton).onClick += OnLigthButtonClick;
        }

        private void OnLigthButtonClick(GameObject go)
        {
            _gameManager.CarInteractive.LigthObject.SetActive(!_gameManager.CarInteractive.LigthObject.activeSelf);
        }

        private void OnFuelingClick(GameObject go)
        {
            if (_gameManager.PlayerModel.Inventory.GetAmount(typeof(Jerrycan)) > 0)
            {
                _gameManager.PlayerModel.Inventory.UseItem(_gameManager, typeof(Jerrycan));
            }
            else
            {
                if (!_gameManager.Player.MainHud.CraftPanel.IsShowing)
                {
                    _gameManager.Player.MainHud.CraftPanel.Show();
                    _gameManager.Player.MainHud.CraftPanel.SelectCategory(CraftCategory.Shop);
                }
            }
        }

        private void OnGasClick(GameObject go)
        {
            if (CurrentCar.Petrol <= 0.0f)
            {
                _gameManager.Player.MainHud.ShowHudText(Localization.Get("no_fuel"), HudTextColor.Red);
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
            OutButton.SetActive(false);
            StartCoroutine(ShowOutButton());
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator ShowOutButton()
        {
            yield return new WaitForSeconds(2.0f);
            OutButton.SetActive(true);
        }

        private void OnOutClick(GameObject go)
        {
            CurrentCar.Out();
        }
    }
}
