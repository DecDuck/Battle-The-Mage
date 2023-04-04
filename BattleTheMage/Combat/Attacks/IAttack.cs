namespace BattleTheMage.Combat.Attacks;

public interface IAttack
{
    public string Name();
    public IWeapon Weapon();
    public List<IWeaponEffect>? WeaponEffects();
}