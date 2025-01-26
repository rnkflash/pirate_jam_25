using System;
using UnityEngine;

namespace _game.rnk.Scripts.crawler
{
    public class PlayerCollider: MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Encounter encounter))
            {
                Debug.Log("yay we got hit " + encounter.gameObject.name);
            }
        }
    }
}