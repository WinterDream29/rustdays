using System;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

namespace Assets.Scripts.SaveModels
{
    [System.Serializable]
    public class MainSaveModel
    {
        private MainSaveModel(){}

        private static MainSaveModel _instatiate;
        public static MainSaveModel Instanse
        {
            get { return _instatiate ?? (_instatiate = new MainSaveModel()); }
        }

        public DateTime LogoutTs;
        public Dictionary<string, float> Stats;
        public List<float> PlayerPosition;
        public float PlayerRotation;
        public InventorySaveModel PlayerInventory;
        public List<GroundItemSaveModel> GroundItems;
        public CarSaveModel CarModel;
        public float CurrentTime { get; set; }
        public int CurrentBackpack;
        public int CurrentCurrency;
        public bool IsBuyStarterPack;
        public bool IsBuyFirst30000;
        public bool IsBuyNoAds;
        public bool InCar;
        public int CurrentTerratinId;

        public void ParseJsonStringToSaveItemModel(string jsString)
        {
           // var mainNode = JsonWriter..JSON.Instance.Parse(jsString) as Dictionary<string, object>;
            var mainNode = JsonReader.Deserialize(jsString) as Dictionary<string, object>;

            LogoutTs = DateTime.Parse(mainNode["LogoutTs"] as string);
            Stats = SaveModelConvertor.DictObjToDictFloat(mainNode["Stats"] as Dictionary<string, object>);
            PlayerPosition = GetVector3Object(mainNode, "PlayerPosition");
            PlayerRotation = Convert.ToSingle(mainNode["PlayerRotation"]);
            PlayerInventory = SaveModelConvertor.ConvertToPlayerInventory(mainNode["PlayerInventory"]);
            GroundItems = SaveModelConvertor.ConvertToGroundItems((mainNode["GroundItems"] as List<object>));
            if (mainNode.ContainsKey("CarModel"))
                CarModel = SaveModelConvertor.ConvertToCarModel(mainNode["CarModel"]);
            CurrentTime = Convert.ToSingle(mainNode["CurrentTime"]);
            CurrentCurrency = Convert.ToInt32(mainNode["CurrentCurrency"]);
            CurrentBackpack = Convert.ToInt32(mainNode["CurrentBackpack"]);
            if (mainNode.ContainsKey("CurrentTerratinId"))
                CurrentTerratinId = Convert.ToInt32(mainNode["CurrentTerratinId"]);
            IsBuyStarterPack = Convert.ToBoolean(mainNode["IsBuyStarterPack"]);
            IsBuyFirst30000 = Convert.ToBoolean(mainNode["IsBuyFirst30000"]);
            if(mainNode.ContainsKey("IsBuyNoAds"))
                IsBuyNoAds = Convert.ToBoolean(mainNode["IsBuyNoAds"]);
            if (mainNode.ContainsKey("InCar"))
                InCar = Convert.ToBoolean(mainNode["InCar"]);
        }

        private List<float> GetVector3Object(Dictionary<string, object> node, string key)
        {
            if (node.ContainsKey(key) && node[key] != null)
                return SaveModelConvertor.MassObjToListFloat(node[key] as object[]);
            return new List<float> { 0.0f, 0.0f, 0.0f };
        }
    }
}
