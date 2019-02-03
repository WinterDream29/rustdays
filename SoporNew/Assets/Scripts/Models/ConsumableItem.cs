namespace Assets.Scripts.Models
{
    public class ConsumableItem : BaseObject
    {
        public float HealthEffect { get; protected set; }
        public float HungerEffect { get; protected set; }
        public float ThirstEffect { get; protected set; }
        public float EnergyEffect { get; protected set; }

        public float FuelEffect { get; protected set; }
    }
}
