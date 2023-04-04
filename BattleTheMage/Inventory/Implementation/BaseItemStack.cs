namespace BattleTheMage.Inventory.Implementation;

public class BaseItemStack : IItemStack
{
    private IItem _item = null!;
    public IItem Item() => _item;
    private int _count;

    public int Count() => _count;

    public void Add(int amount)
    {
        _count++;
    }

    public void Setup(IItem item, int amount)
    {
        _item = item;
        _count = amount;
    }

    public override string ToString()
    {
        return $"{_item.Name()} x{_count}";
    }
}