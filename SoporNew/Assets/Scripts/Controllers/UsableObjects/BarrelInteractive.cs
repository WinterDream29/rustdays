using Assets.Scripts.UI.Interactive;
using UnityEngine;

namespace Assets.Scripts.Controllers.UsableObjects
{
    public class BarrelInteractive : UsableObject, IFilled
    {
        public int FillAmount = 100;
        public float FillTime = 1;
        public Transform Water;
        public int CurrentFilledAmount { get; set; }

        private GameManager _gameMananger;
        private float _currentTime;
        private BarrelPanel _panel;

        protected override void Init()
        {
            base.Init();

            var gmGo = GameObject.FindWithTag("GameManager");
            if (gmGo != null)
                GameManager = gmGo.GetComponent<GameManager>();

            SetWaterPosition();
        }

        private void SetWaterPosition()
        {
            var waterPos = CurrentFilledAmount / (float) FillAmount;
            waterPos *= 1.45f;
            waterPos += 0.25f;
            if (waterPos > 1.65f)
                waterPos = 1.65f;
            Water.localPosition = new Vector3(Water.localPosition.x, waterPos, Water.localPosition.z);
            Water.gameObject.SetActive(waterPos > 0.25f);
        }

        public override void Use(GameManager gameManager)
        {
            if (gameManager.DisplayManager.CurrentInteractPanel != null)
                return;

            base.Use(gameManager);

            if (_panel == null)
            {
                _panel = NGUITools.AddChild(gameManager.UiRoot.gameObject, InteractPanelPrefab).GetComponent<BarrelPanel>();
                _panel.Init(GameManager, FillAmount, CurrentFilledAmount);
                _panel.OnDrinkAction += OnDrink;
                _panel.Hide();
            }

            if (!_panel.IsShowing)
                StartCoroutine(_panel.ShowDelay(0.2f, FillAmount, CurrentFilledAmount));
        }

        private void OnDrink(int amount)
        {
            GameManager.PlayerModel.ChangeThirst(amount);
            CurrentFilledAmount -= amount;
            SetWaterPosition();
        }

        void Update()
        {
            if (GameManager != null)
            {
                if (GameManager.World.IsRain)
                {
                    _currentTime += Time.deltaTime;
                    if (_currentTime >= FillTime)
                    {
                        _currentTime = 0.0f;

                        RaycastHit hit;
                        int waterLayerMask = 1 << 10;
                        waterLayerMask = ~waterLayerMask;
                        Physics.Raycast(transform.position, Vector3.up, out hit, 30, waterLayerMask);
                        if (hit.collider == null)
                        {
                            CurrentFilledAmount += 1;
                            if (CurrentFilledAmount > FillAmount)
                                CurrentFilledAmount = FillAmount;

                            SetWaterPosition();
                        }
                    }
                }
            }
        }
    }
}
