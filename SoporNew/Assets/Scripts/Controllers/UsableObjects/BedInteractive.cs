using Assets.Scripts.UI.Interactive;

namespace Assets.Scripts.Controllers.UsableObjects
{
    public class BedInteractive : UsableObject
    {
        private BedPanel _panel;
        public override void Use(GameManager gameManager)
        {
            if (gameManager.DisplayManager.CurrentInteractPanel != null)
                return;

            base.Use(gameManager);

            if (_panel == null)
            {
                _panel = NGUITools.AddChild(gameManager.UiRoot.gameObject, InteractPanelPrefab).GetComponent<BedPanel>();
                _panel.Init(GameManager);
                _panel.Hide();
            }
            if (!_panel.IsShowing)
                StartCoroutine(_panel.ShowDelay(0.2f));
        }
    }
}
