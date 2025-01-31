using System.Collections;
using System.Collections.Generic;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags.interactor;

namespace _game.rnk.Scripts.tags.buffs
{
    public class TagBuffPoison : EntityComponentDefinition
    {
    }

    public class TagBuffPoisonInteractor : BaseInteraction, IBuffAction, IDiceFaceAction
    {
        public IEnumerator OnBuffAction(BuffState buffState)
        {
            if (buffState.model.Is<TagBuffPoison>(out var poison))
            {
                var value = buffState.model.Get<TagValue>()?.values[0] ?? 0;
                
                var damageable = buffState.target.GetView().GetComponent<Damageable>();
                if (damageable)
                {
                    yield return damageable.Hit(value);
                }
            }
        }
        public IEnumerator OnAction(List<ITarget> targets, CMSEntity face, BaseCharacterState owner)
        {
            if (face.Is<TagBuffPoison>(out var poison))
            {
                foreach (var target in targets)
                {
                    yield return G.battle.AddBuff(target, face, owner);
                }
            }
        }
    }

}