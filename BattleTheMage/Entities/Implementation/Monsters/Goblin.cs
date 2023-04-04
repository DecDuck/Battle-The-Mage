using BattleTheMage.Inventory.Implementation.Items.Weapons;

namespace BattleTheMage.Entities.Implementation.Monsters;

public class Goblin : BaseEntity
{
    public Goblin() : base("Goblin", 10)
    {
        // Adds a Goblin Sword, which also allows this creature to attack
        Inventory().AttemptAddItem(new GoblinSword());
    }

    public override int AttacksPerTurn() => 1;

    public new int InitiativeModifier(int raw)
    {
        return raw + 1;
    }
}