namespace BattleTheMage.Inventory.Implementation;

public class BaseItemStack : IItemStack
{
    private readonly List<IItem> _items = new();
    public List<IItem> Items() => _items;

    public void Add(IItem item)
    {
        _items.Add(item);
    }

    public override string ToString()
    {
        return $"{_items[0].Name()} x{_items.Count}";
    }
}