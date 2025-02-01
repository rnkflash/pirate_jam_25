using System;
using System.Collections;
using UnityEngine;

namespace _game.rnk.Scripts.tags.buffs
{
    public class TagBuffDamageModifier : EntityComponentDefinition
    {
        public float value;
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
    
    public class TagBuffDamageModifierInteractor : IDamageModifier
    {
        public int ModifyDamage(CMSEntity model, int damage)
        {
            var mod = damage;
            if (model.Is<TagBuffDamageModifier>(out var tag))
            {
                mod = Modify(tag.operation, damage, tag.rounding);
            }
            return mod;
        }
        
        public static int Modify(Operation operation, int value, Rounding rounding)
        {
            var modifiedDamage = value;

            switch (operation)
            {
                case Operation.MULTIPLY:
                    modifiedDamage = Round(modifiedDamage * value, rounding);
                    break;

                case Operation.SUM:
                    modifiedDamage = Round(modifiedDamage + value, rounding);
                    break;

                case Operation.PERCENT:
                    modifiedDamage = Round(modifiedDamage * value / 100.0f, rounding);
                    break;

                case Operation.DIVIDE:
                    modifiedDamage = Round(modifiedDamage / value, rounding);
                    break;
            }
                
            return modifiedDamage;
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