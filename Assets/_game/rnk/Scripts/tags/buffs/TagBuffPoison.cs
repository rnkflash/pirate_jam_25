using System.Collections;
using System.Collections.Generic;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags.interactor;

namespace _game.rnk.Scripts.tags.buffs
{
    public class TagBuffPoison : EntityComponentDefinition
    {
    }

    public class TagBuffPoisonInteractor : BaseInteraction, IBuffAction 
    {
        public IEnumerator OnBuffAction(BuffState buffState)
        {
            if (buffState.model.Is<TagBuffPoison>(out var poison))
            {
                var value = buffState.model.Get<TagValue>()?.values[0] ?? 0;

                G.ui.poisonHit.Bahni(buffState.target.GetView().transform.position);
                G.audio.Play<SFX_PoisonAttack>();
                
                yield return G.battle.Damage(buffState.target, buffState.castedBy, value);
            }
        }
    }
}