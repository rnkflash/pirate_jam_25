namespace _game.rnk.Scripts.tags
{
    public class TagActionTargeting : EntityComponentDefinition
    {
        public TargetRow row = TargetRow.Front;
        public TargetSide side = TargetSide.Enemy;
        public TargetArea area = TargetArea.Single;
    }
}