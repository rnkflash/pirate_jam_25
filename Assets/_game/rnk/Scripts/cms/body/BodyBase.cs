using _game.rnk.Scripts.tags;
using Color = UnityEngine.Color;

namespace _game.rnk.Scripts.body
{
    public abstract class BodyBase : CMSEntity
    {
        public BodyBase()
        {
            Define<TagHealth>().health = 5;
            Define<TagTint>().color = Color.white;
        }
    }

}