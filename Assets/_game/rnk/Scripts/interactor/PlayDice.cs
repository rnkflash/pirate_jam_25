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
            //TODO: override from artefacts
            var defaultSlots = state.model.Get<TagDefaultFaces>().faces;
            var idx = state.rollValue;
            var slot = defaultSlots[idx] ?? new BlankFace();
            if (slot.Is<TagAttack>(out var attack))
            {
                //TODO: attack something?
                yield return new WaitForSeconds(0.25f);
            }
            
            if (slot.Is<TagDefend>(out var def))
            {
                //TODO: def something?
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
    
    public interface IOnPlayDice
    {
        public IEnumerator OnPlayDice(DiceState state);
    }
}