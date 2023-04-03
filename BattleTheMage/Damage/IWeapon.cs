namespace BattleTheMage.Damage;

public interface IWeapon
{
    public string Name();
    public IDamageType DamageType();
    public double BaseDamage();
    public int HitModifier();
    public bool AutoHit() => false;
}