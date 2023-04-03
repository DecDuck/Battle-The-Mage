using BattleTheMage.Entities;

namespace BattleTheMage.Damage;

public interface IDamageType : IFormattable
{
    // Name of the damage type
    public string Name();

    public string Color();
    // A set of multipliers, stored by weakness index
    public double[] PositiveModifierTable();
    // A set of multipliers, stored by resistance index
    public double[] NegativeModifierTable();
    // Optional on damage
    public void OnDamage(IDamageableEntity hitEntity, IAttackingEntity attackingEntity){}

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
    {
        return $"[{Color()}]{Name()}[/]";
    }


    /*
     * Indexes:
     * Weaknesses:
     *  Weak
     *  Weaker
     *  Weakest
     * Resistances:
     *  Strong
     *  Stronger
     *  Strongest
     */
}