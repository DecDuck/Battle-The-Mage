using BattleTheMage.Combat.Attacks;
using BattleTheMage.Entities;

namespace BattleTheMage.Combat.Implementation.DamageTypes;

public class Death : IDamageType
{
    public string Name() => "Death";
    public string Color() => ConsoleColor.Black.ToString();

    public double[] PositiveModifierTable() => new[] { 1.2, 1.5, 1.8 };

    public double[] NegativeModifierTable() => new[] { 0.9, 0.8, 0.5 };

    public void OnDamage(IDamageableEntity hitEntity, IAttackingEntity attackingEntity)
    {
        hitEntity.LingeringEffects().Add(new Deathtouched(hitEntity));
    }
}

public class Deathtouched : ILingeringEffect
{
    private IDamageableEntity _attackingEntity;
    
    public string Name() => "Deathtouched";

    private int _turnsRemaining = 3;

    public int TurnsRemaining() => _turnsRemaining;

    public IAttack Attack() => new DeathtouchedAttack();
    public void OnTurnTick()
    {
        _turnsRemaining--;
        _attackingEntity.DoHealthDelta(-Attack().Weapon().BaseDamage());
    }
    
    public Deathtouched(IDamageableEntity attackingEntity)
    {
        _attackingEntity = attackingEntity;
    }

    public override string ToString()
    {
        return $"{Name()} tr: {TurnsRemaining()} ak: {Attack()}";
    }
}

public class DeathtouchedAttack : IAttack
{
    public string Name() => "Deathtouched";

    public IWeapon Weapon() => new DeathtouchedWeapon();

    public List<IWeaponEffect>? WeaponEffects() => null;

    public override string ToString()
    {
        return $"[black]{Weapon().BaseDamage()} dmg[/]";
    }
}

public class DeathtouchedWeapon : IWeapon
{
    public string Name() => "Deathtouched";

    public IDamageType DamageType() => new Necrotic();

    public double BaseDamage() => 1;

    public int HitModifier() => 0;
    public bool AutoHit() => true;
}

