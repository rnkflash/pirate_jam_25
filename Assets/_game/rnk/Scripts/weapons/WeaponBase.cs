using _game.rnk.Scripts.battleSystem;
using UnityEngine;

namespace _game.rnk.Scripts.weapons
{
    public abstract class WeaponBase : CMSEntity
    {
        public WeaponBase()
        {
            Define<TagTint>().color = Color.black;
        }
    }
}