using System.Collections;
using System.Collections.Generic;
using _game.rnk.Scripts.so.scriptable_objects;
using _game.rnk.Scripts.tags.buffs;
using _game.rnk.Scripts.tags.interactor;

namespace _game.rnk.Scripts.tags.actions
{
    public class TagActionAddBuff : EntityComponentDefinition
    {
        public bool self;
        public CMSEntity buff;
    }
    
    public class TagActionAddBuffSO : EntityComponentDefinition
    {
        public bool self;
        public bool dontAddIfAleadyHas;
        public BuffSO buffSO;

        public void Convert(CMSEntity cms)
        {
            var newComp = cms.DefineNew<TagActionAddBuff>();
            newComp.self = self;
            newComp.buff = buffSO.GetEntity();
        }
    }
    
    public class TagActionAddBuffInteractor : BaseInteraction, IDiceFaceAction
    {
        public IEnumerator OnAction(List<ITarget> targets, CMSEntity face, BaseCharacterState owner, int[] values)
        {
            if (face.IsAll<TagActionAddBuff>(out var buffTags))
            {
                foreach (var buffTag in buffTags)
                {
                    G.audio.Play<SFX_AddBuff>();
                    
                    if (buffTag.self)
                    {
                        yield return G.battle.AddBuff(owner, buffTag.buff, owner);
                    }
                    else
                    {
                        foreach (var target in targets)
                        {
                            var dobro = true;
                            
                            //хардкод: на случай если нельзя ложить больше одного бафа этого типа 
                            /*if (buffTag.buff.Is(typeof(TagBuffPoison)))
                            {
                                var buffsOnTarget = G.battle.GetBuffs(target.GetState());
                                dobro = buffsOnTarget.Count == 0;
                            }*/
                            
                            if (dobro)
                                yield return G.battle.AddBuff(target, buffTag.buff, owner);
                        }
                    }
                }
            }
        }
    }
}