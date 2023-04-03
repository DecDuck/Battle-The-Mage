namespace BattleTheMage.Inventory;

public interface IItemStack
{
    public List<IItem> Items();
    public void Add(IItem item);

    public bool AttemptAdd(IItem item)
    {
        if (item.MaxStack() < Items().Count && Items()[0].Name() == item.Name())
        {
            Add(item);
            return true;
        }
        return false;
    }
}