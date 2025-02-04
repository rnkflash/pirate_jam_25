using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.battleSystem;
using UnityEngine;

namespace _game.rnk.Scripts.tags.buffs
{
    public class TagBuffMageArmor : EntityComponentDefinition
    {
    }

    public class TagBuffMageArmorInteractor : BaseInteraction, IBuffActionOppositeSide 
    {
        public IEnumerator OnBuffAction(BuffState buffState)
        {
            if (buffState.model.Is<TagBuffMageArmor>(out var tag))
            {
                var value = buffState.model.Get<TagValue>().values.FirstOrDefault();
                var damageable = buffState.target.GetView().GetComponent<Damageable>();
                if (damageable)
                {
                    yield return damageable.Armor(value);
                }
            }
        }
    }
}