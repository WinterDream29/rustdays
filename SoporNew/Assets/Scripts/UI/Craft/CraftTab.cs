using UnityEngine;

namespace Assets.Scripts.UI.Craft
{
    public enum CraftCategory
    {
        Common,
        Weapons,
        Tools,
        Meds,
        Constructions,
        Clothes,
        Shop,
        None
    }

    public class CraftTab : MonoBehaviour
    {
        public CraftCategory TabType;
        public UISprite Background;
        public Vector2 BackgroundActiveSize;

        private Vector2 _startBackSize = new Vector2(110, 80);

        public void SetActive(bool isActive)
        {
            if(isActive)
                Background.SetDimensions((int)BackgroundActiveSize.x, (int)BackgroundActiveSize.y);
            else
                Background.SetDimensions((int)_startBackSize.x, (int)_startBackSize.y);
        }
    }
}
