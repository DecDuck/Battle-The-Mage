using System.Linq.Expressions;
using BattleTheMage.Combat;
using BattleTheMage.Combat.Attacks;

namespace BattleTheMage.Entities;

public interface IAttackingEntity : IDamageableEntity
{
    public List<IAttack> Attacks();
    public int AttacksPerTurn();
    public int InitiativeModifier(int raw);
}