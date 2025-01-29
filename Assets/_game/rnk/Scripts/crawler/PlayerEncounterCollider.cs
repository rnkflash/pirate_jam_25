using System;
using UnityEngine;

namespace _game.rnk.Scripts.crawler
{
    public class PlayerEncounterCollider: MonoBehaviour
    {
        Player player;

        void Awake()
        {
            player = GetComponentInParent<Player>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Encounter encounter))
            {
                player.OnEncounter(encounter);
            }
        }
    }
}