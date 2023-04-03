using BattleTheMage.Battle;
using BattleTheMage.Entities.Implementation;
using BattleTheMage.Entities.Monsters;

Player player = new Player();
Battle battle = new Battle(new (){new Goblin(), new Goblin()}, player);
battle.StartBattle();
