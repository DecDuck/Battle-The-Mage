using BattleTheMage.Combat;
using BattleTheMage.Combat.Attacks;

namespace BattleTheMage.Inventory;

public interface IWeaponItem : IItem, IWeapon
{
    public new string Name();
    public IAttack DefaultAttack();
}