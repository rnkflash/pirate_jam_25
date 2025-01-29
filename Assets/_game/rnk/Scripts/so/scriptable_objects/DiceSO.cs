using System;
using System.Linq;
using _game.rnk.Scripts.tags;
using UnityEngine;

namespace _game.rnk.Scripts.so.scriptable_objects
{
    [CreateAssetMenu(fileName = "Dice", menuName = "ScriptableObjects/rnk/Dice", order = 1)]
    public class DiceSO : EntitySO
    {
        public FaceSO[] faces;
        
        public CMSEntity GetEntity()
        {
            var entity = new CMSEntity
            {
                components = components.ToList()
            };

            var defaultFaces = entity.Define<TagDefaultFaces>();
            defaultFaces.faces = faces.Select(so => so.GetEntity()).ToArray();
            
            return entity;
        }
    }

    [Serializable]
    public class FaceSO
    {
        public EntitySO face;
        public int value;

        public CMSEntity GetEntity()
        {
            var entity = face.GetEntity();
            entity.Define<TagValue>().value = value;
            return entity;
        }
    }
}