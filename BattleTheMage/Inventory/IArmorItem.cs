namespace BattleTheMage.Inventory;

public interface IArmorItem : IItem
{
    public int ArmorClassModifier();
    public int ProcessArmorClass(int raw) => raw;
}