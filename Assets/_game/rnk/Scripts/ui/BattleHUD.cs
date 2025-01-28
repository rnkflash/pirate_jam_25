using System;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.crawler;
using _game.tinypack.source.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _game.rnk.Scripts
{
    public class BattleHUD: MonoBehaviour
    {
        public GameObject win;
        public GameObject defeat;
        
        public TMP_Text RollButtonText;
        public Button RollButton;
        
        public TMP_Text EndTurnButtonText;
        public Button EndTurnButton;
        
        public SpriteVFX meleeHit;
        public SpriteVFX armorHit;
        public SpriteVFX healHit;
        
        public DiceZone rollDicesZone;
        
        public Transform enemiesRoot;
        [NonSerialized] public EnemyView enemyViewPrefab;
        [NonSerialized] public DiceInteractiveObject dicePrefab;

        void Awake()
        {
            enemyViewPrefab = "prefab/EnemyView".Load<EnemyView>();
            dicePrefab = "prefab/DiceInteractiveObject".Load<DiceInteractiveObject>();
        }

        void Start()
        {
            EndTurnButton.onClick.AddListener(OnClickEndTurn);
            RollButton.onClick.AddListener(OnClickRollButton);
            G.hud.battle.rollDicesZone.OnClickDice += OnDiceClickedInRollzone;
        }

        void OnDestroy()
        {
            EndTurnButton.onClick.RemoveAllListeners();
            RollButton.onClick.RemoveAllListeners();
            G.hud.battle.rollDicesZone.OnClickDice -= OnDiceClickedInRollzone;
        }
        
        public void InitBattle()
        {
            foreach (var enemyState in G.run.enemies)
            {
                CreateEnemyView(enemyState);
            }
            
            foreach (var character in G.run.characters)
            {
                foreach (var diceState in character.diceStates)
                {
                    CreateCharacterDice(character, diceState);    
                }
                
            }
        }
        void CreateCharacterDice(CharacterState character, DiceState diceState)
        {
            var instance = Instantiate(G.hud.battle.dicePrefab, character.view.diceZone.transform);
            instance.SetState(diceState);
            instance.transform.localScale = Vector3.one * 0.25f;
            instance.moveable.targetPosition = instance.transform.position = character.view.diceZone.transform.position;
            instance.transform.DOScale(1.0f, 0.15f);
            character.view.diceZone.Claim(instance);
        }

        public EnemyView CreateEnemyView(EnemyState enemyState)
        {
            var enemy = Instantiate(enemyViewPrefab, enemiesRoot);
            enemy.SetState(enemyState);
            return enemy;
        }
        
        void OnDiceClickedInRollzone(DiceInteractiveObject arg0)
        {
            G.battle.OnDiceClickedInRollzone(arg0);
        }

        public void PunchEndTurn()
        {
            G.ui.Punch(EndTurnButton.transform);
        }
    
        public void PunchRollButton()
        {
            G.ui.Punch(RollButton.transform);
        }
        
        public void DisableHud()
        {
            rollDicesZone.canDrag = false;
            EndTurnButton.interactable = false;
            RollButton.interactable = false;
        }

        public void EnableHud()
        {
            rollDicesZone.canDrag = true;
            EndTurnButton.interactable = true;
            RollButton.interactable = true;
        }
        
        void OnClickEndTurn()
        {
            PunchEndTurn();
            G.battle.EndTurnButton();
        }
    
        void OnClickRollButton()
        {
            PunchRollButton();
            G.battle.ReRollButton();
        }
        public void DisableRerollButton()
        {
            RollButton.interactable = false;
        }
    }
}