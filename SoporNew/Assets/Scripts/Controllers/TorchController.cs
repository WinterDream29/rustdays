using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class TorchController : MonoBehaviour
    {
        public Animation Animation;
        public GameObject TorchObject;
        public Light Light;
        public AudioSource Audio;
        public ParticleSystem FireParticles;

        public bool IsActive;
        private GameManager _gameManager;
        private UiSlot _torchSlot;
        private float _currentTime;
        private float _fireTime;
		private float _updateDurabilityTime = 0.0f;

        public void Start()
        {
            TorchObject.SetActive(false);
            Audio.Stop();
            IsActive = false;
            _currentTime = 0.0f;
        }

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Show(UiSlot slot)
        {
            IsActive = true;
            TorchObject.SetActive(true);
            _torchSlot = slot;
            Animation.Play("ShowTorch");
            Audio.Play();
            FireParticles.Play();
        }

        public void Hide()
        {
            IsActive = false;
            Audio.Stop();
            _torchSlot = null;
            Animation.Play("HideTorch");
        }

        public void OnHided()
        {
            TorchObject.SetActive(false);
        }

        void Update()
        {
            if (!IsActive || 
                _torchSlot == null || 
                _torchSlot.ItemModel == null ||
                _torchSlot.ItemModel.Item == null)
                return;

            _currentTime += Time.deltaTime;
			_updateDurabilityTime += Time.deltaTime;
            if (_currentTime >= _torchSlot.ItemModel.Item.Durability)
            {
                _currentTime = 0.0f;
                _gameManager.PlayerModel.Inventory.QuickSlots[_torchSlot.SlotId].ChangeAmount(1);
                if (_gameManager.PlayerModel.Inventory.QuickSlots[_torchSlot.SlotId] == null)
                    Hide();
            }
			if (_updateDurabilityTime >= 1.0f) 
			{
				_gameManager.Player.MainHud.InventoryPanel.QuickSlotsPanel.Slots[_torchSlot.SlotId].ItemModel.ChangeDurability(1);
				_updateDurabilityTime = 0.0f;

			}
        }
    }
}
