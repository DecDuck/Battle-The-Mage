namespace BattleTheMage.Inventory;

public interface IItemStack
{
    public IItem Item();
    public int Count();
    public void Add(int amount);
    public void Setup(IItem item, int amount);

    public bool AttemptAdd(IItem item)
    {
        if (item.MaxStack() < Count() && Item().Name() == item.Name())
        {
            Add(1);
            return true;
        }
        return false;
    }
}