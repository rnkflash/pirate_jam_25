using _game.rnk.Scripts.battleSystem;
using UnityEngine;

namespace _game.rnk.Scripts.weapons
{
    public class UselessStaff: WeaponBase
    {
        public UselessStaff()
        {
            Define<TagName>().loc = "Useless Staff";
            Define<TagDescription>().loc = "not the best staff around";
            Define<TagTint>().color = Color.yellow;
        }
    }
}