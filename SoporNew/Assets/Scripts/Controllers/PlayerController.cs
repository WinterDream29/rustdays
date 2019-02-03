using System;
using System.Collections;
using Assets.Scripts.Controllers.InteractiveObjects;
using Assets.Scripts.Controllers.UsableObjects;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Ammo;
using Assets.Scripts.Models.Tools;
using Assets.Scripts.Models.Weapons;
using Assets.Scripts.UI;
using Assets.Scripts.Ui;
using UnityEngine;
using Assets.Scripts.Controllers.Fauna;
using Assets.Scripts.Controllers.InteractiveObjects.MiningObjects;
using Assets.Scripts.Models.Constructions;
using Assets.Scripts.Models.Constructions.Fence;
using Assets.Scripts.Models.Events;
using UnityEngine.Advertisements;

namespace Assets.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public Camera FpsCamera;
        public Camera WeaponCamera;
        public vp_FPPlayerEventHandler EventHandler;
        public vp_FPWeaponHandler WeaponHandler;
        public vp_SimpleInventory WeaponInventory;
        public vp_PlayerDamageHandler DamageHandler;
        public vp_FPController Controller;
        public vp_FootstepManager FootstepManager;
        public AudioListener AudioListener;

        public BoxCollider HandWeaponAttackCollider;
        public PlayerMainHud MainHud;
        public CarMainHud CarHud;
        public GameObject DropObjectSpawnPlace;
        public TorchController Torch;
        public AudioSource PlayerAudioSource;
        public Transform RespawnPoint;
        public Transform FishrodCheckWaterTransform;
        public Transform FishrodBobberParent;
        public Transform FishrodBobber;
        public GameObject Rain;
        public Transform RainParent;

        public bool InHouse { get; private set; }
        public bool InCar { get; set; }
        public Action<bool> OnEnterCar { get; set; }

        private GameManager _gameManager;
        public HolderObject _currentWeapon { get; private set; }
        private GameObject _currentGroundPlacementObject;
        private bool _attacked;
        private bool _attackButtonPressed;

        private UiSlot _prevSelectedSlot;

        private bool _initalized;
        private bool _torchActive;

        private BowController _bowController;
        private CrossbowController _crossbowController;
        private ShovelController _shovelController;
        private FishrodController _fishrodController;
        private View _sleepDialog;
        private bool _changingWeapon;
        private SimpleEvents _simpleEvents = new SimpleEvents();

        [HideInInspector]
        public WorldItem _nearWorldItem;

        void Start()
        {
            _torchActive = false;

            UIEventListener.Get(MainHud.AttackButton).onClick += OnAttackButtonClick;
            UIEventListener.Get(MainHud.AttackButton).onPress += OnAttackButtonPress;
            UIEventListener.Get(MainHud.GetItemButton).onClick += OnGetItemClick;
            UIEventListener.Get(MainHud.UseButton).onClick += OnUseButtonClick;
            UIEventListener.Get(MainHud.RemovePlaceItemButton).onClick += OnRemoveCurrentPlacedItem;
            UIEventListener.Get(MainHud.UpFoundationButton).onClick += OnUpFoundation;
            UIEventListener.Get(MainHud.DownFoundationButton).onClick += OnDownFoundation;
            _simpleEvents.Attach(Player.PLAYER_UNDER_WATER, OnUnderWater);
            _simpleEvents.Attach(Inventory.INVENTORY_ADD_ITEM, OnAddItemToInventory);
        }

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
            MainHud.Init(_gameManager);
            CarHud.Init(gameManager);
            Torch.Init(_gameManager);
            MainHud.OnQuickSlotClickAction += OnQuicklSlotClick;
            MainHud.OnInventorySlotClickAction += OnInventorySlotClick;
            MainHud.OnEquipSlotClickAction += OnEquipSlotClick;
            _gameManager.PlayerModel.OnDeathAction += OnDeath;
            _gameManager.PlayerModel.OnSleepAction += OnSleep;

            StartCoroutine(OneSecondUpdate());

            _initalized = true;
        }

        void Update()
        {
            if (_attackButtonPressed)
                MeleeAttack();

            UpdateGetItem();
        }

        private void OnUnderWater(object under)
        {
            MainHud.OnUnderWater(under);
            FootstepManager.enabled = !(bool)under;
        }

        private void RefreshPrevSelectedSlot()
        {
            _prevSelectedSlot = null;

            foreach (var slot in MainHud.InventoryPanel.Slots)
            {
                if (slot.IsSelected)
                {
                    _prevSelectedSlot = slot;
                    break;
                }
            }
            foreach (var slot in MainHud.InventoryPanel.QuickSlotsPanel.Slots)
            {
                if (slot.IsSelected)
                {
                    _prevSelectedSlot = slot;
                    break;
                }
            }
            foreach (var slot in MainHud.InventoryPanel.EquipPanel.Slots)
            {
                if (slot.IsSelected)
                {
                    _prevSelectedSlot = slot;
                    break;
                }
            }

            if (_gameManager.DisplayManager.CurrentInteractPanel != null && _gameManager.DisplayManager.CurrentInteractPanel is InteractView)
            {
                var panel = _gameManager.DisplayManager.CurrentInteractPanel as InteractView;
                foreach (var slot in panel.Slots)
                {
                    if (slot.IsSelected)
                    {
                        _prevSelectedSlot = slot;
                        break;
                    }
                }
            }
        }

        private void OnGetItemClick(GameObject go)
        {
            if (_nearWorldItem != null)
            {
                _nearWorldItem.Get(_gameManager);
                _nearWorldItem = null;
            }
        }

        public void EnterToCar(bool enter)
        {
            InCar = enter;
            if (OnEnterCar != null)
                OnEnterCar(enter);
        }

        private void OnAddItemToInventory(object o)
        {
            var item = o as HolderObject;
            if (item != null && item.Item is Ammo)
            {
                var weaponType = (item.Item as Ammo).WeaponType;
                if (weaponType != null)
                {
                    var weapon = BaseObjectFactory.GetItem(weaponType);
                    var curWeapon = false;
                    if (_currentWeapon != null)
                        curWeapon = _currentWeapon.Item.GetType() == weapon.GetType();
                    (weapon as Weapon).SetAmmo(_gameManager, curWeapon);
                }
            }
        }

        public void OnInventorySlotClick(UiSlot uiSlot)
        {
            RefreshPrevSelectedSlot();

            if (_prevSelectedSlot == uiSlot)
            {
                if (uiSlot.ItemModel.Item is ConsumableItem)
                {
                    if (uiSlot.ItemModel.Amount == 1)
                        uiSlot.SetActiveSlot(false);

                    UseItem(uiSlot.ItemModel);
                }
                else
                {
                    uiSlot.SetActiveSlot(false);
                }
            }
            else if (_prevSelectedSlot == null)
            {
                if(uiSlot.ItemModel != null)
                    uiSlot.SetActiveSlot(true);
            }
            else
            {
                if (uiSlot.ItemModel == null || uiSlot.ItemModel.Item == null)
                {
                    if (_prevSelectedSlot.ItemModel.Amount == 1)
                        _prevSelectedSlot.SetActiveSlot(false);

                    uiSlot.SetData(_gameManager, HolderObjectFactory.GetItem(_prevSelectedSlot.ItemModel.Item.GetType(), 1));
                    _prevSelectedSlot.ChangeAmount(1);
                }
                else if (uiSlot.ItemModel.Item.GetType() == _prevSelectedSlot.ItemModel.Item.GetType() 
                    && (uiSlot.ItemModel.Item.Durability == null || !uiSlot.ItemModel.Item.ShowDurability))
                {
                    if (_prevSelectedSlot.ItemModel.Amount == 1)
                        _prevSelectedSlot.SetActiveSlot(false);

                    _gameManager.PlayerModel.ChangeItemAmount(uiSlot, -1);
                    _gameManager.PlayerModel.ChangeItemAmount(_prevSelectedSlot, 1);
                }
                else
                {
                    var curModel = uiSlot.ItemModel;
                    uiSlot.SetData(_gameManager, _prevSelectedSlot.ItemModel);
                    _prevSelectedSlot.SetData(_gameManager, curModel);
                    _prevSelectedSlot.SetActiveSlot(false);
                }
            }
        }

        private void OnQuicklSlotClick(UiSlot uiSlot)
        {
            if (_changingWeapon)
                return;

            if (MainHud.InventoryPanel.IsShowing)
            {
                OnInventorySlotClick(uiSlot);
            }
            else
            {
                bool useItem = true;

                if (uiSlot.ItemModel != null)
                {
                    if (_fishrodController != null && !(uiSlot.ItemModel.Item is Fishrod))
                    {
                        _fishrodController.CurrentState = FishrodStates.Available;
                        FishrodBobberParent.gameObject.SetActive(false);
                    }

                    if (uiSlot.ItemModel.Item is Torch)
                    {
                        if (_torchActive)
                            Torch.Hide();
                        else
                        {
                            if (MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot != null &&
                                (MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot.ItemModel.Item is Bow || MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot.ItemModel.Item is Crossbow))
                            {
                                foreach (var slot in MainHud.InventoryPanel.QuickSlotsPanel.Slots)
                                        slot.SetActiveSlot(false);
                                MainHud.InventoryPanel.QuickSlotsPanel.UpdateView();
                                WeaponHandler.SetWeaponSmooth();
                            }
                            Torch.Show(uiSlot);
                        }

                        _torchActive = !_torchActive;
                    }
                    else if (uiSlot.ItemModel.Item is ConsumableItem)
                    {
                        foreach (var slot in MainHud.InventoryPanel.QuickSlotsPanel.Slots)
                            if (slot != uiSlot)
                                slot.SetActiveSlot(false);
                    }
                    else if (uiSlot.ItemModel.Item is UsableItem)
                    {
                        foreach (var slot in MainHud.InventoryPanel.QuickSlotsPanel.Slots)
                            if (slot.ItemModel != null && slot.ItemModel.Item is UsableItem)
                            {
                                if (slot == uiSlot)
                                {
                                    if (MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot != null && 
                                        MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot == slot)
                                    {
                                        useItem = false;
                                        MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot = null;
                                        slot.SelectUsable(false);
                                        SetEmptyHand();
                                    }
                                    else
                                    {
                                        slot.SelectUsable(true);
                                        MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot = uiSlot;
                                    }
                                }
                                else
                                {
                                    slot.SelectUsable(false);
                                }
                            }
                    }
                    else if (uiSlot.ItemModel.Item is IPlacement)
                    {
                        if (_currentWeapon != null)
                            SetEmptyHand();

                        if (_currentGroundPlacementObject != null &&
                            _currentGroundPlacementObject.GetComponent<GroundPlacementItemController>().Slot == uiSlot)
                        {
                            useItem = false;
                        }
                        else
                        {
                            MainHud.InventoryPanel.QuickSlotsPanel.CurrentPlacementSlot = uiSlot;
                            uiSlot.SelectUsable(true);

                            var placementItem = uiSlot.ItemModel.Item as IPlacement;
                            _currentGroundPlacementObject = Instantiate(Resources.Load<GameObject>(placementItem.PrefabTemplatePath));
                            var initScale = _currentGroundPlacementObject.transform.localScale;
                            _currentGroundPlacementObject.transform.parent = DropObjectSpawnPlace.transform;
                            _currentGroundPlacementObject.transform.localPosition = Vector3.zero;
                            _currentGroundPlacementObject.transform.localRotation = Quaternion.identity;
                            _currentGroundPlacementObject.transform.localScale = initScale;
                            _currentGroundPlacementObject.GetComponent<GroundPlacementItemController>().Init(_gameManager, uiSlot);

                            MainHud.InventoryPanel.QuickSlotsPanel.Hide();
                            MainHud.RemovePlaceItemButton.SetActive(true);
                            _gameManager.PlacementItemsController.SetActiveConstructionColliders(true);
                            if (uiSlot.ItemModel.Item is Construction)
                            {
                                var constItem = uiSlot.ItemModel.Item as Construction;
                                if(constItem.ConstructionType == ConstructionType.Foundation)
                                {
                                    MainHud.UpFoundationButton.SetActive(true);
                                    MainHud.DownFoundationButton.SetActive(true);
                                }
                            }
                         
                            MainHud.AttackButtonIcon.spriteName = WorldConsts.BuildIconName;
                        }
                    }
                }

                if(useItem)
                    UseItem(uiSlot.ItemModel);
            }
        }

        private void OnEquipSlotClick(UiSlot uiSlot)
        {
            RefreshPrevSelectedSlot();
            if (_prevSelectedSlot != null && !uiSlot.CanSetEquip(_prevSelectedSlot))
            {
                _prevSelectedSlot.SetActiveSlot(false);
            }
            else
            {
                OnInventorySlotClick(uiSlot);
            }
        }

        private void OnAttackButtonClick(GameObject go)
        {
            Attack();
        }

        private void OnAttackButtonPress(GameObject go, bool state)
        {
            _attackButtonPressed = false;

            if (_currentWeapon != null)
            {
                if (_currentWeapon.Item is Bow)
                {
                    if (_bowController == null)
                    {
                        _bowController = WeaponHandler.CurrentWeapon.GetComponentInChildren<BowController>();
                        _bowController.Init(_gameManager);
                    }

                    bool hasArrow = _gameManager.PlayerModel.Inventory.GetAmount(typeof (Arrow)) > 0;

                    if (hasArrow)
                    {
                        if (state)
                        {
                            _bowController.TetevenPull();
                        }
                        else if (_bowController.CurrentState == BowState.Ready)
                        {
                            _bowController.Shoot(FpsCamera);
                            _gameManager.PlayerModel.Inventory.UseItem(_gameManager, (typeof (Arrow)));
                        }
                        else
                        {
                            _bowController.TetevenWeaken();
                        }
                    }
                }
                else if (_currentWeapon.Item is Shovel)
                {
                    _shovelController = WeaponHandler.CurrentWeapon.GetComponentInChildren<ShovelController>();
                    _attackButtonPressed = state;
                }
                else if (_currentWeapon.Item is Fishrod)
                {
                    _fishrodController = WeaponHandler.CurrentWeapon.GetComponentInChildren<FishrodController>();
                    _attackButtonPressed = state;
                }
                else if ((_currentWeapon.Item as Weapon).WeaponType == WeaponType.Melee)
                {
                    _attackButtonPressed = state;
                }
            }
        }

        private void OnWeaponFire(object o)
        {
            var weapon = _currentWeapon.Item as Weapon;
            if(weapon != null && weapon.AmmoType != null)
                _gameManager.PlayerModel.Inventory.UseItem(_gameManager, weapon.AmmoType);
        }

        private void OnUseButtonClick(GameObject go)
        {
            Use();
        }

        private void Use()
        {
            if (_gameManager.DisplayManager.CurrentInteractPanel != null)
                return;

            var hitRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hitInfo;
            int waterLayerMask = 1 << 4;//берем слой воды
            waterLayerMask = ~waterLayerMask;//инвентируем, теперь в переменной все слои крое воды
            if (Physics.Raycast(hitRay, out hitInfo, 5.0f, waterLayerMask))
            {
                if (hitInfo.collider != null)
                {
                    var usableObject = hitInfo.collider.gameObject.GetComponent<UsableObject>();
                    var door = hitInfo.collider.GetComponent<Assets.Scripts.Controllers.Constructions.DoorController>();

                    if (usableObject != null)
                    {
                        usableObject.Use(_gameManager);
                    }
                    else if(door != null)
                    {
                        door.OpenDoor();
                    }
                    else
                    {
                        var worldItem = hitInfo.collider.gameObject.GetComponent<WorldItem>();
                        if (worldItem != null)
                            worldItem.Get(_gameManager);
                    }
                }
            }
        }

        private void Attack()
        {
            if (_currentGroundPlacementObject != null)
            {
                var placementController = _currentGroundPlacementObject.GetComponent<GroundPlacementItemController>();
                if (placementController.CanPlace)
                {
                    var holderObject = MainHud.InventoryPanel.QuickSlotsPanel.CurrentPlacementSlot.ItemModel;
                    var playBuildSound = holderObject.Item is Construction || holderObject.Item is WoodenFence;

                    var item = holderObject.Item as IPlacement;
                    var itemPrefab = Resources.Load<GameObject>(item.PrefabPath);
                    var itemGo = Instantiate(itemPrefab);

                    itemGo.transform.position = _currentGroundPlacementObject.transform.position;
                    itemGo.transform.rotation = _currentGroundPlacementObject.transform.rotation;

                    var destroyable = itemGo.GetComponentInChildren<DestroyableObject>();
                    if (destroyable != null)
                        destroyable.ItemModel = holderObject.Item;

                    _gameManager.PlacementItemsController.AddPlacedItem(itemGo, holderObject.Item);

                    MainHud.InventoryPanel.QuickSlotsPanel.CurrentPlacementSlot.ItemModel.ChangeAmount(1);

                    if (!MainHud.LookPadCollider.enabled)
                        MainHud.LookPadCollider.enabled = true;

                    if (MainHud.InventoryPanel.QuickSlotsPanel.CurrentPlacementSlot.ItemModel == null)
                        RemoveCurrentPlacedItem();

                    if (playBuildSound)
                        SoundManager.PlaySFX(WorldConsts.AudioConsts.Build);
                }
                else if (!string.IsNullOrEmpty(placementController.CantPlaceLocalizationKey))
                {
                    MainHud.ShowHudText(Localization.Get(placementController.CantPlaceLocalizationKey), HudTextColor.Red);
                }
            }
            else if (_currentWeapon != null)
            {
                if (!(_currentWeapon.Item is Bow))
                {
                    switch ((_currentWeapon.Item as Weapon).WeaponType)
                    {
                        case WeaponType.Melee:
                            MeleeAttack();
                            break;
                        case WeaponType.Fire:
                            FireWeaponAttack();
                            break;
                    }
                }
            }
        }

        private void MeleeAttack()
        {
            if (_attacked)
                return;

            if (_currentWeapon == null)
                return;

            if (_currentWeapon.Item is Shovel)
                StartCoroutine(ShovelUse());
            else if (_currentWeapon.Item is Fishrod)
                UseFishRod();
            else
                StartCoroutine(MeleeAttackCor());
        }

        private void UseFishRod()
        {
            if (_fishrodController == null)
                return;

            _fishrodController.Use();
        }


        private IEnumerator ShovelUse()
        {
            _attacked = true;

            yield return new WaitForSeconds(_currentWeapon.Item.AttackFirstHalfTime);

            _shovelController.Mine(_gameManager);

            MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot.ItemModel.ChangeDurability(1);
            if (MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot.ItemModel == null)
            {
                SetEmptyHand();
                _shovelController = null;
                SoundManager.PlaySFX(WorldConsts.AudioConsts.BreakWeapon);
            }

            if (_currentWeapon != null)
                yield return new WaitForSeconds(_currentWeapon.Item.AttackSeconfHalfTime);
            _attacked = false;
        }

        private IEnumerator MeleeAttackCor()
        {
            _attacked = true;

            yield return new WaitForSeconds(_currentWeapon.Item.AttackFirstHalfTime);

            //var hitRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            //RaycastHit hitInfo;
            //int waterLayerMask = 1 << 4;//берем слой воды
            //waterLayerMask = ~waterLayerMask;//инвентируем, теперь в переменной все слои крое воды
            //if (Physics.Raycast(hitRay, out hitInfo, 3.0f, waterLayerMask))
            //{
            //    if (hitInfo.collider != null) //&& hitInfo.collider.bounds.Intersects(HandWeaponAttackCollider.bounds))
            //    {
            //        //Debug.LogError(hitInfo.collider.gameObject.name);
            //        //var intractiveObject = hitInfo.collider.gameObject.GetComponent<InteractiveObject>();
            //        //if (intractiveObject != null)
            //        //{
            //        //    intractiveObject.PlayerInteract(_currentWeapon, _gameManager, hitInfo.point);
            //        //    MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot.ItemModel.ChangeDurability(1);
            //        //    if (MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot.ItemModel == null)
            //        //    {
            //        //        SetEmptyHand();
            //        //        SoundManager.PlaySFX(WorldConsts.AudioConsts.BreakWeapon);
            //        //    }
            //        //}
            //        //else
            //        //{
            //        //    var animalLink = hitInfo.collider.gameObject.GetComponent<AnimalColliderLink>();
            //        //    if (animalLink != null)
            //        //    {
            //        //        animalLink.SetDamage(_currentWeapon.Item.Damage, hitInfo.point);
            //        //        MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot.ItemModel.ChangeDurability(1);
            //        //        SoundManager.PlaySFX(WorldConsts.AudioConsts.DamageEnemy);
            //        //    }
            //        //}
            //    }
            //}

            if(_currentWeapon != null)
                yield return new WaitForSeconds(_currentWeapon.Item.AttackSeconfHalfTime);
            _attacked = false;
        }

        public void FireWeaponAttack()
        {
            if (_currentWeapon.Item is Crossbow)
            {
                if (_crossbowController == null)
                {
                    _crossbowController = WeaponHandler.CurrentWeapon.GetComponentInChildren<CrossbowController>();
                    _crossbowController.Init(_gameManager);
                }

                _crossbowController.Shoot();
            }
        }

        public void UseItem(HolderObject holderObject)
        {
            if (holderObject == null || holderObject.Item == null)
                return;

            if (holderObject.Item is Weapon)
            {
                _currentWeapon = holderObject;
                StartCoroutine(WaitChangeWeapon(_currentWeapon.Item as Weapon));
            }

            holderObject.Use(_gameManager);
        }

        private IEnumerator WaitChangeWeapon(Weapon weapon)
        {
            _changingWeapon = true;
            yield return new WaitForSeconds(weapon.ChangeWeaponTime);
            _changingWeapon = false;

        }

        public void SetEmptyHand()
        {
            if (_currentWeapon != null)
                if (MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot != null)
                {
                    MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot.SelectUsable(false);
                    MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot = null;
                }

            _currentWeapon = null;
            WeaponHandler.SetWeaponSmooth(0);
        }

        private void OnRemoveCurrentPlacedItem(GameObject go)
        {
            RemoveCurrentPlacedItem();
        }

        private void OnUpFoundation(GameObject go)
        {
            if (_currentGroundPlacementObject != null)
                _currentGroundPlacementObject.GetComponent<GroundPlacementItemController>().Up();
        }

        private void OnDownFoundation(GameObject go)
        {
            if (_currentGroundPlacementObject != null)
                _currentGroundPlacementObject.GetComponent<GroundPlacementItemController>().Down();
        }

        private void RemoveCurrentPlacedItem()
        {
            Destroy(_currentGroundPlacementObject);
            MainHud.RemovePlaceItemButton.SetActive(false);
            _gameManager.PlacementItemsController.SetActiveConstructionColliders(false);
            MainHud.UpFoundationButton.SetActive(false);
            MainHud.DownFoundationButton.SetActive(false);
            MainHud.AttackButtonIcon.spriteName = WorldConsts.AttackIconName;
            MainHud.InventoryPanel.QuickSlotsPanel.Show();
            if(MainHud.InventoryPanel.QuickSlotsPanel.CurrentPlacementSlot != null)
            {
                MainHud.InventoryPanel.QuickSlotsPanel.CurrentPlacementSlot.SelectUsable(false);
                MainHud.InventoryPanel.QuickSlotsPanel.CurrentPlacementSlot = null;
            }
        }

        private void OnDeath()
        {
            StartCoroutine(RestartGame());
        }

        private void OnSleep()
        {
            if (_sleepDialog == null)
            {
                _sleepDialog = NGUITools.AddChild(_gameManager.UiRoot.gameObject, _gameManager.DisplayManager.SleepDialog.gameObject).GetComponent<View>();
                _sleepDialog.Init(_gameManager);
            }
            if(!_sleepDialog.IsShowing)
                _sleepDialog.Show();
        }

        private IEnumerator RestartGame()
        {
            if(InCar)
                _gameManager.CarInteractive.Out();

            //var player = gameObject.GetComponent<vp_FPPlayerEventHandler>();
            EventHandler.SetState("Dead");

            if (MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot != null)
            {
                MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot.ItemModel.Remove();
                MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot.SelectUsable(false);
            }

            MainHud.InventoryPanel.QuickSlotsPanel.CurrentSlot = null;
            MainHud.InventoryPanel.QuickSlotsPanel.UpdateView();
            SetEmptyHand();

            _gameManager.Player.MainHud.SetActiveControls(false);
            _gameManager.Player.MainHud.SetActiveButtons(false);
            MainHud.InventoryPanel.QuickSlotsPanel.Hide();
           
            yield return new WaitForSeconds(2.0f);

            _gameManager.DisplayManager.ShowPlayerDeadSplash();

            yield return new WaitForSeconds(2.0f);

            if (!_gameManager.IapManager.IsBuyNoAds && Advertisement.IsReady("video"))
            {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show("video", options);
            }
            else
            {
                StartAfterDead();
            }
        }

        private void StartAfterDead()
        {
            //player = gameObject.GetComponent<vp_FPPlayerEventHandler>();
            EventHandler.SetState("Dead", false);

            transform.position = RespawnPoint.position;
            _gameManager.PlayerModel.ChangeHealth(30.0f);
            if (_gameManager.PlayerModel.Hunger < 5.0f)
                _gameManager.PlayerModel.Hunger = 25.0f;
            if (_gameManager.PlayerModel.Thirst < 5.0f)
                _gameManager.PlayerModel.Thirst = 25.0f;

            _gameManager.Player.MainHud.SetActiveControls(true);
            _gameManager.Player.MainHud.SetActiveButtons(true);
            MainHud.InventoryPanel.QuickSlotsPanel.Show();

            _gameManager.PlayerModel.Dead = false;
            _gameManager.DisplayManager.HidePlayerDeadSplash();
        }

        private void HandleShowResult(ShowResult result)
        {
            StartAfterDead();
        }

        private IEnumerator OneSecondUpdate()
        {
            while(true)
            {
                yield return new WaitForSeconds(1.0f);

                if (_initalized)
                {
                    _gameManager.PlayerModel.UpdateStates();

                    if (!InCar)
                    {
                        RaycastHit hit;
                        Physics.Raycast(transform.position, -Vector3.up, out hit, 10);
                        if (hit.collider != null && hit.collider.gameObject != null)
                        {
                            InHouse = hit.collider.gameObject.tag == "HomeFloor";

                            if (hit.collider.gameObject.name == "Terrain1" && _gameManager.CurrentTerain != _gameManager.Terrain1)
                                _gameManager.CurrentTerain = _gameManager.Terrain1;

                            if (hit.collider.gameObject.name == "Terrain2" && _gameManager.CurrentTerain != _gameManager.Terrain2)
                                _gameManager.CurrentTerain = _gameManager.Terrain2;
                        }
                    }
                }
            }
        }

        public void UpdateNearWorldItem(float sqrDistance, WorldItem wItem)
        {
            if (_nearWorldItem == null)
            {
                if (sqrDistance < 50.0f)
                    _nearWorldItem = wItem;
            }
        }

        public void UpdateGetItem()
        {
            if (_nearWorldItem != null && _nearWorldItem.SqrDistToPlayer > 50.0f)
                _nearWorldItem = null;
        }

        void OnDestroy()
        {
            _simpleEvents.Detach(Player.PLAYER_UNDER_WATER, OnUnderWater);
            _simpleEvents.Detach(Inventory.INVENTORY_ADD_ITEM, OnAddItemToInventory);
            _simpleEvents = null;
        }
    }
}