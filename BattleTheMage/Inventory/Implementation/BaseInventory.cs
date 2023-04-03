namespace BattleTheMage.Inventory.Implementation;

public class BaseInventory : IInventory<BaseItemStack>
{
    private readonly List<BaseItemStack> _itemStacks = new();
    public List<BaseItemStack> ItemStacks() => _itemStacks;
    public int MaxSize() => 9;

    public override string ToString()
    {
        return string.Join(" / ", _itemStacks);
    }
}