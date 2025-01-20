using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _game.rnk.Scripts.battleSystem
{
    public class EnemyView : MonoBehaviour
    {
        public Image avatarImage;
        public TMP_Text nameText;
        public TMP_Text healthText;
        public DiceZone diceZone;

        [NonSerialized] public EnemyState state;
        
        public void SetState(EnemyState enemyState)
        {
            state = enemyState;
            avatarImage.sprite = state.bodyState.model.Get<TagSprite>().sprite;
            nameText.text = state.bodyState.model.Get<TagName>().loc;
            healthText.text = "Health " + state.health;

            foreach (var diceState in state.diceStates)
            {
                CreateDiceObject(diceState);
            }

            healthText.color = state.bodyState.model.Get<TagTint>().color;

            diceZone.OnClickDice += OnDiceClick;

            enemyState.diceZone = diceZone;
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