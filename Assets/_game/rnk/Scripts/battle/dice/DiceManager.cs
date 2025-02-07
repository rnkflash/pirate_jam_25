using System;
using UnityEngine;

namespace _game.rnk.Scripts.battle.dice
{
    public class DiceManager : MonoBehaviour
    {
        public enum State
        {
            ROLL, BACK
        }
        
        public HorizontalLayoutGroup3DModified layoutGroup;

        State state;

        void Start()
        {
            GetDicesBack();
            state = State.ROLL;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (state == State.ROLL)
                {
                    RollDices();
                    state = State.BACK;
                } else if (state == State.BACK)
                {
                    GetDicesBack();
                    state = State.ROLL;
                }
            }
        }
        void GetDicesBack()
        {
            layoutGroup.enabled = true;
            foreach (var dice in layoutGroup.dices)
            {
                dice.SetState(Dice3D.State.FOLLOW);
                dice.SetFaceAnimated(dice.rollValue);
            }
        }
        void RollDices()
        {
            layoutGroup.enabled = false;
            foreach (var dice in layoutGroup.dices)
            {
                dice.SetState(Dice3D.State.FREE);
                dice.RollDice();
            }
        }
    }
}