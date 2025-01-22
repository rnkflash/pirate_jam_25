using System.Collections;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.dice.face;
using _game.rnk.Scripts.tags;
using UnityEngine;

namespace _game.rnk.Scripts.interactor
{
    public class PlayDice : BaseInteraction, IOnPlayDice
    {
        public IEnumerator OnPlayDice(DiceState state)
        {
            yield break;
        }
    }
    
    public interface IOnPlayDice
    {
        public IEnumerator OnPlayDice(DiceState state);
    }
}