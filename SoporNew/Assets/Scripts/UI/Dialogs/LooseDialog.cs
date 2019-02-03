using Assets.Scripts.Ui;
using UnityEngine;

namespace Assets.Scripts.UI.Dialogs
{
    public class LooseDialog : View
    {
        public GameObject ContinueButton;

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            UIEventListener.Get(ContinueButton).onClick += OnContinueClick;
        }

        private void OnContinueClick(GameObject go)
        {
            
        }
    }
}
