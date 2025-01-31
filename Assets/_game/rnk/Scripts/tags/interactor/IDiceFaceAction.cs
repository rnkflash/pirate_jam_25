using System.Collections;
using System.Collections.Generic;

namespace _game.rnk.Scripts.tags.interactor
{
    public interface IDiceFaceAction
    {
        public IEnumerator OnAction(List<ITarget> targets, CMSEntity face);
    }
}