namespace Assets.Scripts.Models.Decor.Pictures
{
    public class MonaLisaPicture : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public MonaLisaPicture()
        {
            LocalizationName = "picture";
            Description = "wall_place_descr";
            IconName = "mona_lisa_picture_icon";
            IsStackable = false;
            AddDestroyReward = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/Pictures/MonaLisa/MonaLisaTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/Pictures/MonaLisa/MonaLisa";
        }
    }
}
