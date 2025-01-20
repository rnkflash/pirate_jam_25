using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags;

namespace _game.rnk.Scripts.artefacts
{
    public abstract class ArtefactBase : CMSEntity
    {
        public ArtefactBase()
        {
            Define<TagName>().loc = "Artefact Base";
        }
    }
}