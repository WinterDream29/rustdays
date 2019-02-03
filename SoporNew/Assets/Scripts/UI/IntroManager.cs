using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class IntroManager : MonoBehaviour 
    {
        public GameObject Coffin;
        public GameObject CoffinInGame;
        public Transform CoffinTarget;
        public Camera IntroCamera;
        public GameObject IntroUiObject;
        public List<GameObject> TextObjects;
        public void Run(GameManager gameManager)
        {
            gameManager.Player.FpsCamera.enabled = false;
            IntroCamera.gameObject.SetActive(true);
            Coffin.SetActive(true);
            CoffinInGame.SetActive(false);
            gameManager.Player.MainHud.gameObject.SetActive(false);
            IntroUiObject.SetActive(true);
            StartCoroutine(RunCor(gameManager));
        }

        private IEnumerator RunCor(GameManager gameManager)
        {
            //foreach (var obj in TextObjects)
            //    TweenAlpha.Begin(obj, 0.0f, 0.0f);

            yield return new WaitForSeconds(6.0f);

            TweenPosition.Begin(Coffin, 40.0f, CoffinTarget.position);

            //for (int i = 0; i < TextObjects.Count; i++)
            //{
            //    if(i > 0)
            //    {
            //        TweenAlpha.Begin(TextObjects[i - 1], 0.5f, 0.0f);
            //        yield return new WaitForSeconds(1.0f);
            //    }
            //    TweenAlpha.Begin(TextObjects[i], 0.5f, 1.0f);
            //    yield return new WaitForSeconds(6.0f);
            //}

            //TweenAlpha.Begin(TextObjects[TextObjects.Count - 1], 0.5f, 0.0f);

            yield return new WaitForSeconds(12.0f);

            gameManager.DisplayManager.ShowPlayerDeadSplash();

            yield return new WaitForSeconds(3.0f);
            SoundManager.PlaySFX("MaceImpact03");
            yield return new WaitForSeconds(0.1f);
            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerHurt);

            yield return new WaitForSeconds(1.0f);
            SoundManager.PlaySFX("MaceImpact03");
            yield return new WaitForSeconds(0.1f);
            SoundManager.PlaySFX(WorldConsts.AudioConsts.PlayerHurt);

            yield return new WaitForSeconds(0.8f);
            SoundManager.PlaySFX("MaceImpact03");
            yield return new WaitForSeconds(0.1f);
            SoundManager.PlaySFX(WorldConsts.AudioConsts.Destroy);

            IntroCamera.gameObject.SetActive(false);
            gameManager.Player.FpsCamera.enabled = true;
            Coffin.SetActive(false);
            CoffinInGame.SetActive(true);
            gameManager.Player.MainHud.gameObject.SetActive(true);
            IntroUiObject.SetActive(false);
            yield return new WaitForSeconds(1.0f);

            gameManager.DisplayManager.HidePlayerDeadSplash();

            yield return new WaitForSeconds(10.0f);
            gameManager.Tutorial.Run(gameManager);
        }
    }
}
