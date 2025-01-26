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