using UnityEngine;

namespace Assets.Scripts.Ui
{
    public class MainMenu : View
    {
        public GameObject LoadIslandButton;

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            UIEventListener.Get(LoadIslandButton).onClick += OnLoadIslandClick;
        }

        private void OnLoadIslandClick(GameObject go)
        {
            
        }
    }
}
