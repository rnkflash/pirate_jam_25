using System;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.tags.actions;

namespace _game.rnk.Scripts.so.scriptable_objects
{
    [Serializable]
    public class FaceSO
    {
        public EntitySO face;
        public int[] values;

        public CMSEntity GetEntity()
        {
            var entity = face.GetEntity();
            entity.Define<TagValue>().values = values;
            if (entity.IsAll<TagActionAddBuffSO>(out var buffSoArr))
            {
                foreach (var buffSo in buffSoArr)
                    buffSo.Convert(entity);
                
                entity.UnDefine<TagActionAddBuffSO>();
            }
            return entity;
        }
    }
}