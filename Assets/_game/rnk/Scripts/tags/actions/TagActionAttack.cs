using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags.interactor;
using UnityEngine;

namespace _game.rnk.Scripts.tags.actions
{
    public class TagActionAttack : EntityComponentDefinition
    {
    }

    public class TagActionAttackInteractor : BaseInteraction, IDiceFaceAction
    {
        public  IEnumerator OnAction(List<ITarget> targets, CMSEntity face)
        {
            if (face.Is<TagActionAttack>(out var action))
            {
                var value = face.Get<TagValue>()?.values.FirstOrDefault() ?? 0;
                foreach (var target in targets)
                {
                    var damageable = target.GetView().GetComponent<Damageable>();
                    if (damageable)
                    {
                        yield return damageable.Hit(value);
                    }
                }
            }
        }
    }

}