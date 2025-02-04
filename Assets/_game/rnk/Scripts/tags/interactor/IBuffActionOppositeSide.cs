using System.Collections;

namespace _game.rnk.Scripts.tags.buffs
{
    public interface IBuffActionOppositeSide
    {
        public IEnumerator OnBuffAction(BuffState buffState);
    }
}