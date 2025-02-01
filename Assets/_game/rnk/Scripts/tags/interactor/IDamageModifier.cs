namespace _game.rnk.Scripts.tags.buffs
{
    public interface IDamageModifier
    {
        public int ModifyDamage(CMSEntity model, int damage);
    }
}