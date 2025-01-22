namespace _game.rnk.Scripts.tags
{
    public class TagAction : EntityComponentDefinition
    {
        public ActionType action;
        public TargetRow row = TargetRow.Front;
        public TargetSide side = TargetSide.Enemy;
        public TargetArea area = TargetArea.Single;
        public int value;
    }

    public enum TargetSide
    {
        Enemy, Ally, Both, Self, None
    }
    
    public enum TargetRow
    {
        Front, Back, Both
    }
    
    public enum TargetArea
    {
        Single, Row, All
    }

    public enum ActionType
    {
        Attack,
        Heal,
        Def,
        Blank
    }
}