namespace _game.rnk.Scripts.tags.buffs
{
    public class TagBuffReceiveDamageModifier : EntityComponentDefinition
    {
        public float value;
        public Operation operation = Operation.SUM;
        public Rounding rounding = Rounding.CEIL;
    }
    
    public class TagBuffReceiveDamageModifierInteractor : IReceiveDamageModifier
    {
        public int ModifyDamage(CMSEntity model, int damage)
        {
            var mod = damage;
            if (model.Is<TagBuffReceiveDamageModifier>(out var tag))
            {
                mod = TagBuffDamageModifierInteractor.Modify(tag.operation, damage, tag.rounding);
            }
            return mod;
        }
        
    }
}