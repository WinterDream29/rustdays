using Assets.Scripts.Models;
using System.Collections.Generic;

namespace Assets.Scripts.Controllers
{
    public interface IInventoryThing
    {
        List<InventoryBase> GetInventoryList();
        void SetInventoryList(List<InventoryBase> inventoryList);
    }
}
