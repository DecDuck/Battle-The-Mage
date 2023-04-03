using BattleTheMage.Entities;

namespace BattleTheMage.Damage.DamageTypes;

public class Fire : IDamageType
{
    public string Name() => "Fire";
    public string Color() => ConsoleColor.DarkRed.ToString();

    public double[] PositiveModifierTable() => new[]{ 1.1, 1.1, 3 };

    public double[] NegativeModifierTable() => new[] { 0.5, 0.3, 0 };
    
    public void OnDamage(IDamageableEntity hitEntity)
    {
        Burning burning = new Burning();
        if (hitEntity.LingeringEffects().Count(e => e.Name() == burning.Name()) == 0)
        {
            hitEntity.LingeringEffects().Add(burning);
        }
    }
}

public class Burning : ILingeringEffect
{
    public string Name() => "Burning";

    private int _turnsRemaining = 3;
    public int TurnsRemaining() => _turnsRemaining;

    public IAttack Attack() => new BurningAttack();
    public void OnTurnTick()
    {
        _turnsRemaining--;
    }
}

public class BurningAttack : IAttack
{
    public string Name() => "Burning";

    public IWeapon Weapon() => new BurningWeapon();

    public List<IWeaponEffect>? WeaponEffects() => null;
}

public class BurningWeapon : IWeapon
{
    public string Name() => "Burning";

    public IDamageType DamageType() => new Fire();

    public double BaseDamage() => 2;
    public int HitModifier() => 0;
    public bool AutoHit() => true;
}

