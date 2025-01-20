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
            G.hud.DisableHud();

            yield return ReturnAllDices();

            yield return ResetRerolls();

            G.camera.UIHit();

            G.hud.EnableHud();

            yield return new WaitForSeconds(0.25f);
            
            yield return G.main.RollAllDices();
        }
        
        IEnumerator ReturnAllDices()
        {
            var copy = G.main.rollDicesZone.objects.Select(o => o).ToList();
            foreach (var dice in copy)
            {
                G.main.ReturnDice(dice);
            }
            
            yield return new WaitForSeconds(0.2f);
        }
        
        IEnumerator ResetRerolls()
        {
            /*var shuffleAmount = discard.Count;
            for (int i = 0; i < shuffleAmount; i++)
            {
                StartCoroutine(ShuffleDiscardCard());
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitUntil(() => discard.Count == 0);*/
            yield return new WaitForSeconds(0.2f);
        }
    }
    
    public interface IOnEndTurn
    {
        public IEnumerator OnEndTurn();
    }
}