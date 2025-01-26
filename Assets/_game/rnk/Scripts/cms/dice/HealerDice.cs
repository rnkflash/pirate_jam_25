using System.Collections.Generic;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.dice.face;
using _game.rnk.Scripts.tags;

namespace _game.rnk.Scripts.dice
{
    public class HealerDice : DiceBase
    {
        public HealerDice()
        {
            Define<TagName>().loc = "Heal Dice";
            Define<TagDescription>().loc = "only heals";
            Define<TagSides>().sides = 6;
            
            Define<TagDefaultFaces>().faces = new FaceBase[]
            {
                new AttackFace(1),
                new AttackFace(2),
                new AttackFace(3),
                new HealFace(1),
                new BlankFace(),
                new BlankFace(),
                new BlankFace()
            };
        }
    }
}