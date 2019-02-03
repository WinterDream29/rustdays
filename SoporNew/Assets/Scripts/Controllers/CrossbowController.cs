using System.Collections;
using Assets.Scripts.Controllers.Fauna;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Ammo;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public enum CrossbowState
    {
        Ready,
        Reload,
        Shoot
    }

    public class CrossbowController : MonoBehaviour
    {
        public GameObject Arrow;
        public Animation Animation;

        public CrossbowState CurrentState = CrossbowState.Ready;

        private GameManager _gameManager;

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Shoot()
        {
            if (CurrentState == CrossbowState.Ready)
            {
                var amountArrow = _gameManager.PlayerModel.Inventory.GetAmount(typeof(CrossbowArrow));
                if (amountArrow > 0)
                {
                    CurrentState = CrossbowState.Shoot;
                    if (!Arrow.activeSelf)
                        Arrow.SetActive(true);
                    _gameManager.PlayerModel.Inventory.UseItem(_gameManager, typeof(CrossbowArrow));
                    Animation.Play("Shoot");
                    SoundManager.PlaySFX(WorldConsts.AudioConsts.BowRelease);
                    StartCoroutine(CheckShoot());
                }
                else
                {
                    _gameManager.Player.MainHud.ShowHudText(Localization.Get("no_arrows"), HudTextColor.Red);
                }
            }
        }

        private IEnumerator CheckShoot()
        {
            yield return new WaitForSeconds(0.3f);

            var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            int waterLayerMask = 1 << 4;//берем слой воды
            waterLayerMask = ~waterLayerMask;//инвентируем, теперь в переменной все слои крое воды
            if (Physics.Raycast(ray, out hit, 150f, waterLayerMask))
            {
                var distance = Vector3.Distance(transform.position, hit.collider.transform.position);
                var precent = 100;
                if (distance > 100)
                    precent = 60;
                else if (distance > 70)
                    precent = 70;
                else if (distance > 50)
                    precent = 80;

                var rand = Random.Range(0, 100);
                if (rand < precent)
                {
                    var enemy = hit.collider.gameObject.GetComponent<AnimalColliderLink>();
                    if (enemy != null)
                    {
                        var item = BaseObjectFactory.GetItem(typeof(CrossbowArrow));
                        enemy.SetDamage(item.Damage);
                    }
                }
            }

            var amountArrow = _gameManager.PlayerModel.Inventory.GetAmount(typeof(CrossbowArrow));
            if (amountArrow > 0)
                Reload();
            else
            {
                CurrentState = CrossbowState.Ready;  
            }
        }

        private void Reload()
        {
            CurrentState = CrossbowState.Reload;
            Animation.Play("Reload");
            SoundManager.PlaySFX(WorldConsts.AudioConsts.BowString, false, 0.3f);
        }

        public void Reloaded()
        {
            CurrentState = CrossbowState.Ready;       
        }
    }
}
