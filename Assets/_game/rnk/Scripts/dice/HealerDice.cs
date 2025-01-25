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
            
            Define<TagDefaultFaces>().faces = new Dictionary<int, FaceBase>()
            {
                { 0, new HealFace(1) },
                { 1, new HealFace(2) },
                { 2, new HealFace(3) },
                { 3, new HealFace(666) },
                { 4, new BlankFace() },
                { 5, new BlankFace() },
                { 6, new BlankFace() }
            };
        }
    }
}