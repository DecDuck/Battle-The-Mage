namespace BattleTheMage.Damage.DamageTypes;

public class Necrotic : IDamageType
{
    public string Name() => "Necrotic";
    public string Color() => ConsoleColor.Black.ToString();

    public double[] PositiveModifierTable() => new[] { 1.0, 1.0, 1.0 };

    public double[] NegativeModifierTable() => new[] { 1.0, 1.0, 1.0 };
}