using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags;
using UnityEngine;

namespace _game.rnk.Scripts.weapons
{
    public class BadBow: WeaponBase
    {
        public BadBow()
        {
            Define<TagName>().loc = "Bad Bow";
            Define<TagDescription>().loc = "its just bad";
            Define<TagTint>().color = Color.green;
        }
    }
}