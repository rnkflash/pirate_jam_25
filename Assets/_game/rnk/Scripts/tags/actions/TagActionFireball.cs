using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.tags.interactor;

namespace _game.rnk.Scripts.tags.actions
{
    public class TagActionFireball : EntityComponentDefinition
    {
        public int valueIndex;
    }

    public class TagActionFireballInteractor : BaseInteraction, IDiceFaceAction
    {
        public IEnumerator OnAction(List<ITarget> targets, CMSEntity face, BaseCharacterState owner, int[] values)
        {
            if (face.Is<TagActionFireball>(out var action))
            {
                var value = values.FirstOrDefault();
                foreach (var target in targets)
                {
                    G.ui.fireballHit.Bahni(target.GetView().transform.position);
                    G.audio.Play<SFX_Fireball>();
                    
                    yield return G.battle.Damage(target, owner, value);
                }
            }
        }
    }

}