using System.Collections.Generic;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.dice.face;
using _game.rnk.Scripts.tags;

namespace _game.rnk.Scripts.dice
{
    public class DiceD6 : DiceBase
    {
        public DiceD6()
        {
            Define<TagName>().loc = "Standart Dice";
            Define<TagDescription>().loc = "atk 1 2 3, def 1 2, heal 3, blank";
            Define<TagSides>().sides = 6;
            
            Define<TagDefaultFaces>().faces = new Dictionary<int, FaceBase>()
            {
                { 0, new AttackFace(1) },
                { 1, new AttackFace(2) },
                { 2, new AttackFace(3) },
                { 3, new DefFace(1) },
                { 4, new DefFace(2) },
                { 5, new HealFace(3) },
                { 6, new BlankFace() }
            };
        }
    }
}