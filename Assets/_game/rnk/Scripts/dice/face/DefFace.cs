using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags;

namespace _game.rnk.Scripts.dice.face
{
    public class DefFace : FaceBase
    {
        public DefFace()
        {
            Init(0);
        }
        
        public DefFace(int value)
        {
            Init(value);
        }
        
        void Init(int value)
        {
            Define<TagName>().loc = "Def face";
            Define<TagDescription>().loc = $"add {value} def to target";
            Define<TagDefend>().value = value;
            Define<TagSprite>().sprite = SpriteUtil.Load("art/dice_sprites", "dice_def");
        }
    }
}