namespace Assets.Scripts.Models.Decor.Pictures
{
    public class Picture3 : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public Picture3()
        {
            LocalizationName = "picture";
            Description = "wall_place_descr";
            IconName = "picture_3";
            IsStackable = false;
            AddDestroyReward = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/Pictures/Picture3/PictureTemplate3";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/Pictures/Picture3/Picture3";
        }
    }
}
