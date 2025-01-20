using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags;

namespace _game.rnk.Scripts.dice.face
{
    public class BlankFace : FaceBase
    {
        public BlankFace()
        {
            Define<TagName>().loc = "Blank";
            Define<TagDescription>().loc = "Do nothing";
            Define<TagBlank>();
        }
    }

}