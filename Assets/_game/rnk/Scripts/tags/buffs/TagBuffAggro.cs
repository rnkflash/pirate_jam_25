using System.Collections.Generic;
using System.Linq;

namespace _game.rnk.Scripts.tags.buffs
{
    public class TagBuffAggro : EntityComponentDefinition
    {
    }

    public class TagBuffAggroInteractor : BaseInteraction, IModifyTargetList 
    {
        public List<ITarget> ModifyTargetList(List<ITarget> targets)
        {
            var withAggro = targets.FindAll(target => target.GetBuffs().Exists(state => state.model.Is<TagBuffAggro>())).ToList();
            if (withAggro.Count > 0)
                return withAggro;
            return targets;
        }
    }

}