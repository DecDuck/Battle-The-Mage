using BattleTheMage.Damage;

namespace BattleTheMage.Inventory;

public interface IWeaponItem : IItem, IWeapon
{
    public new string Name();
    public IAttack DefaultAttack();
}