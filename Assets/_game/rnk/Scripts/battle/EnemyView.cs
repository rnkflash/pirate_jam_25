using System;
using _game.rnk.Scripts.crawler;
using _game.rnk.Scripts.tags;
using DG.Tweening;
using TMPro;
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

        void Awake()
        {
            followTarget = GetComponent<AlwaysFollowWorldSpaceObject>();
            followTarget.enabled = false;
            followTarget.followTarget = null;
        }

        public void SetState(EnemyState enemyState)
        {
            followTarget.followTarget = enemyState.uiPos;
            followTarget.enabled = true;
            
            state = enemyState;
            nameText.text = state.bodyState.model.Get<TagName>().loc;

            foreach (var diceState in state.diceStates)
            {
                CreateDiceObject(diceState);
            }
            damageable.SetState(state);

            diceZone.OnClickDice += OnDiceClick;
            diceZone.canDrag = false;

            enemyState.diceZone = diceZone;
            enemyState.view = this;
        }
        void OnDestroy()
        {
            diceZone.OnClickDice -= OnDiceClick;
        }
        
        void OnDiceClick(DiceInteractiveObject dice)
        {
            G.battle.OnDiceClickInEnemyView(this, dice);
        }

        void CreateDiceObject(DiceState diceState)
        {
            var instance = Instantiate(G.hud.battle.dicePrefab, diceZone.transform);
            instance.SetState(diceState);
            instance.transform.localScale = Vector3.one * 0.25f;
            instance.moveable.targetPosition = instance.transform.position = diceZone.transform.position;
            instance.transform.DOScale(1.0f, 0.15f);
            diceZone.Claim(instance);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            G.battle.EnemyClicked(state);
        }
    }
}