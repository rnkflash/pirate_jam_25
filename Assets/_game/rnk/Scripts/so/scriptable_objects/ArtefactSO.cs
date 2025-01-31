using System;
using System.Linq;
using _game.rnk.Scripts.tags;
using UnityEngine;

namespace _game.rnk.Scripts.so.scriptable_objects
{
    [CreateAssetMenu(fileName = "Artefact", menuName = "ScriptableObjects/rnk/Artefact", order = 1)]
    public class ArtefactSO : EntitySO
    {
        public FaceSO face;

        public CMSEntity GetEntity()
        {
            var entity = new CMSEntity
            {
                components = components.ToList()
            };
            
            entity.Define<TagOverrideFace>().face = face.GetEntity();
            return entity;
        }
    }
}