using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags.interactor;
using UnityEngine;

namespace _game.rnk.Scripts.tags.actions
{
    public class TagActionBloodLetting : EntityComponentDefinition
    {
    }

    public class TagActionBloodLettingInteractor : BaseInteraction, IDiceFaceAction
    {
        public IEnumerator OnAction(List<ITarget> targets, CMSEntity face, BaseCharacterState owner, int[] values)
        {
            if (face.Is<TagActionBloodLetting>(out var action))
            {
                var value1 = values.ElementAtOrDefault(0);
                var value2 = values.ElementAtOrDefault(1);
                
                foreach (var target in targets)
                {
                    G.ui.meleeHit.Bahni(target.GetView().transform.position);
                    G.audio.Play<SFX_GetDamage>();
                    yield return G.battle.Damage(target, owner, value1);
                    
                    var damageable = target.GetView().GetComponent<Damageable>();
                    if (damageable)
                    {
                        yield return damageable.Heal(value2);
                    }
                }
            }
        }
    }

}