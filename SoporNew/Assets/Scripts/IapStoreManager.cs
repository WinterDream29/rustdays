using System.Collections.Generic;
using Assets.Scripts.Models.Food;
using Assets.Scripts.Models.Meds;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using System;
using Assets.Scripts.Models.Ammo;
using Assets.Scripts.Models.ResourceObjects;
using System.Collections;
using Assets.Scripts.Models.Weapons;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Clothes;
using Assets.Scripts.Models.Decor.Pictures;
using Assets.Scripts.Models.ResourceObjects.CraftingResources;
using Assets.Scripts.Models.Seedlings.Cabbage;
using Assets.Scripts.Models.Seedlings.Carrot;
using Assets.Scripts.Models.Seedlings.Corn;
using Assets.Scripts.Models.Seedlings.Pumpkin;
using Assets.Scripts.Models.Seedlings.Strawberry;
using Assets.Scripts.Models.Seedlings.Watermelon;
using CodeStage.AntiCheat.ObscuredTypes;

namespace Assets.Scripts
{
    public class IapStoreManager : MonoBehaviour, IStoreListener
    {
        public static IStoreController StoreController; 
        private static IExtensionProvider StoreExtensionProvider;

        public const string BUY_1000_DOLLARS_ID = "buy_gold_0";
        public const string BUY_5000_DOLLARS_ID = "buy_gold_1";
        public const string BUY_30000_DOLLARS_ID = "buy_gold_2";
        public const string BUY_30000_DOLLARS_FIRST_ID = "buy_gold_2_first";
        public const string BUY_20000_DOLLARS_ID = "buy_gold_3";
        public const string STARTER_PACK = "outcast_starter_pack";
        public const string STARTER_PACK_OLD = "outcast_starter_pack_old";
        public const string FIGHTER_KIT = "fighter_kit";
        public const string BUILDER_KIT = "builder_kit";
        public const string SURVIVAL_KIT = "survival_kit";
        public const string WEAPON_PACK = "weapon_pack";
#if UNITY_ANDROID
        public const string NO_ADS = "no_ads";
#else
        public const string NO_ADS = "survivor_no_ads"; 
#endif
        public const string FREE_GOLD = "free_gold";

        public Action<string> OnBuyCurrency;
        public List<IapGoodItem> GoodItems;
        public Dictionary<string, IapItemDefinition> IapItemDefinitions;

        private GameManager _gameManager;
        public bool IsBuyFirst30000 { get; set; }
        public bool IsBuyStarterPack { get; set; }
        public bool IsBuyNoAds { get; set; }
        private int _stockTime;
        private string _stockTimeString;

        private int _starterPackTime;
        private string _starterPackTimeString;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            GoodItems = new List<IapGoodItem>
            {
                new IapGoodItem(typeof(Adrenaline).Name, 1, 80, "adrenaline", "adrenaline_descr"),
                new IapGoodItem(typeof(Medkit).Name, 1, 80, "medkit", "medkit_descr"),
                new IapGoodItem(typeof(BottleWater).Name, 1, 50, "bottle_water", "bottle_water_descr"),
                new IapGoodItem(typeof(Chocolate).Name, 1, 50, "chocolate", "chocolate_descr"),
                new IapGoodItem(typeof(Soda).Name, 1, 60, "soda", "soda_descr"),
                //new IapGoodItem(typeof(Arrow).Name, 10, 100, "arrow", "arrow_descr"),
                new IapGoodItem(typeof(Jerrycan).Name, 1, 50, "jerrycan", "jerrycan_descr"),
                new IapGoodItem(typeof(WoodResource).Name, 100, 150, "wood_resource", "wood_resource_descr"),
                new IapGoodItem(typeof(StoneResource).Name, 100, 250, "stone_resource", "stone_resource_descr"),
                new IapGoodItem(typeof(Metal).Name, 50, 500, "metal", "metal_descr"),
                new IapGoodItem(typeof(Brick).Name, 100, 300, "brick", "brick_descr"),
                new IapGoodItem(typeof(GlassFragments).Name, 100, 300, "glass_fragments", "glass_fragments_descr"),
                new IapGoodItem(typeof(Gunpowder).Name, 10, 300, "gunpowder", "gunpowder_descr"),
                new IapGoodItem(typeof(CarrotSeed).Name, 10, 60, "carrot_seed", "carrot_seed_descr"),
                new IapGoodItem(typeof(PumpkinSeed).Name, 10, 80, "pumpkin_seed", "pumpkin_seed_descr"),
                new IapGoodItem(typeof(CornSeed).Name, 10, 100, "corn_seed", "corn_seed_descr"),
                new IapGoodItem(typeof(WatermelonSeed).Name, 10, 100, "watermelon_seed", "watermelon_seed_descr"),
                new IapGoodItem(typeof(CabbageSeed).Name, 10, 80, "cabbage_seed", "cabbage_seed_descr"),
                new IapGoodItem(typeof(StrawberrySeed).Name, 10, 150, "strawberry_seed", "strawberry_seed_descr"),
                new IapGoodItem(typeof(MonaLisaPicture).Name, 1, 400, "picture", "wall_place_descr"),
                new IapGoodItem(typeof(MonroPicture).Name, 1, 200, "picture", "wall_place_descr"),
                new IapGoodItem(typeof(Picture1).Name, 1, 500, "picture", "wall_place_descr"),
                new IapGoodItem(typeof(Picture2).Name, 1, 300, "picture", "wall_place_descr"),
                new IapGoodItem(typeof(Picture3).Name, 1, 100, "picture", "wall_place_descr"),
                new IapGoodItem(typeof(Picture4).Name, 1, 150, "picture", "wall_place_descr"),
                new IapGoodItem(typeof(Picture5).Name, 1, 300, "picture", "wall_place_descr")
            };

            IapItemDefinitions = new Dictionary<string, IapItemDefinition>
            {
                { NO_ADS, new IapItemDefinition(0, "no_ads", "No-Ads")},
                { FREE_GOLD, new IapItemDefinition(30, "free", "free")},
                { BUY_1000_DOLLARS_ID, new IapItemDefinition(500, "1000_gold_name", "500_gold")},
                { BUY_5000_DOLLARS_ID, new IapItemDefinition(1000, "5000_gold_name", "1000_gold_icon", null, 0, false, true)},
                { BUY_30000_DOLLARS_ID, new IapItemDefinition(5000, "30000_gold_name", "5000_gold_name_icon", null, 20, true)},
                { BUY_20000_DOLLARS_ID, new IapItemDefinition(10000, "20000_gold_name", "30000_gold_name_icon", null, 25)},
                { STARTER_PACK, new IapItemDefinition(1000, "starter_pack", "starter_pack_icon", new Dictionary<string, int>
                {
                    {typeof(Bow).Name, 1},
                    {typeof(Arrow).Name, 40},
                    {typeof(MetalHatchet).Name, 1},
                    {typeof(MetalPick).Name, 1},
                    {typeof(BottleWater).Name, 3},
                    {typeof(Adrenaline).Name, 10},
                    {typeof(Medkit).Name, 5},
                    {typeof(WoodResource).Name, 300},
                    {typeof(StoneResource).Name, 200}
                },
                50, true)},
                { WEAPON_PACK, new IapItemDefinition(500, "weapon_pack", "weapon_pack", new Dictionary<string, int>
                {
                    {typeof(Revolver).Name, 1},
                    {typeof(RevolverBullet).Name, 60},
                    {typeof(Shotgun).Name, 1},
                    {typeof(ShotgunAmmo).Name, 48},
                    {typeof(DragunobSniperRifle).Name, 1},
                    {typeof(DragunovAmmo).Name, 30},
                    {typeof(Machinegun).Name, 1},
                    {typeof(MachinegunAmmo).Name, 120}

                }, 50, false, false, "weapon_pack_descr")},
                { FIGHTER_KIT, new IapItemDefinition(500, "fighter_kit", "fighter_kit", new Dictionary<string, int>
                {
                    {typeof(Crossbow).Name, 1},
                    {typeof(CrossbowArrow).Name, 30},
                    {typeof(Machete).Name, 1},
                    {typeof(LeatherCap).Name, 5},
                    {typeof(LeatherShirt).Name, 5},
                    {typeof(LeatherPants).Name, 5},
                    {typeof(LeatherBoots).Name, 5}

                })},
                { BUILDER_KIT, new IapItemDefinition(500, "builder_kit", "builder_kit", new Dictionary<string, int>
                {
                    {typeof(WoodResource).Name, 1000},
                    {typeof(StoneResource).Name, 400},
                    {typeof(Brick).Name, 200},
                    {typeof(Metal).Name, 200},
                    {typeof(GlassFragments).Name, 200},
                }, 25)},
                { SURVIVAL_KIT, new IapItemDefinition(500, "survival_kit", "survival_kit", new Dictionary<string, int>
                {
                    {typeof(BottleWater).Name, 10},
                    {typeof(FriedMeat).Name, 20},
                    {typeof(FriedFish).Name, 10},
                    {typeof(Chocolate).Name, 10},
                    {typeof(Adrenaline).Name, 10},
                    {typeof(Medkit).Name, 10},
                })},
            };

            //var item = HolderObjectFactory.GetItem(typeof(Adrenaline), 5);
            //AddItem(item);
            //item = HolderObjectFactory.GetItem(typeof(Medkit), 5);
            //AddItem(item);
            //item = HolderObjectFactory.GetItem(typeof(CrossbowArrow), 30);
            //AddItem(item);
            //item = HolderObjectFactory.GetItem(typeof(Metal), 300);
            //AddItem(item);
            //item = HolderObjectFactory.GetItem(typeof(Brick), 500);
            //AddItem(item);
            //item = HolderObjectFactory.GetItem(typeof(Crossbow), 1);
            //AddItem(item);

            if (StoreController == null)
                InitializePurchasing();

            _stockTime = PlayerPrefs.GetInt("StockTime", 7199);
            _starterPackTime = PlayerPrefs.GetInt("StarterPackTime", 3599);
            StartCoroutine(UpdateStockTime());
        }

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void InitializePurchasing()
        {
            if (IsInitialized())
            {
                return;
            }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct(NO_ADS, ProductType.NonConsumable);
            builder.AddProduct(BUY_1000_DOLLARS_ID, ProductType.Consumable);
            builder.AddProduct(BUY_5000_DOLLARS_ID, ProductType.Consumable);
            builder.AddProduct(BUY_30000_DOLLARS_ID, ProductType.Consumable);
            builder.AddProduct(BUY_30000_DOLLARS_FIRST_ID, ProductType.Consumable);
            builder.AddProduct(BUY_20000_DOLLARS_ID, ProductType.Consumable);
            builder.AddProduct(STARTER_PACK, ProductType.Consumable);
            builder.AddProduct(WEAPON_PACK, ProductType.Consumable);
            builder.AddProduct(STARTER_PACK_OLD, ProductType.Consumable);
            builder.AddProduct(FIGHTER_KIT, ProductType.Consumable);
            builder.AddProduct(BUILDER_KIT, ProductType.Consumable);
            builder.AddProduct(SURVIVAL_KIT, ProductType.Consumable);

            UnityPurchasing.Initialize(this, builder);
        }


        public bool IsInitialized()
        {
            return StoreController != null && StoreExtensionProvider != null && GoodItems != null && IapItemDefinitions != null;
        }

        public void BuyProductId(string productId)
        {
            if (IsInitialized())
            {
                Product product = StoreController.products.WithID(productId);

                if (product != null && product.availableToPurchase)
                {
                    //Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    StoreController.InitiatePurchase(product);
                }
                else
                {
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }

        // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
        // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
        public void RestorePurchases()
        {
            // If Purchasing has not yet been set up ...
            if (!IsInitialized())
            {
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                //Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            // If we are running on an Apple device ... 
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                // ... begin restoring purchases
                //Debug.Log("RestorePurchases started ...");

                // Fetch the Apple store-specific subsystem.
                var apple = StoreExtensionProvider.GetExtension<IAppleExtensions>();
                // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
                // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
                apple.RestoreTransactions((result) =>
                {
                    // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                    // no purchases are available to be restored.
                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                });
            }
            // Otherwise ...
            else
            {
                // We are not running on an Apple device. No work is necessary to restore purchases.
                //Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            //Debug.LogError("OnInitialized: PASS");

            StoreController = controller;
            StoreExtensionProvider = extensions;
        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }


        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            bool validPurchase = true;
            // Unity IAP's validation logic is only included on these platforms.
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX)
            // Prepare the validator with the secrets we prepared in the Editor
            // obfuscation window.
            var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                AppleTangle.Data(), Application.identifier);

            try
            {
                // On Google Play, result has a single product ID.
                // On Apple stores, receipts contain multiple products.
                var result = validator.Validate(args.purchasedProduct.receipt);
                // For informational purposes, we list the receipt(s)
                //MyLogger.Log("Receipt is valid. Contents:");
                //foreach (IPurchaseReceipt productReceipt in result)
                //{
                //    Debug.Log(productReceipt.productID);
                //    Debug.Log(productReceipt.purchaseDate);
                //    Debug.Log(productReceipt.transactionID);
                //}
            }
            catch (IAPSecurityException)
            {
                Debug.Log("Invalid receipt, not unlocking content");
                validPurchase = false;
            }
#endif

            if (validPurchase)
            {
                StartCoroutine(VerifyReceiptCo(args.purchasedProduct));
            }

            //AddCapsById(args.purchasedProduct.definition.id);
            //if (OnBuyCurrency != null)
            //    OnBuyCurrency(args.purchasedProduct.definition.id);

            return PurchaseProcessingResult.Complete;
        }

        private IEnumerator VerifyReceiptCo(Product product)
        {
            //need show loader...

            if (Application.platform == RuntimePlatform.Android)
            {
                if (!_gameManager.ServerGameSettings.ConfigurationIsLoaded)
                {
                    _gameManager.ServerGameSettings.ReloadConfigureFile();
                    yield return new WaitForSeconds(10.0f);
                }

                var result = "0";

                var data = JsonUtility.FromJson<ReceiptData>(product.receipt);
                var payload = JsonUtility.FromJson<ReceiptPayloadData>(data.Payload);

                WWWForm hsFm = new WWWForm();
                hsFm.AddField("data", payload.json);
                hsFm.AddField("signature", payload.signature);

                var validatePath = _gameManager.ServerGameSettings.GetValidatePurchasePath();
                if (string.IsNullOrEmpty(validatePath) || validatePath == ServerGameSettings.ConfigurationNotLoadedString)
                {
                    result = "1";
                }
                else
                {
                    WWW hs = new WWW(_gameManager.ServerGameSettings.GetValidatePurchasePath(), hsFm);
                    yield return hs;

                    if (!string.IsNullOrEmpty(hs.error))
                    {
                        result = "1";
                        Debug.LogError("validate get data error: " + result);
                    }

                    result = hs.text;
                }

                Debug.Log("verify result is " + result);

                if (result == "1")
                {
                    AddCapsById(product.definition.id);
                    if (OnBuyCurrency != null)
                        OnBuyCurrency(product.definition.id);

                    Debug.Log("Purchased item " + product.definition.id);
                }
                else
                {
                    //if (OnValidatePurchaseFail != null)
                    //    OnValidatePurchaseFail();
                    Debug.Log("Hack Purchased item " + product.definition.id);
                }
            }
            else
            {
                AddCapsById(product.definition.id);
                if (OnBuyCurrency != null)
                    OnBuyCurrency(product.definition.id);

                Debug.Log("Purchased item " + product.definition.id);
                yield break;
            }
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }

        public void AddCapsById(string itemId)
        {
            Reward(itemId);
        }

        public void Reward(string id)
        {
            var item = IapItemDefinitions[id];
            AddCaps(item.Currency);
            if (item.Items != null)
            {
                foreach (var itemReward in item.Items)
                {
                    var ho = HolderObjectFactory.GetItem(itemReward.Key, itemReward.Value);
                    AddItem(ho);
                }
            }

            if (id == NO_ADS)
            {
                IsBuyNoAds = true;
                ObscuredPrefs.SetBool(WorldConsts.BuyNoAds, true);
            }

            ProgressManager.SaveProgress(_gameManager);
        }

        public void AddCaps(int amount)
        {
            CurrencyManager.AddCurrency(amount);
            ProgressManager.SaveProgress(_gameManager);
        }

        private void AddItem(HolderObject item)
        {
            var isAdd = _gameManager.PlayerModel.Inventory.AddItem(item);
            if (!isAdd)
                _gameManager.PlacementItemsController.DropItemToGround(_gameManager, item);
        }

        private IEnumerator UpdateStockTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);
                _stockTime -= 1;
                if (_stockTime < 0)
                    _stockTime = 4000;

                _starterPackTime -= 1;
                if (_starterPackTime < 0)
                    _starterPackTime = 3599;
            }
        }

        public string GetStockTime()
        {
            TimeSpan t = TimeSpan.FromSeconds(_stockTime);
            _stockTimeString = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);
            return _stockTimeString;
        }

        public string GetStarterPackTime()
        {
            TimeSpan t = TimeSpan.FromSeconds(_starterPackTime);
            _starterPackTimeString = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);
            return _starterPackTimeString;
        }

        void OnDestroy()
        {
            PlayerPrefs.SetInt("StockTime", _stockTime);
            PlayerPrefs.SetInt("StarterPackTime", _starterPackTime);
        }
    }
}