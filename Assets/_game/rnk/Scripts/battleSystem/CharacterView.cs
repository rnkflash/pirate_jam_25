using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _game.rnk.Scripts.battleSystem
{
    public class CharacterView : MonoBehaviour
    {
        public Image avatarImage;
        public TMP_Text nameText;
        public TMP_Text healthText;
        public DiceZone diceZone;

        [NonSerialized] public CharacterState state;
        
        public void SetState(CharacterState characterState)
        {
            state = characterState;
            avatarImage.sprite = state.bodyState.model.Get<TagSprite>().sprite;
            nameText.text = state.weaponState.model.Get<TagName>().loc;
            healthText.text = "Health " + state.health;

            foreach (var diceState in state.diceStates)
            {
                CreateDiceObject(diceState);
            }

            healthText.color = state.weaponState.model.Get<TagTint>().color;

            diceZone.OnClickDice += OnDiceClick;

            characterState.diceZone = diceZone;
        }
        void OnDestroy()
        {
            diceZone.OnClickDice -= OnDiceClick;
        }
        
        void OnDiceClick(DiceInteractiveObject dice)
        {
            G.main.ReturnDiceToRollzone(dice);
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
    }
}