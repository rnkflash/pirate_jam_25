using System.Collections;
using System.Linq;
using _game.rnk.Scripts.battleSystem;
using UnityEngine;

namespace _game.rnk.Scripts.interactor
{
    public class EndTurn : BaseInteraction, IOnEndTurn
    {
        public IEnumerator OnEndTurn()
        {
            yield break;
        }
    }
    
    public interface IOnEndTurn
    {
        public IEnumerator OnEndTurn();
    }
}