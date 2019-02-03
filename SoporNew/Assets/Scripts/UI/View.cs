using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Ui
{
    public class View : MonoBehaviour
    {
        public bool IsShowing { get; protected set; }

        protected GameManager GameManager;

        public virtual void Init(GameManager gameManager)
        {
            GameManager = gameManager;
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            IsShowing = true;
        }

        public virtual IEnumerator ShowDelay(float delayTime)
        {
            IsShowing = true;
            yield return new WaitForSeconds(delayTime);
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            IsShowing = false;
        }

        public void HideImmediately()
        {
            gameObject.SetActive(false);
            IsShowing = false;
        }

        public virtual void UpdateView()
        {
            
        }

        public virtual void Destroyed()
        {

        }

        void OnDestroy()
        {
            Destroyed();
        }
    }
}
