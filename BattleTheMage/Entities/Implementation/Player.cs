using BattleTheMage.Inventory.Implementation.Items.Armor;
using BattleTheMage.Inventory.Implementation.Items.Weapons;
namespace BattleTheMage.Entities.Implementation;

public class Player : BaseEntity
{
    public Player() : base("Player", 100)
    {
        Inventory().AttemptAddItem(new LeatherArmor());
        Inventory().AttemptAddItem(new LeatherArmor());
        Inventory().AttemptAddItem(new Shortsword());
    }

    public override int AttacksPerTurn() => 1;
}