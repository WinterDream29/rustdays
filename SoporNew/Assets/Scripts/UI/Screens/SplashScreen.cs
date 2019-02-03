using Assets.Scripts.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Screens
{
    public class SplashScreen : MonoBehaviour
    {
        public List<GameObject> HelpObjects;
        public List<GameObject> QuoteObjects;
        public void Show()
        {
            foreach(var ho in HelpObjects)
                ho.SetActive(false);

            foreach (var qo in QuoteObjects)
                qo.SetActive(false);

            var randValue = Random.Range(0, HelpObjects.Count);
            HelpObjects[randValue].SetActive(true);

            randValue = Random.Range(0, QuoteObjects.Count);
            QuoteObjects[randValue].SetActive(true);

            gameObject.SetActive(true);
            TweenAlpha.Begin(gameObject, 0.0f, 1.0f);
            StartCoroutine(DelayHide());
        }

        private IEnumerator DelayHide()
        {
            yield return new WaitForSeconds(0.5f);
            SoundManager.PlaySFX(WorldConsts.AudioConsts.Back1);
            yield return new WaitForSeconds(5.5f);
            TweenAlpha.Begin(gameObject, 1.0f, 0.0f);
            yield return new WaitForSeconds(1.0f);
            gameObject.SetActive(false);
        }
    }
}
