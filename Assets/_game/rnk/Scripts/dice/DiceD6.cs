using _game.rnk.Scripts.battleSystem;

namespace _game.rnk.Scripts.dice
{
    public class DiceD6 : DiceBase
    {
        public DiceD6()
        {
            Define<TagName>().loc = "D6 dice";
            Define<TagDescription>().loc = "rolls 1-6";
            Define<TagSides>().sides = 6;
        }  
    }
}