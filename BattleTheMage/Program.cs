using BattleTheMage.Combat.Battle;
using BattleTheMage.Entities.Implementation;
using BattleTheMage.Entities.Implementation.Monsters;

Player player = new Player();
Battle battle = new Battle(new (){new Goblin(), new Goblin()}, player);
battle.StartBattle();
