using System;
using System.Collections.Generic;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Ammo;
using Assets.Scripts.Models.Clothes.Backpack;
using Assets.Scripts.Models.Common;
using Assets.Scripts.Models.Constructions.Stone;
using Assets.Scripts.Models.Constructions.Wood;
using Assets.Scripts.Models.Meds;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.Tools;
using Assets.Scripts.Models.Weapons;
using Assets.Scripts.Ui;
using Assets.Scripts.UI.Shop;
using UnityEngine;
using Assets.Scripts.Models.Clothes;
using Assets.Scripts.Models.Constructions.Brick;
using Assets.Scripts.Models.Constructions.Fence;
using Assets.Scripts.Models.Constructions.Glass;
using Assets.Scripts.Models.Decor;
using Assets.Scripts.UI.ShopNew;
#if FACEBOOK
using Facebook.Unity;
#endif

namespace Assets.Scripts.UI.Craft
{
    public class CraftView : View
    {
        public ShopView ShopView;
        public List<CraftTab> Tabs;
        public GameObject CraftItemPrefab;
        public UIGrid ItemsGrid;
        public UIScrollView ItemsScroll;
        public GameObject ItemsMainObject;
        public CraftSelectedItemView SelectedItemView;
        public BoxCollider CloseCollider;
        public GameObject AddFriendsButton;

        private Dictionary<Type, CraftCategory> _itemModels = new Dictionary<Type, CraftCategory>();

        private List<CraftItem> _items = new List<CraftItem>();

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            //ShopView.Init(GameManager);
            SelectedItemView.Init(GameManager);
            SelectedItemView.OnBiggestBackpack += BiggestBackpack;
            SelectedItemView.NoManyAction += NoMany;
            SelectedItemView.Hide();
            AddItems();
            SelectCategory(CraftCategory.Common);

            foreach (var craftTab in Tabs)
            {
                UIEventListener.Get(craftTab.gameObject).onClick += OnTabClick;
            }

            UIEventListener.Get(CloseCollider.gameObject).onClick += go => Hide();
            UIEventListener.Get(AddFriendsButton).onClick += OnAddFriendsClick;
        }

        private void OnAddFriendsClick(GameObject go)
        {
#if FACEBOOK
            if (FB.IsLoggedIn)
                FbManager.InviteFriendsRequest();
            else
                FbManager.Login();
#endif
        }

        private void NoMany()
        {
            SelectedItemView.Hide();
            SelectCategory(CraftCategory.Shop, true, NewShopCategory.Gold);
        }

        private void AddItems()
        {
            _itemModels[typeof(CampFire)] = CraftCategory.Common;
            _itemModels[typeof(Furnace)] = CraftCategory.Common;
            _itemModels[typeof(Barrel)] = CraftCategory.Common;
            _itemModels[typeof(InventoryBox)] = CraftCategory.Common;
            _itemModels[typeof(Chest)] = CraftCategory.Common;
            _itemModels[typeof(Tent)] = CraftCategory.Common;
            _itemModels[typeof(Bed)] = CraftCategory.Common;
            _itemModels[typeof(Rope)] = CraftCategory.Common;
            _itemModels[typeof(Gunpowder)] = CraftCategory.Common;
            _itemModels[typeof(GardenBed)] = CraftCategory.Common;
            _itemModels[typeof(GardenBedStone)] = CraftCategory.Common;
            _itemModels[typeof(GardenBedBrick)] = CraftCategory.Common;
            _itemModels[typeof(TorchOnWall)] = CraftCategory.Common;

            //itemModels[typeof(SmallStoneHatchet)] = CraftCategory.Weapons;
            _itemModels[typeof(Torch)] = CraftCategory.Weapons;
            _itemModels[typeof(StoneHatchet)] = CraftCategory.Weapons;
            _itemModels[typeof(StonePick)] = CraftCategory.Weapons;
            //itemModels[typeof(StoneKnife)] = CraftCategory.Weapons;
            //itemModels[typeof(Knife)] = CraftCategory.Weapons;
            _itemModels[typeof(Machete)] = CraftCategory.Weapons;
            _itemModels[typeof(MetalHatchet)] = CraftCategory.Weapons;
            _itemModels[typeof(MetalPick)] = CraftCategory.Weapons;
            //itemModels[typeof(Mace)] = CraftCategory.Weapons;
            _itemModels[typeof(Catana)] = CraftCategory.Weapons;
            _itemModels[typeof(Crowbar)] = CraftCategory.Weapons;
            _itemModels[typeof(Bow)] = CraftCategory.Weapons;
            _itemModels[typeof(Arrow)] = CraftCategory.Weapons;
            _itemModels[typeof(Crossbow)] = CraftCategory.Weapons;
            _itemModels[typeof(CrossbowArrow)] = CraftCategory.Weapons;
            _itemModels[typeof(Pistol)] = CraftCategory.Weapons;
            _itemModels[typeof(PistolBullet)] = CraftCategory.Weapons;
            _itemModels[typeof(Revolver)] = CraftCategory.Weapons;
            _itemModels[typeof(RevolverBullet)] = CraftCategory.Weapons;
            _itemModels[typeof(Machinegun)] = CraftCategory.Weapons;
            _itemModels[typeof(MachinegunAmmo)] = CraftCategory.Weapons;
            _itemModels[typeof(Shotgun)] = CraftCategory.Weapons;
            _itemModels[typeof(ShotgunAmmo)] = CraftCategory.Weapons;
            _itemModels[typeof(DragunobSniperRifle)] = CraftCategory.Weapons;
            _itemModels[typeof(DragunovAmmo)] = CraftCategory.Weapons;
            _itemModels[typeof(Shovel)] = CraftCategory.Weapons;
            _itemModels[typeof(Fishrod)] = CraftCategory.Weapons;

            _itemModels[typeof(Sofa)] = CraftCategory.Tools;
            _itemModels[typeof (Chair)] = CraftCategory.Tools;
            _itemModels[typeof(CoffeeTable)] = CraftCategory.Tools;
            _itemModels[typeof(KitchenTable)] = CraftCategory.Tools;
            _itemModels[typeof(Cupboard_Closed)] = CraftCategory.Tools;
            _itemModels[typeof(CupboardRoom)] = CraftCategory.Tools;
            _itemModels[typeof(BearRug)] = CraftCategory.Tools;
            _itemModels[typeof(SheepRug)] = CraftCategory.Tools;

            var backpack = GetNextBackpack();
            if (backpack != null)
                _itemModels[backpack] = CraftCategory.Clothes;
            _itemModels[typeof(LeatherCap)] = CraftCategory.Clothes;
            _itemModels[typeof(LeatherShirt)] = CraftCategory.Clothes;
            _itemModels[typeof(LeatherPants)] = CraftCategory.Clothes;
            _itemModels[typeof(LeatherBoots)] = CraftCategory.Clothes;
            _itemModels[typeof(FurCap)] = CraftCategory.Clothes;
            _itemModels[typeof(FurShirt)] = CraftCategory.Clothes;
            _itemModels[typeof(FurPants)] = CraftCategory.Clothes;
            _itemModels[typeof(FurBoots)] = CraftCategory.Clothes;

            _itemModels[typeof(Bandage)] = CraftCategory.Meds;
            _itemModels[typeof(Medkit)] = CraftCategory.Meds;
            _itemModels[typeof(AntiRadiationPills)] = CraftCategory.Meds;

            _itemModels[typeof(WoodFoundation)] = CraftCategory.Constructions;
            _itemModels[typeof(WoodWall)] = CraftCategory.Constructions;
            _itemModels[typeof(WoodWallDoor)] = CraftCategory.Constructions;
            _itemModels[typeof(WoodWallWindow)] = CraftCategory.Constructions;
            _itemModels[typeof(WoodCeiling)] = CraftCategory.Constructions;
            _itemModels[typeof(WoodStairs)] = CraftCategory.Constructions;
            _itemModels[typeof(WoodStreetStairs)] = CraftCategory.Constructions;
            _itemModels[typeof(StoneFoundation)] = CraftCategory.Constructions;
            _itemModels[typeof(StoneWall)] = CraftCategory.Constructions;
            _itemModels[typeof(StoneWallDoor)] = CraftCategory.Constructions;
            _itemModels[typeof(StoneWallWindow)] = CraftCategory.Constructions;
            _itemModels[typeof(StoneCeiling)] = CraftCategory.Constructions;
            _itemModels[typeof(StoneStairs)] = CraftCategory.Constructions;
            _itemModels[typeof(BrickFoundation)] = CraftCategory.Constructions;
            _itemModels[typeof(BrickWall)] = CraftCategory.Constructions;
            _itemModels[typeof(BrickWallDoor)] = CraftCategory.Constructions;
            _itemModels[typeof(BrickWallWindow)] = CraftCategory.Constructions;
            _itemModels[typeof(BrickCeiling)] = CraftCategory.Constructions;
            _itemModels[typeof(BrickStairs)] = CraftCategory.Constructions;
            _itemModels[typeof(GlassWall)] = CraftCategory.Constructions;
            _itemModels[typeof(GlassCeiling)] = CraftCategory.Constructions;
            _itemModels[typeof(WoodenFence)] = CraftCategory.Constructions;

            foreach (var craftCategory in _itemModels)
            {
                var craftItem = NGUITools.AddChild(ItemsGrid.gameObject, CraftItemPrefab).GetComponent<CraftItem>();
                var amount = GameManager.PlayerModel.Inventory.GetAmount(craftCategory.Key);
                craftItem.Init(GameManager, craftCategory.Value, craftCategory.Key, amount.ToString());
                craftItem.OnItemClickAction += OnItemClick;
                _items.Add(craftItem);
            }

            ItemsGrid.Reposition();
        }

        private void BiggestBackpack(CraftSelectedItemView selectedItem)
        {
            var backpack = GetNextBackpack();
            KeyValuePair<Type, CraftCategory> backPackItem = new KeyValuePair<Type,CraftCategory>();
            foreach (var item in _itemModels)
            {
                if (item.Key == typeof(SmallBackpack) || item.Key == typeof(BigBackpack))
                {
                    backPackItem = item;
                }
            }

            backPackItem = new KeyValuePair<Type, CraftCategory>(backpack, CraftCategory.Clothes);
            CraftItem cItem = null;
            foreach(var craftItem in _items)
            {
                if (craftItem.ItemModel is Backpack)
                {
                    craftItem.Init(GameManager, backPackItem.Value, backPackItem.Key, "1");
                    cItem = craftItem;
                }
            }

            selectedItem.SetView(cItem, GameManager.PlayerModel.CurrentBackpack == BackpackType.Big);
        }

        private Type GetNextBackpack()
        {
            switch (GameManager.PlayerModel.CurrentBackpack)
            {
                case BackpackType.None:
                    return typeof(SmallBackpack);
                    break;
                case BackpackType.Small:
                    return typeof(BigBackpack);
                    break;
                case BackpackType.Big:
                    return typeof(BigBackpack);
                    break;
            }

            return null;
        }

        public override void UpdateView()
        {
            base.UpdateView();

            foreach (var craftItem in _items)
                craftItem.UpdateView();
        }

        public override void Show()
        {
            base.Show();

            GameManager.Player.MainHud.InventoryPanel.QuickSlotsPanel.Hide();
            GameManager.Player.MainHud.SetActiveButtons(false);
            GameManager.Player.MainHud.SetActiveControls(false);

            SelectCategory(CraftCategory.Common);

            UpdateView();
        }

        public override void Hide()
        {
            GameManager.Player.MainHud.SetActiveControls(true);
            GameManager.Player.MainHud.SetActiveButtons(true);
            GameManager.Player.MainHud.InventoryPanel.QuickSlotsPanel.Show();

            //ShopView.Hide();

            base.Hide();
        }

        private void OnItemClick(CraftItem item)
        {
            ItemsMainObject.SetActive(false);
            SelectedItemView.Show();

            bool isMax = false;
            if (GameManager.PlayerModel.CurrentBackpack == BackpackType.Big && item.ItemModel is Backpack)
                isMax = true;
            SelectedItemView.SetView(item, isMax);
        }

        public void SelectCategory(CraftCategory type, bool shopGoldCategory = false, NewShopCategory shopCategory = NewShopCategory.Items)
        {
            if (type == CraftCategory.Shop)
            {
                //ItemsMainObject.SetActive(false);
                //ShopView.Show();
                //if (shopGoldCategory)
                //    ShopView.SelectCategory(ShopCategory.Gold);

                GameManager.Player.MainHud.ShopPanel.Show(shopCategory);
            }
            else
            {
                ItemsMainObject.SetActive(true);
                //ShopView.Hide();

                foreach (var craftItem in _items)
                {
                    if (craftItem.Category == type)
                        craftItem.Show();
                    else
                        craftItem.Hide();
                }
            }

            ItemsGrid.Reposition();

            foreach (var craftTab in Tabs)
                craftTab.SetActive(type == craftTab.TabType);
        }

        private void OnTabClick(GameObject go)
        {
            var tab = go.GetComponent<CraftTab>();
            SelectCategory(tab.TabType);
            SoundManager.PlaySFX(WorldConsts.AudioConsts.ButtonClick);
        }
    }
}
