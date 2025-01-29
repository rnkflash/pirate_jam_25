using UnityEngine;

namespace _game.rnk.Scripts.so.scriptable_objects
{
    [CreateAssetMenu(fileName = "Body", menuName = "ScriptableObjects/rnk/Body", order = 1)]
    public class BodySO : EntitySO
    {
        public DiceSO[] dices;
    }
}