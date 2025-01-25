using System;
using System.Collections;
using _game.rnk.Scripts.body;
using _game.rnk.Scripts.dice;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.weapons;

namespace _game.rnk.Scripts.battleSystem
{
    public class NextBattleLevel : CMSEntity
    {
        public NextBattleLevel()
        {
            Define<tags.TagExecuteScript>().toExecute = Script;
        }

        IEnumerator Script()
        {
            G.audio.Play<SFX_Win>();
            yield return G.ui.Say("Suddenly another two kolbaser appear...");
            yield return G.main.SmartWait(5f);
            
            yield return G.ui.Unsay();
            
            var enemyModel = new NakedMan();
            G.run.enemies.Add(new EnemyState()
            {
                health = enemyModel.Get<TagHealth>().health,
                maxHealth = enemyModel.Get<TagHealth>().health,
                bodyState = new BodyState() { model = enemyModel }
            });
            G.run.enemies[^1].diceStates.Add(new DiceState()
            {
                model = new AggressiveDice(),
                owner = G.run.enemies[^1] 
            });
            G.run.enemies.Add(new EnemyState()
            {
                health = enemyModel.Get<TagHealth>().health,
                maxHealth = enemyModel.Get<TagHealth>().health,
                bodyState = new BodyState() { model = enemyModel }
            });
            G.run.enemies[^1].diceStates.Add(new DiceState()
            {
                model = new AggressiveDice(),
                owner = G.run.enemies[^1] 
            });

            foreach (var enemy in G.run.enemies)
            {
                enemy.dead = false;
                enemy.health = enemy.maxHealth;
            }
            
            //G.run.inventory.Add(new ArtefactState() { model = new TestArtefact() });
        }
    }
}