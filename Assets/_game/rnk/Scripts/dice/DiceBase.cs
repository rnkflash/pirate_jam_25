using System.Collections;
using System.Collections.Generic;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.dice.face;
using _game.rnk.Scripts.tags;

namespace _game.rnk.Scripts.dice
{
    public abstract class DiceBase : CMSEntity
    {
        protected DiceBase()
        {
            Define<TagPrefab>().prefab = "prefab/DiceInteractiveObject".Load<DiceInteractiveObject>();
            Define<TagSides>().sides = 1;

            Define<TagDefaultFaces>().faces = new Dictionary<int, FaceBase>()
            {
                { 0, new AttackFace(1) },
                { 1, new AttackFace(2) },
                { 2, new AttackFace(3) },
                { 3, new DefFace(1) },
                { 4, new DefFace(2) },
                { 5, new DefFace(3) },
                { 6, new BlankFace() }
            };
        }
    }
}