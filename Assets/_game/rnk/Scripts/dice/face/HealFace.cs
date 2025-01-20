using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags;

namespace _game.rnk.Scripts.dice.face
{
    public class HealFace : FaceBase
    {
        public HealFace()
        {
            Init(0);
        }
        
        public HealFace(int value)
        {
            Init(value);
        }
        
        void Init(int value)
        {
            Define<TagName>().loc = "Heal face";
            Define<TagDescription>().loc = $"add {value} hp to target";
            Define<TagDefend>().value = value;
            Define<TagSprite>().sprite = SpriteUtil.Load("art/dice_sprites", "dice_heal");
        }
    }
}