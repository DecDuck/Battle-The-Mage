namespace BattleTheMage.Combat.Attacks;

public interface ILingeringEffect
{
    public string Name();
    public int TurnsRemaining();
    public IAttack Attack();
    public void OnTurnTick();
}