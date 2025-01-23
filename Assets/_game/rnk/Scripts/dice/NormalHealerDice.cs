using System.Collections.Generic;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.dice.face;
using _game.rnk.Scripts.tags;

namespace _game.rnk.Scripts.dice
{
    public class NormalHealerDice : DiceBase
    {
        public NormalHealerDice()
        {
            Define<TagName>().loc = "Heal Dice";
            Define<TagDescription>().loc = "heals mostly";
            Define<TagSides>().sides = 6;
            
            Define<TagDefaultFaces>().faces = new Dictionary<int, FaceBase>()
            {
                { 0, new AttackFace(1) },
                { 1, new DefFace(1) },
                { 2, new HealFace(1) },
                { 3, new HealFace(2) },
                { 4, new HealFace(3) },
                { 5, new HealFace(4) },
                { 6, new BlankFace() }
            };
        }
    }
}