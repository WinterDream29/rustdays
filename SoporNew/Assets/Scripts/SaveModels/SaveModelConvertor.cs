using System;
using System.Collections.Generic;

namespace Assets.Scripts.SaveModels
{
    [System.Serializable]
    public class SaveModelConvertor 
    {
        public static List<float> ListObjToListFloat(List<object> objItems)
        {
            var list = new List<float>();
            if (objItems == null)
                return list;
            foreach (var item in objItems)
                list.Add(Convert.ToSingle(item));

            return list;
        }

        public static Dictionary<string, float> DictObjToDictFloat(Dictionary<string, object> objItems)
        {
            var dict =new Dictionary<string, float>();
            if (objItems == null)
                return dict;
            foreach (var item in objItems)
                dict[item.Key] = Convert.ToSingle(item.Value);
            return dict;
        }

        public static Dictionary<string, int> DictObjToDictInt(Dictionary<string, object> objItems)
        {
            var dict = new Dictionary<string, int>();
            if (objItems == null)
                return dict;
            foreach (var item in objItems)
                dict[item.Key] = Convert.ToInt32(item.Value);
            return dict;
        }

        public static Dictionary<string, int?> DictObjToDictNullInt(Dictionary<string, object> objItems)
        {
            var dict = new Dictionary<string, int?>();
            if (objItems == null)
                return dict;
            foreach (var item in objItems)
            {
                if (item.Value == null)
                    dict[item.Key] = null;
                else
                    dict[item.Key] = Convert.ToInt32(item.Value);
            }
            return dict;
        }

        public static Dictionary<string, string> DictObjToDictString(Dictionary<string, object> objItems)
        {
            var dict = new Dictionary<string, string>();
            if (objItems == null)
                return dict;
            foreach (var item in objItems)
                dict[item.Key] = item.Value as string;
            return dict;
        }

        public static Dictionary<string, bool> DictObjToDictBool(Dictionary<string, object> objItems)
        {
            var dict = new Dictionary<string, bool>();
            if (objItems == null)
                return dict;
            foreach (var item in objItems)
                dict[item.Key] = Convert.ToBoolean(item.Value);
            return dict;
        }

        public static InventorySaveModel ConvertToPlayerInventory(object inventoryObj)
        {
            var inventorySaveModel = new InventorySaveModel();
            var nodeDict = inventoryObj as Dictionary<string, object>;
            if (nodeDict == null)
                return null;

            var itemsInSlots = nodeDict["ItemInSlots"] as Dictionary<string, object>;
            inventorySaveModel.ItemInSlots = DictObjToDictString(itemsInSlots);
            var itemAmountInSlots = nodeDict["ItemAmountInSlots"] as Dictionary<string, object>;
            inventorySaveModel.ItemAmountInSlots = DictObjToDictInt(itemAmountInSlots);
            var itemDurabilityInSlots = nodeDict["ItemDurabilityInSlots"] as Dictionary<string, object>;
            inventorySaveModel.ItemDurabilityInSlots = DictObjToDictNullInt(itemDurabilityInSlots);

            var itemInQuickSlots = nodeDict["ItemInQuickSlots"] as Dictionary<string, object>;
            inventorySaveModel.ItemInQuickSlots = DictObjToDictString(itemInQuickSlots);
            var itemAmountInQickSlots = nodeDict["ItemAmountInQuickSlots"] as Dictionary<string, object>;
            inventorySaveModel.ItemAmountInQuickSlots = DictObjToDictInt(itemAmountInQickSlots);
            var itemDurabilityInQuickSlots = nodeDict["ItemDurabilityInQuickSlots"] as Dictionary<string, object>;
            inventorySaveModel.ItemDurabilityInQuickSlots = DictObjToDictNullInt(itemDurabilityInQuickSlots);

            var itemInEquipSlots = nodeDict["ItemInEquipSlots"] as Dictionary<string, object>;
            inventorySaveModel.ItemInEquipSlots = DictObjToDictString(itemInEquipSlots);
            var itemAmountInEquipSlots = nodeDict["ItemAmountInEquipSlots"] as Dictionary<string, object>;
            inventorySaveModel.ItemAmountInEquipSlots = DictObjToDictInt(itemAmountInEquipSlots);
            var itemDurabilityInEquipSlots = nodeDict["ItemDurabilityInEquipSlots"] as Dictionary<string, object>;
            inventorySaveModel.ItemDurabilityInEquipSlots = DictObjToDictNullInt(itemDurabilityInEquipSlots);

            return inventorySaveModel;
        }

        public static InventoryBaseSaveModelList ConvertToBaseInventory(object objItems)
        {
            var invListSaveModel = new InventoryBaseSaveModelList {InventoryList = new List<InventoryBaseSaveModel>()};

            var invList = (objItems as Dictionary<string, object>)["InventoryList"] as List<object>;

            foreach (var savedInventory in invList)
            {
                var inventorySavemodel = new InventoryBaseSaveModel();
                var savedInvConverted = savedInventory as Dictionary<string, object>;

                if (savedInvConverted.ContainsKey("SlotAmount"))
                    inventorySavemodel.SlotAmount = Convert.ToInt32(savedInvConverted["SlotAmount"]);

                if (savedInvConverted.ContainsKey("Items"))
                {
                    inventorySavemodel.Items = new List<ItemHolderSaveModel>();
                    var tempItemHolderObjList = savedInvConverted["Items"] as List<object>;
                    foreach (var itemHolderObj in tempItemHolderObjList)
                    {
                        var itemHolderSaveModel = new ItemHolderSaveModel();
                        var itemHolderObjDict = itemHolderObj as Dictionary<string, object>;
                        itemHolderSaveModel.SlotId = Convert.ToInt32(itemHolderObjDict["SlotId"]);
                        itemHolderSaveModel.ItemName = Convert.ToString(itemHolderObjDict["ItemName"]);
                        itemHolderSaveModel.Amount = Convert.ToInt32(itemHolderObjDict["Amount"]);
                        if (itemHolderObjDict.ContainsKey("CurrentDurability"))
                            itemHolderSaveModel.CurrentDurability = Convert.ToInt32(itemHolderObjDict["CurrentDurability"]);
                        else
                            itemHolderSaveModel.CurrentDurability = -1;
                        inventorySavemodel.Items.Add(itemHolderSaveModel);
                    }
                }
                invListSaveModel.InventoryList.Add(inventorySavemodel);
            }

            return invListSaveModel;
        }

        public static List<GroundItemSaveModel> ConvertToGroundItems(List<object> objItems)
        {
            var list = new List<GroundItemSaveModel>();
            if (objItems == null)
                return list;
            foreach (var objItem in objItems)
            {
                var inputObjData = objItem as Dictionary<string, object>;
                var GroundItem = new GroundItemSaveModel();

                GroundItem.ItemName = Convert.ToString(inputObjData["ItemName"]);
                GroundItem.CurrentHp = Convert.ToInt32(inputObjData["CurrentHp"]);
                if (inputObjData.ContainsKey("IsActive"))
                    GroundItem.IsActive = Convert.ToBoolean(inputObjData["IsActive"]);

                GroundItem.PosX = Convert.ToSingle(inputObjData["PosX"]);
                GroundItem.PosY = Convert.ToSingle(inputObjData["PosY"]);
                GroundItem.PosZ = Convert.ToSingle(inputObjData["PosZ"]);

                GroundItem.Pitch = Convert.ToSingle(inputObjData["Pitch"]);
                GroundItem.Roll = Convert.ToSingle(inputObjData["Roll"]);
                GroundItem.Yaw = Convert.ToSingle(inputObjData["Yaw"]);

                GroundItem.Dropped = Convert.ToBoolean(inputObjData["Dropped"]);
                GroundItem.Amount = Convert.ToInt32(inputObjData["Amount"]);
                if (inputObjData.ContainsKey("AmountFilled"))
                    GroundItem.AmountFilled = Convert.ToInt32(inputObjData["AmountFilled"]);

                var durability = inputObjData["Durability"];
                if (durability == null)
                    GroundItem.Durability = null;
                else
                    GroundItem.Durability = Convert.ToInt32(durability);

                if (inputObjData.ContainsKey("InventoryList") && inputObjData["InventoryList"] != null)
                    GroundItem.InventoryList = ConvertToBaseInventory(inputObjData["InventoryList"]);

                list.Add(GroundItem);
            }
            return list;
        }

        public static CarSaveModel ConvertToCarModel(object data)
        {
            var carModel = new CarSaveModel();
            var dictData = data as Dictionary<string, object>;
            if (dictData == null)
                return null;

            carModel.PosX = Convert.ToSingle(dictData["PosX"]);
            carModel.PosY = Convert.ToSingle(dictData["PosY"]);
            carModel.PosZ = Convert.ToSingle(dictData["PosZ"]);

            carModel.RotX = Convert.ToSingle(dictData["RotX"]);
            carModel.RotY = Convert.ToSingle(dictData["RotY"]);
            carModel.RotZ = Convert.ToSingle(dictData["RotZ"]);

            carModel.Petrol = Convert.ToSingle(dictData["Petrol"]);

            return carModel;
        }
    }
}
