using BattleTheMage.Damage;
using BattleTheMage.Damage.DamageTypes;
using BattleTheMage.Damage.DefaultAttacks;

namespace BattleTheMage.Inventory.Items;

public class GoblinSword : IWeaponItem
{
    public string Name() => "Goblin Sword";

    public IDamageType DamageType() => new Kinetic();

    public double BaseDamage() => 2;

    public int MaxStack() => 1;

    public IAttack DefaultAttack() => new Swing(this);
    public int HitModifier() => 3;
}