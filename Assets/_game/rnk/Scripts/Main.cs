using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.body;
using _game.rnk.Scripts.crawler;
using _game.rnk.Scripts.dice;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.weapons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _game.rnk.Scripts
{
    public class Main : MonoBehaviour
    {
        public List<Character> startingCharacters = new List<Character>();
        
        void Awake()
        {
            G.main = this;
        }

        IEnumerator Start()
        {
            if (G.run == null)
            {
                G.run = new RunState();
                G.run.characters = new List<CharacterState>();
                foreach (var startingCharacter in startingCharacters)
                {
                    G.run.characters.Add(CreateCharacter(startingCharacter));
                }
            }

            G.hud.Init();
            CMS.Init();

            G.ui.EnableInput();
            
            yield break;
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene(GameSettings.MAIN_SCENE);
                }
            }
        }

        CharacterState CreateCharacter(Character character)
        {
            var body = character.body.GetEntity();
            var weapon = character.weapon.GetEntity();
            
            var characterState = new CharacterState
            {
                health = body.Get<TagHealth>().health,
                maxHealth = body.Get<TagHealth>().health,
                bodyState = new BodyState() { model = body },
                weaponState = new WeaponState() { model = weapon }
            };
            
            var dices = character.body.dices.Select(diceSo =>
                new DiceState()
                {
                    owner = characterState,
                    model = diceSo.GetEntity()
                }
            ).ToList();
            characterState.diceStates.AddRange(dices);
            
            return characterState;
        }
    }
}