namespace Assets.Scripts.Models.Decor.Pictures
{
    public class Picture2 : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public Picture2()
        {
            LocalizationName = "picture";
            Description = "wall_place_descr";
            IconName = "picture_2";
            IsStackable = false;
            AddDestroyReward = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/Pictures/Picture2/PictureTemplate2";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/Pictures/Picture2/Picture2";
        }
    }
}
