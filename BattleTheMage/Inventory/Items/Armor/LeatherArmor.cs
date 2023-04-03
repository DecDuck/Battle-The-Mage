namespace BattleTheMage.Inventory.Items;

public class LeatherArmor : IArmorItem
{
    public string Name() => "Leather Armor";

    public int MaxStack() => 1;

    public int ArmorClassModifier() => 2;
}