using BattleTheMage.Combat.Attacks;

namespace BattleTheMage.Combat.Implementation.GenericAttacks;

public class Swing : IAttack
{
    public string Name() => "Swing";
    
    private readonly IWeapon _weapon;
    public IWeapon Weapon() => _weapon;

    public List<IWeaponEffect>? WeaponEffects() => null;

    public Swing(IWeapon weapon)
    {
        _weapon = weapon;
    }

    public override string ToString()
    {
        return $"[grey93]{Name()}[/] w/ {_weapon.Name()} ([cyan]{_weapon.HitModifier()}[/] hit mod, [orangered1]{_weapon.BaseDamage()}[/] {_weapon.DamageType()} dmg)";
    }
}