namespace Assets.Scripts.Models.Decor.Pictures
{
    public class MonroPicture : BaseObject, IPlacement
    {
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }

        public MonroPicture()
        {
            LocalizationName = "picture";
            Description = "wall_place_descr";
            IconName = "monro_picture_icon";
            IsStackable = false;
            AddDestroyReward = false;

            PrefabTemplatePath = "Prefabs/Items/PlacedItems/Decor/Pictures/Monro/MonroTemplate";
            PrefabPath = "Prefabs/Items/PlacedItems/Decor/Pictures/Monro/Monro";
        }
    }
}
