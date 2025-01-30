using System;
using UnityEngine;

namespace _game.rnk.Scripts.crawler
{
    public abstract class Encounter: MonoBehaviour
    {
        [NonSerialized] public bool isTriggered;
        public abstract void CleanUp();
    }

}