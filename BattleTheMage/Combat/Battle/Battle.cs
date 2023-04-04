using System.Text.RegularExpressions;
using BattleTheMage.Combat.Attacks;
using BattleTheMage.Entities;
using BattleTheMage.Entities.Implementation;
using Spectre.Console;

namespace BattleTheMage.Combat.Battle;

public class Battle
{
    private List<BattlingBaseEntity> _creatures;
    private readonly Random _random = new();

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
                PrintTable(creature, round);
                foreach (ILingeringEffect lingeringEffect in creature.BaseEntity.LingeringEffects().ToList())
                {
                    lingeringEffect.OnTurnTick();
                    if (lingeringEffect.TurnsRemaining() <= 0)
                    {
                        creature.BaseEntity.LingeringEffects().Remove(lingeringEffect);
                    }
                }
                if (creature.BaseEntity is Player player)
                {
                    for (int m = 0; m < player.AttacksPerTurn(); m++)
                    {
                        string creatureSelectTarget = Spectre.Console.AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title($"Attack {m+1}/{player.AttacksPerTurn()} | Who do you attack?")
                                .PageSize(10)
                                .MoreChoicesText("[grey]Move up and down to reveal more options[/]")
                                .AddChoices(_creatures.Where(e => !(e.BaseEntity is Player)).Select((e, i) => $"{e.BaseEntity.Name()} (#{e.InitiativePlacement+1})")));
                        // We have to do some nasty parsing of the return string
                        int creatureIndex = int.Parse(new Regex("\\(([^)]*)\\)[^(]*$").Match(creatureSelectTarget).Value.Substring(2)[..^1]) - 1;
                        string attackSelectTarget = Spectre.Console.AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title($"Attack {m+1}/{player.AttacksPerTurn()} | How do you attack?")
                                .PageSize(10)
                                .MoreChoicesText("[grey]Move up and down to reveal more options[/]")!
                                .AddChoices(player.Attacks().Select((e, i) => e.ToString() + $" (#{i+1})"))
                        );
                        int attackIndex = int.Parse(new Regex("\\(([^)]*)\\)[^(]*$").Match(attackSelectTarget).Value.Substring(2)[..^1]) - 1;
                        BattlingBaseEntity targetCreature = _creatures[creatureIndex];
                        IAttack targetAttack = player.Attacks()[attackIndex];

                        DoAttack(player, targetCreature.BaseEntity, creatureIndex, targetAttack);
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
                    
                        if (DoAttack(creature.BaseEntity, targetCreature.BaseEntity, 0, targetAttack))
                        {
                            return false;
                        }
                    }
                }
            }
            round++;
        }

        // We won!
        return true;
    }

    // Returns true if creature dies
    private bool DoAttack(IAttackingEntity attackingEntity, IDamageableEntity attackedEntity, int attackedEntityIndex, IAttack attack)
    {
        Spectre.Console.AnsiConsole.Markup($"[grey93]{attackingEntity.Name()}[/] attacks [cyan]{attackedEntity.Name()}[/]...\n");
        bool hits = false;
        if (attack.Weapon().AutoHit())
        {
            Spectre.Console.AnsiConsole.Markup($"The attack [yellow3]auto-hits[/]!\n");
        }
        else
        {
            int baseRoll = _random.Next(1, 20);
            int modifier = attack.Weapon().HitModifier();
            hits = baseRoll + modifier >= attackedEntity.ArmorClass();
            Spectre.Console.AnsiConsole.Markup($"[grey93]{attackingEntity.Name()}[/] rolled a [cyan]{baseRoll}[/], with a [red]{modifier}[/] to hit... ");
            Thread.Sleep(600);
            Spectre.Console.AnsiConsole.Markup((hits
                ? $"[green]The attack hits! [{attack.Weapon().DamageType().Color()}]{attack.Weapon().BaseDamage()} damage[/]!"
                : "[red]The attack misses!") + "[/]\n");
            Thread.Sleep(800);
        }

        if (!hits) return false;
        if (!attackedEntity.ApplyAttack(attack, attackingEntity)) return false;
        // If they die
        _creatures.RemoveAt(attackedEntityIndex);
        for (int j = 0; j < _creatures.Count; j++)
        {
            _creatures[j].InitiativePlacement = j;
        }

        return true;
    }

    private void PrintTable(BattlingBaseEntity creature, int round)
    {
        Console.Clear();
        Table table = new Table();
        table.AddColumn("#");
        table.AddColumn("Name");
        table.AddColumn("Health");
        table.AddColumn("Attacks");
        table.AddColumn("Lingering Effects");
        table.Title = new TableTitle($"Battle - {string.Join(" / ", _creatures)} - Round {round}");
        foreach (BattlingBaseEntity c in _creatures)
        {
            table.AddRow(
                $"{(creature == c ? "[green]":"")}{c.InitiativePlacement + 1}{(creature == c ? "[/]":"")}", 
                $"{c.BaseEntity.Name()}", 
                $"{c.BaseEntity.Health()}/{c.BaseEntity.MaxHealth()} / AC {c.BaseEntity.ArmorClass()}", 
                $"{c.BaseEntity.AttacksPerTurn()} atk(s) / turn\n {string.Join("  \n", c.BaseEntity.Attacks())}", 
                string.Join(" / ", c.BaseEntity.LingeringEffects())
            );
        }
        Spectre.Console.AnsiConsole.Write(table);
    }

    private class BattlingBaseEntity
    {
        public readonly BaseEntity BaseEntity;
        public readonly int Initiative;
        public int InitiativePlacement = -1;

        public BattlingBaseEntity(BaseEntity entity)
        {
            BaseEntity = entity;
            Initiative = entity.InitiativeModifier(new Random().Next(1, 20));
        }

        public override string ToString()
        {
            return BaseEntity.ToString();
        }
    }
}

