using UnityEngine;

namespace Assets.Scripts
{
    public class InWaterCar : MonoBehaviour
    {
        public GameManager GameManager;

        private LayerMask _carLayer = 1 << 15;

        void OnTriggerEnter(Collider col)
        {
            if ((_carLayer.value & 1 << col.gameObject.layer) == 0)
                return;
            TurnOn();
        }
        void OnTriggerExit(Collider col)
        {
            if ((_carLayer.value & 1 << col.gameObject.layer) == 0)
                return;
            TurnOff();
        }

        void TurnOff()
        {
        }

        private void TurnOn()
        {
            if(GameManager.Player.InCar)
                GameManager.CarInteractive.Out();
        }
    }
}
