using System.Collections.Generic;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.dice.face;
using _game.rnk.Scripts.tags;

namespace _game.rnk.Scripts.dice
{
    public class AggressiveDice : DiceBase
    {
        public AggressiveDice()
        {
            Define<TagName>().loc = "Attack Dice";
            Define<TagDescription>().loc = "attacks and heals";
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