namespace _game.rnk.Scripts.tags.buffs
{
    public class TagBuffReceiveDamageModifier : EntityComponentDefinition
    {
        public int value;
        public Operation operation = Operation.SUM;
        public Rounding rounding = Rounding.CEIL;
    }
    
    public class TagBuffReceiveDamageModifierInteractor : BaseInteraction, IReceiveDamageModifier
    {
        public int ModifyDamage(CMSEntity model, int damage)
        {
            if (model.Is<TagBuffReceiveDamageModifier>(out var tag))
            {
                return TagBuffDamageModifierInteractor.Modify(tag.operation, damage, tag.rounding, tag.value);
            }
            return damage;
        }
        
    }
}