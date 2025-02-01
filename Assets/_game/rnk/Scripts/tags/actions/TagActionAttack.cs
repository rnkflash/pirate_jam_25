using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.tags.interactor;

namespace _game.rnk.Scripts.tags.actions
{
    public class TagActionAttack : EntityComponentDefinition
    {
        public int valueIndex;
    }

    public class TagActionAttackInteractor : BaseInteraction, IDiceFaceAction
    {
        public IEnumerator OnAction(List<ITarget> targets, CMSEntity face, BaseCharacterState owner, int[] values)
        {
            if (face.Is<TagActionAttack>(out var action))
            {
                var value = values.FirstOrDefault();
                foreach (var target in targets)
                {
                    G.ui.meleeHit.Bahni(target.GetView().transform.position);
                    G.audio.Play<SFX_GetDamage>();
                    
                    yield return G.battle.Damage(target, owner, value);
                }
            }
        }
    }

}