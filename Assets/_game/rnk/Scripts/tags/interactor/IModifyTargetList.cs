using System.Collections.Generic;

namespace _game.rnk.Scripts.tags.buffs
{
    public interface IModifyTargetList
    {
        public List<ITarget> ModifyTargetList(List<ITarget> targets);
    }
}