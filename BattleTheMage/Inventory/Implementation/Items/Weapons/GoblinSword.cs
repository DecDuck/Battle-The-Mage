using BattleTheMage.Combat.Attacks;
using BattleTheMage.Combat.Implementation.DamageTypes;
using BattleTheMage.Combat.Implementation.GenericAttacks;

namespace BattleTheMage.Inventory.Implementation.Items.Weapons;

public class GoblinSword : IWeaponItem
{
    public string Name() => "Goblin Sword";

    public IDamageType DamageType() => new Kinetic();

    public double BaseDamage() => 2;

    public int MaxStack() => 1;

    public IAttack DefaultAttack() => new Swing(this);
    public int HitModifier() => 3;
}