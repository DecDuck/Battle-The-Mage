using System.Text.RegularExpressions;
using BattleTheMage.Damage;
using BattleTheMage.Entities.Implementation;
using Spectre.Console;

namespace BattleTheMage.Battle;

public class Battle
{
    private List<BattlingBaseEntity> _creatures;

    public Battle(List<BaseEntity> monsters, Player player)
    {
        _creatures = monsters.Select(e => new BattlingBaseEntity(e)).ToList();
        _creatures.Add(new BattlingBaseEntity(player));
        _creatures.Sort((x, y) => Math.Clamp(y.Initiative - x.Initiative, -1, 1));
        for (int i = 0; i < _creatures.Count; i++)
        {
            _creatures[i].InitiativePlacement = i;
        }
    }

    public bool StartBattle()
    {
        // Player deaths will be dealt with inside the loop
        Random random = new Random();
        int round = 1;
        while (_creatures.Count > 1)
        {
            // Loops through in initiative order
            for (int i = 0; i < _creatures.Count; i++)
            {
                BattlingBaseEntity creature = _creatures[i];
                Console.Clear();
                Table table = new Table();
                table.AddColumn("#");
                table.AddColumn("Name");
                table.AddColumn("Health");
                table.AddColumn("Attacks");
                table.AddColumn("Lingering Effects");
                table.Title = new TableTitle($"Battle - {string.Join(" / ", _creatures)} - Round {round}");
                foreach (BattlingBaseEntity _c in _creatures)
                {
                    table.AddRow(
                        $"{(creature == _c ? "[green]":"")}{_c.InitiativePlacement + 1}{(creature == _c ? "[/]":"")}", 
                        $"{_c.BaseEntity.Name()}", 
                        $"{_c.BaseEntity.Health()}/{_c.BaseEntity.MaxHealth()} / AC {_c.BaseEntity.ArmorClass()}", 
                        $"{_c.BaseEntity.AttacksPerTurn()} atk(s) / turn\n {string.Join("  \n", _c.BaseEntity.Attacks())}", 
                        string.Join(" / ", _c.BaseEntity.LingeringEffects())
                    );
                }
                Spectre.Console.AnsiConsole.Write(table);
                if (creature.BaseEntity is Player player)
                {
                    string creatureSelectTarget = Spectre.Console.AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Who do you attack?")
                            .PageSize(10)
                            .MoreChoicesText("[grey]Move up and down to reveal more options[/]")
                            .AddChoices(_creatures.Where(e => !(e.BaseEntity is Player)).Select((e, i) => $"{e.BaseEntity.Name()} (#{e.InitiativePlacement+1})")));
                    // We have to do some nasty parsing of the return string
                    int creatureIndex = int.Parse(new Regex("\\(([^)]*)\\)[^(]*$").Match(creatureSelectTarget).Value.Substring(2)[..^1]) - 1;
                    string attackSelectTarget = Spectre.Console.AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("How do you attack?")
                            .PageSize(10)
                            .MoreChoicesText("[grey]Move up and down to reveal more options[/]")!
                            .AddChoices(player.Attacks().Select((e, i) => e.ToString() + $" (#{i+1})"))
                        );
                    int attackIndex = int.Parse(new Regex("\\(([^)]*)\\)[^(]*$").Match(attackSelectTarget).Value.Substring(2)[..^1]) - 1;
                    BattlingBaseEntity targetCreature = _creatures[creatureIndex];
                    IAttack targetAttack = player.Attacks()[attackIndex];

                    bool hits = false;
                    if (targetAttack.Weapon().AutoHit())
                    {
                        Spectre.Console.AnsiConsole.Markup("Your attack [yellow3]auto-hits[/]!\n");
                    }
                    else
                    {
                        int baseRoll = targetCreature.D20();
                        int modifier = targetAttack.Weapon().HitModifier();
                        hits = baseRoll + modifier >= targetCreature.BaseEntity.ArmorClass();
                        Spectre.Console.AnsiConsole.Markup($"You rolled a [cyan]{baseRoll}[/], with a [red]{modifier}[/] to hit... ");
                        Thread.Sleep(600);
                        Spectre.Console.AnsiConsole.Markup((hits
                            ? "[green]Your attack hits!"
                            : "[red]Your attack misses!") + "[/]\n");
                        Thread.Sleep(800);
                    }

                    if (hits)
                    {
                        if (targetCreature.BaseEntity.ApplyAttack(targetAttack, player))
                        {
                            // If they die
                            _creatures.RemoveAt(creatureIndex);
                            for (int j = 0; j < _creatures.Count; j++)
                            {
                                _creatures[j].InitiativePlacement = j;
                            }
                        }

                    }
                }
                else
                {
                    for (int m = 0; m < creature.BaseEntity.AttacksPerTurn(); m++)
                    {
                        // Generic creature turn
                        BattlingBaseEntity targetCreature = _creatures.First(e => e.BaseEntity is Player);
                        // Potentially add a filter?
                        List<IAttack> attacks = creature.BaseEntity.Attacks();
                        IAttack targetAttack = attacks[random.Next(0, attacks.Count-1)];
                    
                        bool hits = false;
                        if (targetAttack.Weapon().AutoHit())
                        {
                            Spectre.Console.AnsiConsole.Markup($"[grey93]${creature.BaseEntity.Name()}[/]'(s) attack [yellow3]auto-hits[/]!\n");
                        }
                        else
                        {
                            int baseRoll = targetCreature.D20();
                            int modifier = targetAttack.Weapon().HitModifier();
                            hits = baseRoll + modifier >= targetCreature.BaseEntity.ArmorClass();
                            Spectre.Console.AnsiConsole.Markup($"[grey93]{creature.BaseEntity.Name()}[/] rolled a [cyan]{baseRoll}[/], with a [red]{modifier}[/] to hit... ");
                            Thread.Sleep(600);
                            Spectre.Console.AnsiConsole.Markup((hits
                                ? "[green]Their attack hits!"
                                : "[red]Their attack misses!") + "[/]\n");
                            Thread.Sleep(800);
                        }

                        if (hits)
                        {
                            if (targetCreature.BaseEntity.ApplyAttack(targetAttack, creature.BaseEntity))
                            {
                                // If they die
                                return false;
                            }

                        }
                    }
                }
            }
            round++;
        }

        // We won!
        return true;
    }

    private class BattlingBaseEntity
    {
        public readonly BaseEntity BaseEntity;
        public readonly int Initiative;
        private readonly Random _random = new();

        public int InitiativePlacement = -1;

        public BattlingBaseEntity(BaseEntity entity)
        {
            BaseEntity = entity;
            Initiative = entity.InitiativeModifier(D20());
        }

        public override string ToString()
        {
            return BaseEntity.ToString();
        }

        public int D20()
        {
            return _random.Next(1, 20);
        }
    }
}

