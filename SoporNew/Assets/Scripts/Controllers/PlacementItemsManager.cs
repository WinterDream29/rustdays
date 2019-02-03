using System.Collections.Generic;
using Assets.Scripts.Models;
using Assets.Scripts.SaveModels;
using UnityEngine;
using Assets.Scripts.Controllers.InteractiveObjects.MiningObjects;
using Assets.Scripts.Controllers.UsableObjects;
using Assets.Scripts.Models.Common;
using Assets.Scripts.Controllers.Constructions;

namespace Assets.Scripts.Controllers
{
    public class PlacementItemsManager : MonoBehaviour
    {
        public List<InventoryInteractive> LootChests; 

        private Dictionary<GameObject, GroundPlacementItemData> _placedItems = new Dictionary<GameObject, GroundPlacementItemData>();
        private List<ConstructionController> _constructionObjects = new List<ConstructionController>();

        public void AddPlacedItem(GameObject go, BaseObject item, bool dropped = false, int amount = 1, int? durability = null)
        {
            if (go != null)
            {
                _placedItems[go] = new GroundPlacementItemData(item, dropped, amount, durability);
                AddConstructionItem(go);
            }
                
        }

        public void RemovePlacedItem(GameObject go)
        {
            if (_placedItems.ContainsKey(go))
            {
                _placedItems.Remove(go);
                RemoveConstructionItem(go);
            }
        }

        public void AddStartLoot()
        {
            foreach(var item in LootChests)
            {
                AddPlacedItem(item.gameObject, BaseObjectFactory.GetItem(typeof(Chest)));
            }
        }

        public void PlaceSavedItems(List<GroundItemSaveModel> items)
        {
            foreach (var item in LootChests)
            {
                item.gameObject.SetActive(false);
            }

            foreach (var groundItemSaveModel in items)
            {
                var item = BaseObjectFactory.GetItem(groundItemSaveModel.ItemName);

                string prefabPath = string.Empty;
                if (!groundItemSaveModel.Dropped && item is IPlacement)
                    prefabPath = (item as IPlacement).PrefabPath;
                else
                    prefabPath = item.OnGroundPrefabPath;

                var itemPrefab = Resources.Load<GameObject>(prefabPath);
                var itemGo = Instantiate(itemPrefab);
                if (groundItemSaveModel.Dropped)
                    itemGo.GetComponent<WorldItem>().SetItem(item.GetType(), groundItemSaveModel.Amount, groundItemSaveModel.Durability);

                itemGo.transform.position = new Vector3(groundItemSaveModel.PosX, groundItemSaveModel.PosY,
                    groundItemSaveModel.PosZ);
                itemGo.transform.rotation =
                    Quaternion.Euler(new Vector3(groundItemSaveModel.Pitch, groundItemSaveModel.Roll,
                        groundItemSaveModel.Yaw));

                var destroyable = itemGo.GetComponentInChildren<DestroyableObject>();
                if (destroyable != null)
                {
                    destroyable.CurrentHp = groundItemSaveModel.CurrentHp;
                    destroyable.ItemModel = item;
                    destroyable.Initialized = true;
                }

                if (groundItemSaveModel.IsActive)
                {
                    var iconverter = itemGo.GetComponent<IConverter>();
                    if (iconverter != null)
                        iconverter.AutoStart = true;
                }

                var filledItem = itemGo.GetComponent<IFilled>();
                if (filledItem != null)
                    filledItem.CurrentFilledAmount = groundItemSaveModel.AmountFilled;

                AddPlacedItem(itemGo, item, groundItemSaveModel.Dropped, groundItemSaveModel.Amount);

                var inventoryItem = itemGo.GetComponent<IInventoryThing>();
                if (groundItemSaveModel.InventoryList != null && inventoryItem != null)
                {
                    var inventoryList = new List<InventoryBase>();
                    foreach (var invListModel in groundItemSaveModel.InventoryList.InventoryList)
                    {
                        var currentInventory = new InventoryBase();
                        currentInventory.Init(invListModel.SlotAmount);
                        foreach (var invItemModel in invListModel.Items)
                        {
                            var amount = invItemModel.Amount;
                            if (currentInventory.Slots.Count <= invItemModel.SlotId)
                                currentInventory.Slots.Add(null);
                            currentInventory.Slots[invItemModel.SlotId] =
                                HolderObjectFactory.GetItem(invItemModel.ItemName, amount,
                                    invItemModel.CurrentDurability);
                        }
                        inventoryList.Add(currentInventory);
                    }
                    inventoryItem.SetInventoryList(inventoryList);
                }
            }

            SetActiveConstructionColliders(false);
        }

        public List<GroundItemSaveModel> GetItemsToSerialize()
        {
            var list = new List<GroundItemSaveModel>();
            foreach (var inputObj in _placedItems)
            {
                if (inputObj.Key == null)
                    continue;

                var groundItem = new GroundItemSaveModel();

                groundItem.ItemName = inputObj.Value.ItemModel.GetType().Name;

                var destroyable = inputObj.Key.GetComponentInChildren<DestroyableObject>();
                if (destroyable != null)
                    groundItem.CurrentHp = destroyable.CurrentHp;

                var filledItem = inputObj.Key.GetComponent<IFilled>();
                if (filledItem != null)
                    groundItem.AmountFilled = filledItem.CurrentFilledAmount;

                var converter = inputObj.Key.GetComponent<IConverter>();
                groundItem.IsActive = converter != null && converter.IsBurning;

                var cachedTransf = inputObj.Key.transform;

                groundItem.PosX = cachedTransf.transform.position.x;
                groundItem.PosY = cachedTransf.transform.position.y;
                groundItem.PosZ = cachedTransf.transform.position.z;

                groundItem.Pitch = cachedTransf.transform.rotation.eulerAngles.x;
                groundItem.Roll = cachedTransf.transform.rotation.eulerAngles.y;
                groundItem.Yaw = cachedTransf.transform.rotation.eulerAngles.z;

                groundItem.Dropped = inputObj.Value.Dropped;
                groundItem.Amount = inputObj.Value.Amount;
                groundItem.Durability = inputObj.Value.Durability;

                var inventoryItem = inputObj.Key.GetComponent<IInventoryThing>();
                if (inventoryItem != null)
                {
                    var invListSaveModel = new InventoryBaseSaveModelList();
                    var inventoryList = inventoryItem.GetInventoryList();
                    foreach (var inv in inventoryList)
                    {
                        var invSaveModel = new InventoryBaseSaveModel();
                        invSaveModel.Items = new List<ItemHolderSaveModel>();
                        invSaveModel.SlotAmount = inv.MaxSlots;
                        int i = 0;
                        foreach (var invItem in inv.Slots)
                        {
                            if (invItem != null && invItem.Item != null)
                            {
                                var itemSaveModel = new ItemHolderSaveModel();
                                itemSaveModel.SlotId = i;
                                itemSaveModel.ItemName = invItem.Item.GetType().Name;
                                itemSaveModel.Amount = invItem.Amount;
                                itemSaveModel.CurrentDurability = invItem.CurrentDurability;
                                invSaveModel.Items.Add(itemSaveModel);
                            }
                            i++;
                        }
                        invListSaveModel.InventoryList.Add(invSaveModel);
                    }
                    groundItem.InventoryList = invListSaveModel;
                }

                list.Add(groundItem);
            }

            return list;
        }

        public void DropItemToGround(GameManager gameManager, HolderObject itemHolder)
        {
            if(itemHolder == null || itemHolder.Item == null)
                return;

            var itemPrefab = Resources.Load<GameObject>(itemHolder.Item.OnGroundPrefabPath);
            if (itemPrefab == null)
                return;

            var itemGo = Instantiate(itemPrefab);
            itemGo.transform.position = gameManager.Player.DropObjectSpawnPlace.transform.position;
            itemGo.GetComponent<WorldItem>().SetItem(itemHolder.Item.GetType(), itemHolder.Amount, itemHolder.CurrentDurability);
            AddPlacedItem(itemGo, itemHolder.Item, true, itemHolder.Amount, itemHolder.CurrentDurability);
        }

        #region Constructions
        public void SetActiveConstructionColliders(bool active)
        {
            foreach (var item in _constructionObjects)
            {
                if (item != null && item.RootConstructionColliders != null)
                    item.RootConstructionColliders.SetActive(active);
            }
        }

        private void RemoveConstructionItem(GameObject go)
        {
            var construction = go.GetComponent<ConstructionController>();
            if (construction != null && construction.RootConstructionColliders != null)
            {
                _constructionObjects.Remove(construction);
            }
        }

        private void AddConstructionItem(GameObject go)
        {
            var construction = go.GetComponent<ConstructionController>();
            if (construction != null && construction.RootConstructionColliders != null)
            {
                _constructionObjects.Add(construction);
            }
        }
        #endregion
    }
}
