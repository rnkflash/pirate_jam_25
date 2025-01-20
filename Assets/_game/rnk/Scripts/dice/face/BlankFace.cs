using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags;

namespace _game.rnk.Scripts.dice.face
{
    public class BlankFace : FaceBase
    {
        public BlankFace()
        {
            Define<TagName>().loc = "blank face";
            Define<TagDescription>().loc = "does nothing";
            Define<TagBlank>();
        }
    }

}