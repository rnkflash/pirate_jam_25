using System.Collections;

namespace _game.rnk.Scripts.tags.buffs
{
    public interface IReceiveDamageModifier
    {
        public int ModifyDamage(CMSEntity model, int damage);
    }

}