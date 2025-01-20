using _game.rnk.Scripts.battleSystem;
using UnityEngine;

namespace _game.rnk.Scripts.weapons
{
    public class BetrayedSword: WeaponBase
    {
        public BetrayedSword()
        {
            Define<TagName>().loc = "Betrayed Sword";
            Define<TagDescription>().loc = "blablabla koroche ego predali";
            Define<TagTint>().color = Color.red;
        }
    }

}