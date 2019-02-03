namespace Assets.Scripts.Models.Decor.Pictures
{
    public class Picture4 : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public Picture4()
        {
            LocalizationName = "picture";
            Description = "wall_place_descr";
            IconName = "picture_4";
            IsStackable = false;
            AddDestroyReward = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/Pictures/Picture4/PictureTemplate4";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/Pictures/Picture4/Picture4";
        }
    }
}
