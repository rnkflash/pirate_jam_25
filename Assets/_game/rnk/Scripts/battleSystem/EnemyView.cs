using System;
using _game.rnk.Scripts.tags;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _game.rnk.Scripts.battleSystem
{
    public class EnemyView : MonoBehaviour, ISelectable, IPointerClickHandler
    {
        public Image avatarImage;
        public TMP_Text nameText;
        public TMP_Text healthText;
        public DiceZone diceZone;
        public GameObject selection;

        [NonSerialized] public EnemyState state;

        bool isSelected;
        
        public void SetState(EnemyState enemyState)
        {
            state = enemyState;
            avatarImage.sprite = state.bodyState.model.Get<TagSprite>().sprite;
            nameText.text = state.bodyState.model.Get<TagName>().loc;
            healthText.text = state.health + "/" + state.maxHealth + " +" + state.armor;

            foreach (var diceState in state.diceStates)
            {
                CreateDiceObject(diceState);
            }

            healthText.color = state.bodyState.model.Get<TagTint>().color;

            diceZone.OnClickDice += OnDiceClick;

            enemyState.diceZone = diceZone;
            enemyState.view = this;
        }
        void OnDestroy()
        {
            diceZone.OnClickDice -= OnDiceClick;
        }
        
        void OnDiceClick(DiceInteractiveObject dice)
        {
            G.main.OnDiceClickInEnemyView(this, dice);
        }

        public DiceInteractiveObject CreateDiceObject(DiceState diceState)
        {
            var instance = Instantiate(diceState.model.Get<TagPrefab>().prefab, diceZone.transform);
            instance.SetState(diceState);
            instance.moveable.targetPosition = instance.transform.position = diceZone.transform.position;
            //instance.transform.localScale = Vector3.one * 0.75f;
            diceZone.Claim(instance);
            return instance;
        }
        public bool IsSelected()
        {
            return isSelected;
        }
        public void SetSelected(bool newValue)
        {
            isSelected = newValue;
            selection.SetActive(isSelected);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            G.main.EnemyClicked(state);
        }
    }
}