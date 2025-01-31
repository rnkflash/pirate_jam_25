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
        public  IEnumerator OnAction(List<ITarget> targets, CMSEntity face)
        {
            if (face.Is<TagActionBloodLetting>(out var action))
            {
                var value1 = face.Get<TagValue>()?.values[0] ?? 0;
                var value2 = face.Get<TagValue>()?.values[1] ?? 0;
                
                foreach (var target in targets)
                {
                    var damageable = target.GetView().GetComponent<Damageable>();
                    if (damageable)
                    {
                        yield return damageable.Hit(value1);
                        yield return damageable.Heal(value2);
                    }
                }
            }
        }
    }

}