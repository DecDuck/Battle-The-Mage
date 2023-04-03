using BattleTheMage.Inventory.Implementation;

namespace BattleTheMage.Inventory;

public interface IInventory<T> where T : IItemStack, new()
{
    public List<T> ItemStacks();
    public int MaxSize();

    public bool AttemptAddItem(IItem item)
    {
        foreach (T itemStack in ItemStacks())
        {
            if (itemStack.AttemptAdd(item))
            {
                return true;
            }
        }

        if (ItemStacks().Count < MaxSize())
        {
            T itemStack = new T();
            itemStack.Add(item);
            ItemStacks().Add(itemStack);
            return true;
        }

        return false;
    }
}