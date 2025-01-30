using _game.rnk.Scripts.so.scriptable_objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace _game.rnk.Scripts.crawler
{
    public class Enemy: MonoBehaviour
    {
        public Transform uiPos;
        public GameObject graphic;
        [FormerlySerializedAs("scriptableObject")] public BodySO body;
    }
}