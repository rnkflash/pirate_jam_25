using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags;

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
            Define<TagName>().loc = "Attack face";
            Define<TagDescription>().loc = $"Deals {attackValue} dmg to target";
            Define<TagAttack>().value = attackValue;
            Define<TagSprite>().sprite = SpriteUtil.Load("art/dice_sprites", "dice_attack");
        }
    }
}