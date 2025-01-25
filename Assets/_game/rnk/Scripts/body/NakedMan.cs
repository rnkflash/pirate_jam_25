using _game.rnk.Scripts.tags;

namespace _game.rnk.Scripts.body
{
    public class NakedMan : BodyBase
    {
        public NakedMan()
        {
            Define<TagName>().loc = "Kolbaser";
            Define<TagDescription>().loc = "we are further from the god each day";
            Define<TagSprite>().sprite = SpriteUtil.Load("art/bodies/boer");
            Define<TagHealth>().health = 8;
        }
    }
    
    public class Healer : BodyBase
    {
        public Healer()
        {
            Define<TagName>().loc = "Corrupt Priest";
            Define<TagDescription>().loc = "we are further from the god each day";
            Define<TagSprite>().sprite = SpriteUtil.Load("art/bodies/healer");
            Define<TagHealth>().health = 6;
        }
    }
}