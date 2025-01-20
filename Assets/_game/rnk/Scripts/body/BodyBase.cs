using _game.rnk.Scripts.battleSystem;

namespace _game.rnk.Scripts.body
{
    public abstract class BodyBase : CMSEntity
    {
        public BodyBase()
        {
            Define<TagHealth>().health = 5;
        }
    }

}