using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.so.scriptable_objects;
using _game.rnk.Scripts.tags.interactor;

namespace _game.rnk.Scripts.tags.actions
{
    public class TagActionAddBuff : EntityComponentDefinition
    {
        public CMSEntity buff;
    }
    
    public class TagActionAddBuffSO : EntityComponentDefinition
    {
        public BuffSO buffSO;
    }
    
    public class TagActionAddBuffInteractor : BaseInteraction, IDiceFaceAction
    {
        public IEnumerator OnAction(List<ITarget> targets, CMSEntity face, BaseCharacterState owner, int[] values)
        {
            if (face.Is<TagActionAddBuff>(out var buffTag))
            {
                var value = values.FirstOrDefault();
                foreach (var target in targets)
                {
                    G.audio.Play<SFX_AddBuff>();
                    
                    yield return G.battle.AddBuff(target, buffTag.buff, owner);
                }
            }
        }
    }
}