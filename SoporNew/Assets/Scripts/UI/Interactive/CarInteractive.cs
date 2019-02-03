using System.Collections;
using Assets.Scripts.Controllers.UsableObjects;
using UnityEngine;

namespace Assets.Scripts.UI.Interactive
{
    public class CarInteractive : UsableObject
    {
        public RCC_CarControllerV3 CarController;
        public GameObject CarCameraObject;
        public Transform PlayerParent;
        public Transform PlayerOutPosition;
        public Transform CarDefaultPosition;
        public GameObject LigthObject;
        public Transform RainParent;

        public float Petrol { get; set; }

        private Coroutine UpdateTankCor;

        public override void Use(GameManager gameManager)
        {
            base.Use(gameManager);

            if (_playerDistance > ShowUiDistance)
                return;

            if(CarController.engineRunning || GameManager.Player.InCar || GameManager.PlayerModel.UnderWater)
                return;

            if (gameManager.DisplayManager.CurrentInteractPanel != null)
                return;


            GameManager.Player.FpsCamera.gameObject.SetActive(false);
            GameManager.Player.WeaponCamera.gameObject.SetActive(false);
            GameManager.Player.AudioListener.enabled = false;
            GameManager.Player.MainHud.InventoryPanel.QuickSlotsPanel.gameObject.SetActive(false);
            GameManager.Player.MainHud.RightButtonsPlacer.SetActive(false);
            GameManager.Player.MainHud.LeftButtonsPlacer.SetActive(false);
            GameManager.Player.Controller.enabled = false;
            GameManager.Player.transform.position = PlayerParent.position;

            GameManager.Player.CarHud.Show();
            GameManager.Player.EnterToCar(true);
            CarCameraObject.SetActive(true);
            CarController.canControl = true;

            var rainTransform = GameManager.Player.Rain.transform;
            rainTransform.parent = RainParent;
            rainTransform.localScale = Vector3.one;
            rainTransform.localPosition = Vector3.zero;
            rainTransform.localRotation = Quaternion.identity;

            CarController.KillOrStartEngine();

            UpdateTankCor = StartCoroutine(UpdateTankFill());
        }

        private IEnumerator UpdateTankFill()
        {
            while (true)
            {
                Petrol -= 0.001f;
                if (Petrol <= 0)
                    Petrol = 0.0f;

                CarController.fuelInput = Petrol;
                GameManager.Player.CarHud.TankFillSprite.fillAmount = Petrol;

                UpdateCurrentTerrain();

                yield return new WaitForSeconds(1.0f);
            }
        }

        private void UpdateCurrentTerrain()
        {
            if(!GameManager.Player.InCar)
                return;

            RaycastHit hit;
            Physics.Raycast(transform.position, -Vector3.up, out hit, 10);
            if (hit.collider != null && hit.collider.gameObject != null)
            {
                if (hit.collider.gameObject.name == "Terrain1" && GameManager.CurrentTerain != GameManager.Terrain1)
                    GameManager.CurrentTerain = GameManager.Terrain1;

                if (hit.collider.gameObject.name == "Terrain2" && GameManager.CurrentTerain != GameManager.Terrain2)
                    GameManager.CurrentTerain = GameManager.Terrain2;
            }
        }

        public void Out()
        {
            GameManager.Player.EnterToCar(false);
            CarCameraObject.SetActive(false);
            CarController.canControl = false;
            GameManager.Player.CarHud.Hide();

            //var posY = Terrain.activeTerrain.SampleHeight(PlayerOutPosition.position) + 2.0f;
            var posY = GameManager.CurrentTerain.SampleHeight(PlayerOutPosition.position) + 2.0f;
            GameManager.Player.transform.position = new Vector3(PlayerOutPosition.position.x, posY, PlayerOutPosition.position.z);
            GameManager.Player.FpsCamera.gameObject.SetActive(true);
            GameManager.Player.WeaponCamera.gameObject.SetActive(true);
            GameManager.Player.AudioListener.enabled = true;
            GameManager.Player.Controller.enabled = true;
            GameManager.Player.MainHud.InventoryPanel.QuickSlotsPanel.gameObject.SetActive(true);
            GameManager.Player.MainHud.RightButtonsPlacer.SetActive(true);
            GameManager.Player.MainHud.LeftButtonsPlacer.SetActive(true);

            var rainTransform = GameManager.Player.Rain.transform;
            rainTransform.parent = GameManager.Player.RainParent;
            rainTransform.localScale = Vector3.one;
            rainTransform.localPosition = Vector3.zero;
            rainTransform.localRotation = Quaternion.identity;

            CarController.KillOrStartEngine();

            if (UpdateTankCor != null)
            {
                StopCoroutine(UpdateTankCor);
                UpdateTankCor = null;
            }
        }

        public void AddFuel(float amount)
        {
            Petrol += amount;
            if (Petrol >= 1.0f)
                Petrol = 1.0f;

            CarController.fuelInput = Petrol;
        }

        public void MoveToDefaultPosition()
        {
            transform.position = CarDefaultPosition.position;
            transform.rotation = CarDefaultPosition.rotation;
        }

        void OnDestroy()
        {
            if (UpdateTankCor != null)
            {
                StopCoroutine(UpdateTankCor);
                UpdateTankCor = null;
            }
        }
    }
}
