namespace Assets.Scripts.Controllers
{
    public interface IConverter
    {
        bool IsBurning { get; }
        bool AutoStart { get; set; }
        void Fire();
    }
}
