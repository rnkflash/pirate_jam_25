using System;
using _game.rnk.Scripts.tags;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _game.rnk.Scripts.battleSystem
{
    public class CharacterView : MonoBehaviour, IPointerClickHandler
    {
        public Image avatarImage;
        public TMP_Text nameText;
        public TextMeshProUGUI healthText;
        public DiceZone diceZone;
        public Damageable damageable;

        [NonSerialized] public CharacterState state;
        
        public void SetState(CharacterState characterState)
        {
            state = characterState;
            avatarImage.sprite = state.bodyState.model.Get<TagSprite>().sprite;
            //nameText.text = state.weaponState.model.Get<TagName>().loc;
            healthText.text = state.health + "/" + state.maxHealth + " +" + state.armor;

            

            //nameText.color = state.weaponState.model.Get<TagTint>().color;
            
            damageable.SetState(state);
            healthText.color = state.weaponState.model.Get<TagTint>().color;

            diceZone.OnClickDice += OnDiceClick;

            characterState.diceZone = diceZone;
            characterState.view = this;
        }

        public void EnableBattleHUD()
        {
            foreach (var diceState in state.diceStates)
            {
                CreateDiceObject(diceState);
            }
        }

        void OnDestroy()
        {
            diceZone.OnClickDice -= OnDiceClick;
        }
        
        void OnDiceClick(DiceInteractiveObject dice)
        {
            G.battle.OnDiceClickInCharacterView(this, dice);
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
        public void OnPointerClick(PointerEventData eventData)
        {
            G.battle.CharacterClicked(state);
        }
    }
}