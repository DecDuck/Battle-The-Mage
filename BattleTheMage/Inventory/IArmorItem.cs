namespace BattleTheMage.Inventory;

public interface IArmorItem : IItem, ITag, IUnique
{
    public int ArmorClassModifier();
    public int ProcessArmorClass(int raw) => raw;
    public bool OnACHit() => true;

    List<string> ITag.Tags() => new() { "Armor" };
    ITag IUnique.UniqueTags() => this;
}