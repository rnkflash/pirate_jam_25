using System.Collections;

namespace _game.rnk.Scripts.tags.buffs
{
    public interface IBuffAction
    {
        public IEnumerator OnBuffAction(BuffState buffState);
    }
}