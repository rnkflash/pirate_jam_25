using System.Collections;
using System.Collections.Generic;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags.interactor;

namespace _game.rnk.Scripts.tags.actions
{
    public class TagActionArmor : EntityComponentDefinition
    {
        
    }

    public class TagActionArmorInteractor : BaseInteraction, IDiceFaceAction
    {
        public IEnumerator OnAction(List<ITarget> targets, CMSEntity face)
        {
            if (face.Is<TagActionArmor>(out var action))
            {
                var value = face.Get<TagValue>()?.value ?? 0;
                foreach (var target in targets)
                {
                    var damageable = target.GetView().GetComponent<Damageable>();
                    if (damageable)
                    {
                        yield return damageable.Armor(value);
                    }
                }
            }
        }
    }

}