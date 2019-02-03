using Assets.Scripts.Models;
using Assets.Scripts.Ui;
using System.Collections.Generic;

namespace Assets.Scripts.UI
{
    public class InteractView : View
    {
        public List<UiSlot> Slots;

        public override void Show()
        {
            base.Show();
            GameManager.DisplayManager.CurrentInteractPanel = this;
        }

        public override void Hide()
        {
            GameManager.DisplayManager.CurrentInteractPanel = null;
            base.Hide();
            //Destroy(gameObject);
        }

        public virtual void ChageAmount(UiSlot slot, int amount)
        {

        }

        public virtual void SetItem(UiSlot slot, HolderObject itemModel)
        {

        }
    }
}
