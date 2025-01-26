using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.util;

namespace _game.rnk.Scripts.dice.face
{
    public class BlankFace : FaceBase
    {
        public BlankFace()
        {
            Define<TagName>().loc = "Blank";
            Define<TagDescription>().loc = "Do nothing";
            Define<TagAction>().With(t => {
                t.action = ActionType.Blank;
            });
        }
    }

}