using System;
using System.Collections.Generic;
using _game.rnk.Scripts.crawler;
using _game.rnk.Scripts.tags;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _game.rnk.Scripts.battleSystem
{
    public class EnemyView : MonoBehaviour, IPointerClickHandler
    {
        public TMP_Text nameText;
        public DiceZone diceZone;
        public Damageable damageable;
        AlwaysFollowWorldSpaceObject followTarget;

        [NonSerialized] public EnemyState state;

        List<DiceInteractiveObject> dices;

        void Awake()
        {
            followTarget = GetComponent<AlwaysFollowWorldSpaceObject>();
            followTarget.enabled = false;
            followTarget.followTarget = null;

            damageable.OnDead += OnDead;
        }

        void OnDestroy()
        {
            diceZone.OnClickDice -= OnDiceClick;
            damageable.OnDead -= OnDead;
        }
        
        void OnDead()
        {
            state.objInScene.graphic.SetActive(false);
        }

        public void SetState(EnemyState enemyState)
        {
            followTarget.followTarget = enemyState.objInScene.uiPos;
            followTarget.enabled = true;
            
            state = enemyState;
            nameText.text = state.bodyState.model.Get<TagName>().loc;

            dices = new List<DiceInteractiveObject>();
            foreach (var diceState in state.diceStates)
            {
                dices.Add(CreateDiceObject(diceState));
            }
            damageable.SetState(state);

            diceZone.OnClickDice += OnDiceClick;
            diceZone.canDrag = false;

            state.diceZone = diceZone;
            state.view = this;
        }

        public void FreeState()
        {
            followTarget.followTarget = null;
            followTarget.enabled = false;

            state.diceStates.Clear();
            foreach (var dice in dices)
            {
                dice.FreeState();
                Destroy(dice.gameObject);
            }
            dices.Clear();

            state.diceZone = null;
            state.view = null;
            state = null;
            damageable.SetState(null);
        }


        void OnDiceClick(DiceInteractiveObject dice)
        {
            G.battle.OnDiceClickInEnemyView(this, dice);
        }

        DiceInteractiveObject CreateDiceObject(DiceState diceState)
        {
            var instance = Instantiate(G.hud.battle.dicePrefab, diceZone.transform);
            instance.SetState(diceState);
            /*instance.transform.localScale = Vector3.one * 0.25f;
            instance.transform.DOScale(1.0f, 0.15f);*/
            instance.moveable.targetPosition = instance.transform.position = diceZone.transform.position;
            diceZone.Claim(instance);
            return instance;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            G.battle.EnemyClicked(state);
        }
    }
}