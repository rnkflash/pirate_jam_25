using _game.rnk.Scripts.battleSystem;

namespace _game.rnk.Scripts.body
{
    public class AnimeTyan : BodyBase
    {
        public AnimeTyan()
        {
            Define<TagName>().loc = "Anime tyan";
            Define<TagDescription>().loc = "hp 10 attak 666";
            Define<TagSprite>().sprite = SpriteUtil.Load("art/bodies/zombietyan");
            Define<TagHealth>().health = 7;
        }
    }
    
    public class NakedMan : BodyBase
    {
        public NakedMan()
        {
            Define<TagName>().loc = "Naked man";
            Define<TagDescription>().loc = "further from the god each day";
            Define<TagSprite>().sprite = SpriteUtil.Load("art/bodies/boer");
            Define<TagHealth>().health = 8;
        }
    }
}