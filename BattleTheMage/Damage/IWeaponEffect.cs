namespace BattleTheMage.Damage;

public interface IWeaponEffect
{
    public string Name();
    public double Multiplier();
    public double Additive();
    public IDamageType? OverrideDamageType();
}