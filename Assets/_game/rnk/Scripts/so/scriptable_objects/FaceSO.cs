using System;
using _game.rnk.Scripts.tags;

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
            return entity;
        }
    }
}