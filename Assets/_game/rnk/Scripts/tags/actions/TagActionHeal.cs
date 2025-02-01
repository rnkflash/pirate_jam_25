using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags.interactor;

namespace _game.rnk.Scripts.tags.actions
{
    public class TagActionHeal : EntityComponentDefinition
    {
    }

    public class TagActionHealInteractor : BaseInteraction, IDiceFaceAction
    {
        public IEnumerator OnAction(List<ITarget> targets, CMSEntity face, BaseCharacterState owner, int[] values)
        {
            if (face.Is<TagActionHeal>(out var action))
            {
                var value = values.FirstOrDefault();
                foreach (var target in targets)
                {
                    var damageable = target.GetView().GetComponent<Damageable>();
                    if (damageable)
                    {
                        yield return damageable.Heal(value);
                    }
                }
            }
        }
    }

}