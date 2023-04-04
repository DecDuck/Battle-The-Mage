using BattleTheMage.Combat.Attacks;

namespace BattleTheMage.Combat.Implementation.DamageTypes;

public class Cold : IDamageType
{
    public string Name() => "Cold";
    public string Color() => ConsoleColor.Cyan.ToString();

    public double[] PositiveModifierTable() => new []{ 1.1, 1.2, 1.3 };

    public double[] NegativeModifierTable() => new[] { 0.9, 0.8, 0.7 };
    
    
}