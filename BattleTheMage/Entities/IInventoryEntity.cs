using BattleTheMage.Inventory;

namespace BattleTheMage.Entities;

public interface IInventoryEntity<T> : IEntity where T : IItemStack, new()
{
    public IInventory<T> Inventory();
}