using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags;

namespace _game.rnk.Scripts.artefacts
{
    public class TestArtefact : ArtefactBase
    {
        public TestArtefact()
        {
            Define<TagAddToAllRolls>().addValue = 1;
            Define<TagName>().loc = "Test Artefact";
            Define<TagDescription>().loc = "Adds 1 to all rolls";
        }
    }

    public class TagAddToAllRolls : EntityComponentDefinition
    {
        public int addValue;
    }

    public class AddToAllRolls : BaseInteraction, IRollFilter
    {
        public int OverwriteRoll(DiceState state, int roll)
        {
            var addValue = 0;
            foreach (var artefact in state.artefacts)
            {
                if (artefact.model.Is<TagAddToAllRolls>(out var tag))
                {
                    addValue += tag.addValue;
                }    
            }
            
            return roll + addValue;
        }
    }

    public interface IRollFilter
    {
        public int OverwriteRoll(DiceState state, int roll);
    }
}