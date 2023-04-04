using BattleTheMage.Combat.Attacks;
using BattleTheMage.Combat.Implementation.DamageTypes;
using BattleTheMage.Combat.Implementation.GenericAttacks;

namespace BattleTheMage.Inventory.Implementation.Items.Weapons;

public class Shortsword : IWeaponItem
{
    public string Name() => "Shortsword";

    public int MaxStack() => 1;
    
    public IDamageType DamageType() => new Kinetic();

    public double BaseDamage() => 5;

    public IAttack DefaultAttack() => new Swing(this);

    public int HitModifier() => 3;
}