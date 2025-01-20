using UnityEngine;
using UnityEngine.Serialization;

namespace _game
{
    [CreateAssetMenu(fileName = "NewItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
    public class ItemData : ScriptableObject
    {
        public int id;
        public Sprite sprite;
    }
}