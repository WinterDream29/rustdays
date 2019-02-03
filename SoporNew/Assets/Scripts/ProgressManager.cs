using System;
using System.Collections.Generic;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Clothes;
using Assets.Scripts.Models.Common;
using Assets.Scripts.Models.Constructions.Wood;
using Assets.Scripts.Models.Food;
using Assets.Scripts.Models.ResourceObjects;
using Assets.Scripts.Models.Weapons;
using Assets.Scripts.SaveModels;
using Assets.Scripts.WorkWithFiles;
using UnityEngine;
using Assets.Scripts.Models.Tools;
using Assets.Scripts.Models.Constructions;
using Assets.Scripts.Models.Ammo;
using JsonWriter = Pathfinding.Serialization.JsonFx.JsonWriter;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using CodeStage.AntiCheat.ObscuredTypes;

namespace Assets.Scripts
{
    public static class ProgressManager
    {
        public static void SaveProgress(GameManager gameManager, bool toCloud = false)
        {
            ObscuredPrefs.SetInt(WorldConsts.Currency, CurrencyManager.CurrentCurrency);
            MainSaveModel.Instanse.CurrentCurrency = CurrencyManager.CurrentCurrency;
            MainSaveModel.Instanse.LogoutTs = DateTime.Now;
            
            MainSaveModel.Instanse.Stats = new Dictionary<string, float>();
            MainSaveModel.Instanse.Stats[WorldConsts.Health] = gameManager.PlayerModel.Health;
            MainSaveModel.Instanse.Stats[WorldConsts.Hunger] = gameManager.PlayerModel.Hunger;
            MainSaveModel.Instanse.Stats[WorldConsts.Thirst] = gameManager.PlayerModel.Thirst;
            MainSaveModel.Instanse.Stats[WorldConsts.Energy] = gameManager.PlayerModel.Energy;

            MainSaveModel.Instanse.CurrentTime = TOD_Sky.Instance.Cycle.Hour;

            MainSaveModel.Instanse.IsBuyStarterPack = gameManager.IapManager.IsBuyStarterPack;
            MainSaveModel.Instanse.IsBuyFirst30000 = gameManager.IapManager.IsBuyFirst30000;
            MainSaveModel.Instanse.IsBuyNoAds = gameManager.IapManager.IsBuyNoAds;

            MainSaveModel.Instanse.InCar = gameManager.Player.InCar;

            MainSaveModel.Instanse.CurrentBackpack = (int)gameManager.PlayerModel.CurrentBackpack;
            var playerPosition = gameManager.Player.transform.localPosition;
            MainSaveModel.Instanse.PlayerPosition = new List<float>
            {
                playerPosition.x,
                playerPosition.y,
                playerPosition.z
            };
            var playerRotation = gameManager.Player.transform.localEulerAngles;
            MainSaveModel.Instanse.PlayerRotation = new List<float>
            {
                playerRotation.x,
                playerRotation.y,
                playerRotation.z
            };

            var respawnPlayerPointPos = gameManager.Player.RespawnPoint.position;
            MainSaveModel.Instanse.RespawnPointPosition = new List<float>
            {
                respawnPlayerPointPos.x,
                respawnPlayerPointPos.y,
                respawnPlayerPointPos.z
            };

            #region Inventory
            MainSaveModel.Instanse.PlayerInventory = new InventorySaveModel();

            for (int i = 0; i < gameManager.PlayerModel.Inventory.MaxSlots; i++)
            {
                var slot = gameManager.PlayerModel.Inventory.Slots[i];
                if (slot != null)
                {
                    MainSaveModel.Instanse.PlayerInventory.ItemInSlots[i.ToString()] = slot.Item.GetType().Name;
                    MainSaveModel.Instanse.PlayerInventory.ItemAmountInSlots[i.ToString()] = slot.Amount;
                    MainSaveModel.Instanse.PlayerInventory.ItemDurabilityInSlots[i.ToString()] = slot.CurrentDurability;
                }
            }
            for (int i = 0; i < WorldConsts.QuickSlotsAmount; i++)
            {
                var slot = gameManager.PlayerModel.Inventory.QuickSlots[i];
                if (slot != null)
                {
                    MainSaveModel.Instanse.PlayerInventory.ItemInQuickSlots[i.ToString()] = slot.Item.GetType().Name;
                    MainSaveModel.Instanse.PlayerInventory.ItemAmountInQuickSlots[i.ToString()] = slot.Amount;
                    MainSaveModel.Instanse.PlayerInventory.ItemDurabilityInQuickSlots[i.ToString()] = slot.CurrentDurability;
                }
            }
            for (int i = 0; i < WorldConsts.EquipSlotsAmount; i++)
            {
                var slot = gameManager.PlayerModel.Inventory.EquipSlots[i];
                if (slot != null)
                {
                    MainSaveModel.Instanse.PlayerInventory.ItemInEquipSlots[i.ToString()] = slot.Item.GetType().Name;
                    MainSaveModel.Instanse.PlayerInventory.ItemAmountInEquipSlots[i.ToString()] = slot.Amount;
                    MainSaveModel.Instanse.PlayerInventory.ItemDurabilityInEquipSlots[i.ToString()] = slot.CurrentDurability;
                }
            }
            #endregion

            MainSaveModel.Instanse.GroundItems = gameManager.PlacementItemsController.GetItemsToSerialize();

            if (gameManager.CarInteractive != null)
            {
                var carModel = new CarSaveModel();
                var carTransfrom = gameManager.CarInteractive.transform;
                carModel.PosX = carTransfrom.position.x;
                carModel.PosY = carTransfrom.position.y;
                carModel.PosZ = carTransfrom.position.z;

                var carRotation = carTransfrom.eulerAngles;
                carModel.RotX = carRotation.x;
                carModel.RotY = carRotation.y;
                carModel.RotZ = carRotation.z;

                carModel.Petrol = gameManager.CarInteractive.Petrol;
                MainSaveModel.Instanse.CarModel = carModel;
            }

            MainSaveModel.Instanse.CurrentTerratinId = gameManager.CurrentTerain == gameManager.Terrain2 ? 1 : 0;

            var jsString = JsonWriter.Serialize(MainSaveModel.Instanse);
            var saveData = AES.Encrypt(jsString, WorldConsts.CryptKey);
            ObscuredPrefs.SetString(WorldConsts.Save0, saveData);

            if(toCloud)
                GooglePlayServicesController.SaveGame(GetBytes(saveData));

            //Debug.Log("Save Progress " + jsString);
        }

        public static void LoadProgress(GameManager gameManager)
        {
            var saveData = ObscuredPrefs.GetString(WorldConsts.Save0, "");
            if(string.IsNullOrEmpty(saveData))
                saveData = PlayerPrefs.GetString(WorldConsts.Save0);

            if (PlayerPrefs.GetInt(WorldConsts.LoadFromCloud, 0) == 1)
                saveData = PlayerPrefs.GetString(WorldConsts.CurrentCloudSave);

            if (string.IsNullOrEmpty(saveData))
            {
                LoadDefaultProgress(gameManager);
                return;
            }

            var progress = AES.Decrypt(saveData, WorldConsts.CryptKey);
            fastJSON.JSON.Instance.Parameters.EnableAnonymousTypes = true;
            MainSaveModel.Instanse.ParseJsonStringToSaveItemModel(progress);

            CurrencyManager.AddCurrency(MainSaveModel.Instanse.CurrentCurrency);

            gameManager.CurrentTerain = MainSaveModel.Instanse.CurrentTerratinId == 0
                ? gameManager.Terrain1
                : gameManager.Terrain2;

            gameManager.IapManager.IsBuyStarterPack = MainSaveModel.Instanse.IsBuyStarterPack;
            gameManager.IapManager.IsBuyFirst30000 = MainSaveModel.Instanse.IsBuyFirst30000;
            gameManager.IapManager.IsBuyNoAds = MainSaveModel.Instanse.IsBuyNoAds;

            gameManager.PlayerModel.Health = MainSaveModel.Instanse.Stats[WorldConsts.Health];
            gameManager.PlayerModel.Hunger = MainSaveModel.Instanse.Stats[WorldConsts.Hunger];
            gameManager.PlayerModel.Thirst = MainSaveModel.Instanse.Stats[WorldConsts.Thirst];
            gameManager.PlayerModel.Energy = MainSaveModel.Instanse.Stats[WorldConsts.Energy];
            TimeSpan difference = DateTime.Now.Subtract(MainSaveModel.Instanse.LogoutTs);
            gameManager.PlayerModel.AddSleepEnergy(difference);
            gameManager.PlayerModel.Breath = 100f;

            TOD_Sky.Instance.Cycle.Hour = MainSaveModel.Instanse.CurrentTime;

            gameManager.Player.RespawnPoint.position = new Vector3(
                MainSaveModel.Instanse.RespawnPointPosition[0],
                MainSaveModel.Instanse.RespawnPointPosition[1],
                MainSaveModel.Instanse.RespawnPointPosition[2]);

            gameManager.Player.transform.localPosition = new Vector3(
                MainSaveModel.Instanse.PlayerPosition[0],
                MainSaveModel.Instanse.PlayerPosition[1],
                MainSaveModel.Instanse.PlayerPosition[2]);
            gameManager.Player.transform.eulerAngles = new Vector3(
                MainSaveModel.Instanse.PlayerRotation[0],
                MainSaveModel.Instanse.PlayerRotation[1],
                MainSaveModel.Instanse.PlayerRotation[2]);

            #region Inventory

            gameManager.PlayerModel.CurrentBackpack = (BackpackType)MainSaveModel.Instanse.CurrentBackpack;
            gameManager.PlayerModel.PrepareInventorySlots();

            if (MainSaveModel.Instanse.PlayerInventory != null)
            {
                for (int i = 0; i < gameManager.PlayerModel.Inventory.MaxSlots; i++)
                {
                    if (MainSaveModel.Instanse.PlayerInventory.ItemInSlots != null &&
                        MainSaveModel.Instanse.PlayerInventory.ItemInSlots.ContainsKey(i.ToString()))
                    {
                        var itemName = MainSaveModel.Instanse.PlayerInventory.ItemInSlots[i.ToString()];
                        var itemAmount = MainSaveModel.Instanse.PlayerInventory.ItemAmountInSlots[i.ToString()];
                        var durability = MainSaveModel.Instanse.PlayerInventory.ItemDurabilityInSlots[i.ToString()];

                        gameManager.PlayerModel.Inventory.Slots[i] = HolderObjectFactory.GetItem(itemName, itemAmount, durability);
                    }
                }
                for (int i = 0; i < WorldConsts.QuickSlotsAmount; i++)
                {
                    if (MainSaveModel.Instanse.PlayerInventory.ItemInQuickSlots != null &&
                        MainSaveModel.Instanse.PlayerInventory.ItemInQuickSlots.ContainsKey(i.ToString()))
                    {
                        var itemName = MainSaveModel.Instanse.PlayerInventory.ItemInQuickSlots[i.ToString()];
                        var itemAmount = MainSaveModel.Instanse.PlayerInventory.ItemAmountInQuickSlots[i.ToString()];
                        var durability = MainSaveModel.Instanse.PlayerInventory.ItemDurabilityInQuickSlots[i.ToString()];

                        gameManager.PlayerModel.Inventory.QuickSlots[i] = HolderObjectFactory.GetItem(itemName, itemAmount, durability);
                    }
                }
                for (int i = 0; i < WorldConsts.EquipSlotsAmount; i++)
                {
                    if (MainSaveModel.Instanse.PlayerInventory.ItemInEquipSlots != null &&
                        MainSaveModel.Instanse.PlayerInventory.ItemInEquipSlots.ContainsKey(i.ToString()))
                    {
                        var itemName = MainSaveModel.Instanse.PlayerInventory.ItemInEquipSlots[i.ToString()];
                        var itemAmount = MainSaveModel.Instanse.PlayerInventory.ItemAmountInEquipSlots[i.ToString()];
                        var durability = MainSaveModel.Instanse.PlayerInventory.ItemDurabilityInEquipSlots[i.ToString()];

                        gameManager.PlayerModel.Inventory.EquipSlots[i] = HolderObjectFactory.GetItem(itemName, itemAmount, durability);
                    }
                }
            }
            #endregion

            if (MainSaveModel.Instanse.GroundItems != null)
                gameManager.PlacementItemsController.PlaceSavedItems(MainSaveModel.Instanse.GroundItems);

            if (MainSaveModel.Instanse.CarModel != null)
            {
                if (gameManager.CarInteractive != null)
                {
                    var carTransform = gameManager.CarInteractive.transform;
                    carTransform.position = new Vector3(MainSaveModel.Instanse.CarModel.PosX,
                        MainSaveModel.Instanse.CarModel.PosY, MainSaveModel.Instanse.CarModel.PosZ);
                    carTransform.eulerAngles = new Vector3(MainSaveModel.Instanse.CarModel.RotX,
                        MainSaveModel.Instanse.CarModel.RotY, MainSaveModel.Instanse.CarModel.RotZ);
                    gameManager.CarInteractive.Petrol = MainSaveModel.Instanse.CarModel.Petrol;
                }
            }
            else
            {
                if (gameManager.CarInteractive != null)
                {
                    gameManager.CarInteractive.Petrol = 0.5f;
                }
            }

            if (MainSaveModel.Instanse.InCar)
            {
                //var posY = Terrain.activeTerrain.SampleHeight(gameManager.CarInteractive.PlayerOutPosition.position) + 2.0f;
                var posY = gameManager.CurrentTerain.SampleHeight(gameManager.CarInteractive.PlayerOutPosition.position) + 2.0f;
                gameManager.Player.transform.position = new Vector3(gameManager.CarInteractive.PlayerOutPosition.position.x, posY, gameManager.CarInteractive.PlayerOutPosition.position.z);
            }

            //Debug.Log("Load Progress " + progress);

            //AddTestItems(gameManager);
        }

        public static void LoadProgressFromCloud(GameManager gameManager)
        {
            var encryptedSave = GetString(GooglePlayServicesController.SaveData);
            if (string.IsNullOrEmpty(encryptedSave))
                return;

            PlayerPrefs.SetInt(WorldConsts.LoadFromCloud, 1);
            PlayerPrefs.SetString(WorldConsts.CurrentCloudSave, encryptedSave);
            gameManager.RestartGame();
        }

        private static void LoadDefaultProgress(GameManager gameManager)
        {
            CurrencyManager.SetCurrency(ObscuredPrefs.GetInt(WorldConsts.Currency, 80));

            gameManager.PlayerModel.Health = 100f;
            gameManager.PlayerModel.Hunger = 80f;
            gameManager.PlayerModel.Thirst = 80;
            gameManager.PlayerModel.Energy = 90f;
            gameManager.PlayerModel.Breath = 100f;

            gameManager.PlayerModel.CurrentBackpack = BackpackType.None;
            gameManager.PlayerModel.PrepareInventorySlots();

            gameManager.IapManager.IsBuyNoAds = ObscuredPrefs.GetBool(WorldConsts.BuyNoAds, false);

            TOD_Sky.Instance.Cycle.Hour = 7.0f;

            if(gameManager.CarInteractive != null)
                gameManager.CarInteractive.Petrol = 0.5f;

            gameManager.PlacementItemsController.AddStartLoot();

            gameManager.CurrentTerain = gameManager.Terrain1;

            //gameManager.Intro.Run(gameManager);
            //AddTestItems(gameManager);
        }

        public static void DeleteProgress()
        {
            PlayerPrefs.DeleteKey(WorldConsts.Save0);
            ObscuredPrefs.DeleteKey(WorldConsts.Save0);
        }

        private static void AddTestItems(GameManager gameManager)
        {
            //gameManager.PlayerModel.Inventory.AddToQuick(HolderObjectFactory.GetItem(typeof(StoneHatchet), 1));
            //gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(Leather), 100));
            //gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(Rope), 100));
            //gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(Fur), 100));
            //gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(Furnace), 1));
            //gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(CampFire), 1));
            //gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(MetalOre), 100));
            //gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(LeatherBoots), 1));
            //gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(LeatherCap), 1));
            //gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(Bed), 1));
            //gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(Furnace), 1));
            gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(Leather), 1000));
            gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(Fur), 1000));
            gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(Metal), 1000));
            gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(Crowbar), 1));
            gameManager.PlayerModel.Inventory.AddItem(HolderObjectFactory.GetItem(typeof(MetalHatchet), 1));
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
