using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class SniperRifleController : MonoBehaviour
    {
        public vp_FPPlayerEventHandler PlayerEventHandler;
        public GameManager GameManager;
        public Camera MainCamera;
        private bool _isZooming = false;
        private bool _hasZoomed = false;

        void Update()
        {
            if (PlayerEventHandler.Zoom.Active && !_isZooming)
            {
                _isZooming = true;
                StartCoroutine("ZoomSniper");
            }
            else if (!PlayerEventHandler.Zoom.Active)
            {
                GameManager.Player.MainHud.SniperZoom.SetActive(false);
                _isZooming = false;
                _hasZoomed = false;
                GetComponent<vp_FPWeapon>().WeaponModel.SetActive(true);
            }

            if (_hasZoomed)
            {
                MainCamera.fieldOfView = 6;
            }
        }

        IEnumerator ZoomSniper()
        {
            yield return new WaitForSeconds(0.3f);
            GetComponent<vp_FPWeapon>().WeaponModel.SetActive(false);
            GameManager.Player.MainHud.SniperZoom.SetActive(true);
            _hasZoomed = true;
        }
    }
}
