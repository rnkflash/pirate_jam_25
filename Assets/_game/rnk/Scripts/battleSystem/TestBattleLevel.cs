using System;
using System.Collections;
using System.Collections.Generic;
using _game.rnk.Scripts.artefacts;
using _game.rnk.Scripts.body;
using _game.rnk.Scripts.dice;
using _game.rnk.Scripts.weapons;

namespace _game.rnk.Scripts.battleSystem
{
    public class TestBattleLevel : CMSEntity
    {
        public TestBattleLevel()
        {
            Define<TagExecuteScript>().toExecute = Script;
        }

        IEnumerator Script()
        {
            /*G.main.HideHud();
            yield return G.ui.Say("Suddenly a pair of skeletons appear.");
            yield return G.main.SmartWait(5f);
            */
            yield return G.ui.Unsay();
            G.main.ShowHud();

            G.run.characters.Add(CreateCharacter(
                new AnimeTyan(),
                new BetrayedSword()
            ));    

            G.run.characters.Add(CreateCharacter(
                new AnimeTyan(),
                new BrokenShield()
            ));
            
            G.run.characters.Add(CreateCharacter(
                new AnimeTyan(),
                new BadBow()
            ));
            
            G.run.characters.Add(CreateCharacter(
                new AnimeTyan(),
                new UselessStaff()
            ));

            G.run.enemies.Add(new EnemyState()
            {
                health = 5,
                bodyState = new BodyState() { model = new NakedMan() }
            });
            G.run.enemies[^1].diceStates.Add(new DiceState()
            {
                model = new DiceD6(),
                owner = G.run.enemies[^1] 
            });
            G.run.enemies.Add(new EnemyState()
            {
                health = 5,
                bodyState = new BodyState() { model = new NakedMan() }
            });
            G.run.enemies[^1].diceStates.Add(new DiceState()
            {
                model = new DiceD6(),
                owner = G.run.enemies[^1] 
            });
            
            G.run.inventory.Add(new ArtefactState() { model = new TestArtefact() });
        }
        
        CharacterState CreateCharacter(BodyBase body, WeaponBase weapon)
        {
            var character = new CharacterState
            {
                health = body.Get<TagHealth>().health,
                bodyState = new BodyState() { model = body },
                weaponState = new WeaponState() { model = weapon }
            };
            character.diceStates.Add(new DiceState()
            {
                model = new DiceD6(),
                owner = character,
                artefacts = new List<ArtefactState>() { new ArtefactState() { model = new TestArtefact() } }
            });
            return character;
        }
    }

    public class TagExecuteScript : EntityComponentDefinition
    {
        public Func<IEnumerator> toExecute;
    }

    public class TagIsFinal : EntityComponentDefinition
    {
    }
}