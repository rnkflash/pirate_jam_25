using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.util;
using Engine.Math;

namespace _game.rnk.Scripts.dice.face
{
    public class AttackFace : FaceBase
    {
        public AttackFace()
        {
            Init(0);
        }
        
        public AttackFace(int value)
        {
            Init(value);
        }
        
        void Init(int attackValue)
        {
            Define<TagName>().loc = "Attack";
            Define<TagDescription>().loc = "Deal " + attackValue.ToString().Color(TextStuff.Reddish) + " dmg to target";
            Define<TagAttack>();
            Define<TagValue>().value = attackValue;
            Define<TagSprite>().sprite = SpriteUtil.Load("art/dice_sprites", "dice_attack");
        }
    }

}