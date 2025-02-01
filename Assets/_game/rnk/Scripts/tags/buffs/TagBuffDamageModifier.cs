using System;
using System.Collections;
using UnityEngine;

namespace _game.rnk.Scripts.tags.buffs
{
    public class TagBuffDamageModifier : EntityComponentDefinition
    {
        public int value;
        public Operation operation = Operation.SUM;
        public Rounding rounding = Rounding.CEIL;
    }

    public enum Rounding
    {
        FLOOR, CEIL
    }
    
    public enum Operation
    {
        MULTIPLY, SUM, PERCENT, DIVIDE
    }
    
    public class TagBuffDamageModifierInteractor : BaseInteraction, IDamageModifier
    {
        public int ModifyDamage(CMSEntity model, int damage)
        {
            var mod = damage;
            if (model.Is<TagBuffDamageModifier>(out var tag))
            {
                mod = Modify(tag.operation, damage, tag.rounding, tag.value);
            }
            return mod;
        }
        
        public static int Modify(Operation operation, int damage, Rounding rounding, int value)
        {
            switch (operation)
            {
                case Operation.MULTIPLY:
                    damage = Round(damage * value, rounding);
                    break;

                case Operation.SUM:
                    damage = Round(damage + value, rounding);
                    break;

                case Operation.PERCENT:
                    damage = Round(damage * value / 100.0f, rounding);
                    break;

                case Operation.DIVIDE:
                    damage = Round(damage / (float)value, rounding);
                    break;
            }
                
            return damage;
        }

        static int Round(float value, Rounding rounding)
        {
            int newValue = 0;
            switch (rounding)
            {
                case Rounding.FLOOR:
                    newValue = Mathf.FloorToInt(value);
                    break;
                case Rounding.CEIL:
                    newValue = Mathf.CeilToInt(value);
                    break;
            }
            return newValue;
        }
    }
}