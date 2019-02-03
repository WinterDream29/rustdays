using Assets.Scripts.Models;
using Assets.Scripts.Ui;

namespace Assets.Scripts.UI
{
    public class UiDraggableSlot : View
    {
        public UISprite Icon;

        public void Init(HolderObject itemModel)
        {
            Icon.spriteName = itemModel.Item.IconName;
        }
    }
}
