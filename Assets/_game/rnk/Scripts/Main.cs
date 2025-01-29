using System.Collections;
using _game.rnk.Scripts.body;
using _game.rnk.Scripts.dice;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.weapons;
using UnityEngine;

namespace _game.rnk.Scripts
{
    public class Main : MonoBehaviour
    {
        void Awake()
        {
            G.main = this;
        }

        IEnumerator Start()
        {
            if (G.run == null)
            {
                G.run = new RunState();
                
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
            }

            G.hud.Init();
            CMS.Init();

            G.ui.EnableInput();
            
            yield break;
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