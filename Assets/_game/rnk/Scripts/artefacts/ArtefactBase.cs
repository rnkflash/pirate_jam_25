using _game.rnk.Scripts.battleSystem;

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