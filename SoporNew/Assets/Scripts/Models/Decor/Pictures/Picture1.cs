namespace Assets.Scripts.Models.Decor.Pictures
{
    public class Picture1 : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public Picture1()
        {
            LocalizationName = "picture";
            Description = "wall_place_descr";
            IconName = "picture_1";
            IsStackable = false;
            AddDestroyReward = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/Pictures/Picture1/PictureTemplate1";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/Pictures/Picture1/Picture1";
        }
    }
}
