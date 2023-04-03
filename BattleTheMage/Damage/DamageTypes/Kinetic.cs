namespace BattleTheMage.Damage.DamageTypes;

public class Kinetic : IDamageType
{
    public string Name() => "Kinetic";
    public string Color() => "grey62";

    public double[] PositiveModifierTable() => new[] { 1.0, 1.0, 1.0 };

    public double[] NegativeModifierTable() => new[] { 1.0, 1.0, 1.0 };
}