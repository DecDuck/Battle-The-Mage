using System.Linq.Expressions;
using BattleTheMage.Damage;

namespace BattleTheMage.Entities;

public interface IAttackingEntity : IDamageableEntity
{
    public List<IAttack> Attacks();
    public int AttacksPerTurn();
    public int InitiativeModifier(int raw);
}