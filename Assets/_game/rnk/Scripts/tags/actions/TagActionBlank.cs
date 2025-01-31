using System.Collections;
using System.Collections.Generic;
using _game.rnk.Scripts.tags.interactor;

namespace _game.rnk.Scripts.tags.actions
{
    public class TagActionBlank : EntityComponentDefinition
    {
    }

    public class TagActionBlankInteractor : BaseInteraction, IDiceFaceAction
    {
        public IEnumerator OnAction(List<ITarget> targets, CMSEntity face)
        {
            if (face.Is<TagActionAttack>(out var action))
            {
                yield break;
            }
        }
    }

}