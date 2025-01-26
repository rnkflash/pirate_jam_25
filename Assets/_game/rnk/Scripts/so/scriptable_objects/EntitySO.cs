using UnityEngine;

namespace _game.rnk.Scripts.so.scriptable_objects
{
    [CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObjects/rnk/Entity", order = 1)]
    public class EntitySO : ScriptableObject
    {
        [SerializeReference, Subclass]
        public EntityComponentDefinition[] components;
    }
}