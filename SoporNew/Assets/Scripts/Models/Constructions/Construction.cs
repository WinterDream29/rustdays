namespace Assets.Scripts.Models.Constructions
{
    public enum ConstructionType
    {
        Foundation,
        Wall,
        Ceiling,
        Stairs,
        StreetStairs
    }

    public class Construction : BaseObject, IPlacement
    {
        public ConstructionType ConstructionType;
        public string PrefabTemplatePath { get; set; }
        public string PrefabPath { get; set; }
        public int HP { get; set; }

        public Construction()
        {
           // IsStackable = false;
        }
    }
}
