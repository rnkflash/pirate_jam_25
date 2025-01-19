using _game.rnk.Scripts.battleSystem;
using UnityEngine;

namespace _game.rnk.Scripts.weapons
{
    public class BrokenShield: WeaponBase
    {
        public BrokenShield()
        {
            Define<TagName>().loc = "Broken Shield";
            Define<TagDescription>().loc = "this shield broke when was most needed and killed its owner";
            Define<TagTint>().color = Color.cyan;
        }
    }
}