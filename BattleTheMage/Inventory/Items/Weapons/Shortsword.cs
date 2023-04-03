using BattleTheMage.Damage;
using BattleTheMage.Damage.DamageTypes;
using BattleTheMage.Damage.DefaultAttacks;

namespace BattleTheMage.Inventory.Items;

public class Shortsword : IWeaponItem
{
    public string Name() => "Shortsword";

    public int MaxStack() => 1;
    
    public IDamageType DamageType() => new Kinetic();

    public double BaseDamage() => 5;

    public IAttack DefaultAttack() => new Swing(this);

    public int HitModifier() => 3;
}