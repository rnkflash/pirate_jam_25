using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.util;
using Engine.Math;

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
            Define<TagName>().loc = "Heal";
            Define<TagDescription>().loc = "Add " + value.ToString().Color(TextStuff.Greenish) + " hp to target";
            Define<TagSprite>().sprite = SpriteUtil.Load("art/dice_sprites", "dice_heal");
            Define<TagValue>().value = value;
            Define<TagAction>().With(t => {
                t.action = ActionType.Heal;
                t.side = TargetSide.Ally;
                t.area = TargetArea.Single;
            });
        }
    }

}