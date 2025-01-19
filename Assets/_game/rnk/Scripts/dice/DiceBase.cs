using _game.rnk.Scripts.battleSystem;

namespace _game.rnk.Scripts.dice
{
    public abstract class DiceBase : CMSEntity
    {
        public DiceBase()
        {
            Define<TagPrefab>().prefab = "prefab/DiceInteractiveObject".Load<DiceInteractiveObject>();
            Define<TagSides>().sides = 1;
        }
    }
}