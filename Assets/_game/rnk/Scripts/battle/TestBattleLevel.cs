using System.Collections;
using System.Collections.Generic;
using _game.rnk.Scripts.artefacts;
using _game.rnk.Scripts.body;
using _game.rnk.Scripts.dice;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.weapons;

namespace _game.rnk.Scripts.battleSystem
{
    public class TestBattleLevel : CMSEntity
    {
        public TestBattleLevel()
        {
            Define<tags.TagExecuteScript>().toExecute = Script;
        }

        IEnumerator Script()
        {
            /*G.main.HideHud();
            yield return G.ui.Say("Suddenly a pair of skeletons appear.");
            yield return G.main.SmartWait(5f);
            */
            yield return G.ui.Unsay();
            G.battle.ShowHud();

            G.run.characters.Add(CreateCharacter(
                new AnimeTyan(),
                new BetrayedSword(),
                new NormalAggressiveDice()
            ));    

            G.run.characters.Add(CreateCharacter(
                new AnimeTyan(),
                new BrokenShield(),
                new DiceD6()
            ));
            
            G.run.characters.Add(CreateCharacter(
                new AnimeTyan(),
                new BadBow(),
                new DiceD6()
            ));
            
            G.run.characters.Add(CreateCharacter(
                new AnimeTyan(),
                new UselessStaff(),
                new NormalHealerDice()
            ));

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

            var priestModel = new Healer();
            G.run.enemies.Add(new EnemyState()
            {
                health = priestModel.Get<TagHealth>().health,
                maxHealth = priestModel.Get<TagHealth>().health,
                bodyState = new BodyState() { model = priestModel }
            });
            G.run.enemies[^1].diceStates.Add(new DiceState()
            {
                model = new HealerDice(),
                owner = G.run.enemies[^1] 
            });

        }
        
        CharacterState CreateCharacter(BodyBase body, WeaponBase weapon, DiceBase dice)
        {
            var character = new CharacterState
            {
                health = body.Get<TagHealth>().health,
                maxHealth = body.Get<TagHealth>().health,
                bodyState = new BodyState() { model = body },
                weaponState = new WeaponState() { model = weapon }
            };
            character.diceStates.Add(new DiceState()
            {
                model = dice,
                owner = character
            });
            return character;
        }
    }
}