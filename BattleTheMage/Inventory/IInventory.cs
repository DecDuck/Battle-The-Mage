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

            if (item is IUnique itemUnique && itemStack.Item() is IUnique itemStackUnique)
            {
                string[] sharedTags = itemUnique.UniqueTags().Tags().Intersect(itemStackUnique.UniqueTags().Tags())
                    .ToArray();
                if (sharedTags.Length > 0) return false;
            }
        }

        if (ItemStacks().Count < MaxSize())
        {
            T itemStack = new T();
            itemStack.Setup(item, 1);
            ItemStacks().Add(itemStack);
            return true;
        }

        return false;
    }
}