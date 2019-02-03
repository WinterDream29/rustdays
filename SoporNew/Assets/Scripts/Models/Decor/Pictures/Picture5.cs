namespace Assets.Scripts.Models.Decor.Pictures
{
    public class Picture5 : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public Picture5()
        {
            LocalizationName = "picture";
            Description = "wall_place_descr";
            IconName = "picture_5";
            IsStackable = false;
            AddDestroyReward = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/Pictures/Picture5/PictureTemplate5";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/Pictures/Picture5/Picture5";
        }
    }
}
