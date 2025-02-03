using System.Linq;
using UnityEngine;

namespace _game.rnk.Scripts.so.scriptable_objects
{
    [CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObjects/rnk/Entity", order = 1)]
    public class EntitySO : ScriptableObject
    {
        [SerializeReference, Subclass(IsList = false)]
        public EntityComponentDefinition[] components;

        public CMSEntity GetEntity()
        {
            var entity = new CMSEntity
            {
                id = name,
                components = components.ToList()
            };
            return entity;
        }
    }
}