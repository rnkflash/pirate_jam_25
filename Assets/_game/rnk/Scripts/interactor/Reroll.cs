using System.Collections;
using _game.rnk.Scripts.battleSystem;
using UnityEngine;

namespace _game.rnk.Scripts.interactor
{
    public class Reroll: BaseInteraction, IOnReroll
    {
        public IEnumerator OnReroll()
        {
            foreach (var dice in G.battle.rollDicesZone.objects)
            {
                dice.Roll();
            }
            
            yield return new WaitForSeconds(0.2f);
        }
    }
    
    public interface IOnReroll
    {
        public IEnumerator OnReroll();
    }
}