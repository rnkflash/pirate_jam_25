using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.util;
using Engine.Math;

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
            Define<TagName>().loc = "Defend";
            Define<TagDescription>().loc = "Add " + value.ToString().Color(TextStuff.Blueish) + " armor to target";
            Define<TagSprite>().sprite = SpriteUtil.Load("art/dice_sprites", "dice_def");
            Define<TagValue>().value = value;
            Define<TagAction>().With(t => {
                t.action = ActionType.Def;
                t.side = TargetSide.Ally;
                t.area = TargetArea.Single;
            });
        }
    }
}