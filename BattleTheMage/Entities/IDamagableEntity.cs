using BattleTheMage.Damage;

namespace BattleTheMage.Entities;

public interface IDamageableEntity : IEntity
{
    public double Health();
    public double MaxHealth();
    public int ArmorClass();
    public Dictionary<IDamageType, int> Weaknesses();
    public Dictionary<IDamageType, int> Resistances();
    public List<ILingeringEffect> LingeringEffects();

    public void DoHealthDelta(double health);
    public void DoMaxHealthDelta(double health);

    // We die when we die
    public bool OnDeath(IAttack attack, IAttackingEntity attackingEntity);

    public bool ApplyAttack(IAttack attack, IAttackingEntity attackingEntity) => false;
}