using Assets.Scripts.Ui;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GettingResourceItemView : View
    {
        public UISprite Icon;
        public UILabel AmountLabel;
        public UILabel NameLabel;

        public override void Show()
        {
            base.Show();
            TweenAlpha.Begin(gameObject, 0.0f, 0.0f);
            TweenAlpha.Begin(gameObject, 0.2f, 1.0f);
        }

        public override void Hide()
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(HideAlpha());
            else
                base.Hide();
        }

        private IEnumerator HideAlpha()
        {
            TweenAlpha.Begin(gameObject, 0.3f, 0.0f);
            yield return new WaitForSeconds(0.3f);
            base.Hide();
        }
    }
}
