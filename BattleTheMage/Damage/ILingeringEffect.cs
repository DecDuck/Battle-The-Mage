namespace BattleTheMage.Damage;

public interface ILingeringEffect : IFormattable
{
    public string Name();
    public int TurnsRemaining();
    public IAttack Attack();
    public void OnTurnTick();

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
    {
        return $"{Name()} tr: {TurnsRemaining()} ak: {Attack()}";
    }
}