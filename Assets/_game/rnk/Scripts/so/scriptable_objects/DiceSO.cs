using System;
using UnityEngine;

namespace _game.rnk.Scripts.so.scriptable_objects
{
    [CreateAssetMenu(fileName = "Dice", menuName = "ScriptableObjects/rnk/Dice", order = 1)]
    public class DiceSO : EntitySO
    {
        public FaceSO[] faces;
    }

    [Serializable]
    public class FaceSO
    {
        public EntitySO face;
        public int value;
    }
}