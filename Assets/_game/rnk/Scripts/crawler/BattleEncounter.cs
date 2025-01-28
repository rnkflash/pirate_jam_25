using System;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.so.scriptable_objects;
using UnityEngine;

namespace _game.rnk.Scripts.crawler
{
    public class BattleEncounter: Encounter
    {
        [NonSerialized] public List<Enemy> enemies;

        void Awake()
        {
            enemies = GetComponentsInChildren<Enemy>().ToList();
            GetComponent<Collider>().isTrigger = true;
        }
    }
}