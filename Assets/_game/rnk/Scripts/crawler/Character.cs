using System;
using System.Collections.Generic;
using _game.rnk.Scripts.so.scriptable_objects;
using UnityEngine;

namespace _game.rnk.Scripts.crawler
{
    [Serializable]
    public class Character
    {
        public WeaponSO weapon;
        public BodySO body;
    }
    
    [Serializable]
    public class Item
    {
        public ArtefactSO artefact;
    }
}