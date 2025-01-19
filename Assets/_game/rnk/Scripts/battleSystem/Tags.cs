using System.Collections.Generic;
using UnityEngine;

namespace _game.rnk.Scripts.battleSystem
{
    public class TagDescription : EntityComponentDefinition
    {
        public string loc;
    }
    
    public class TagName : EntityComponentDefinition
    {
        public string loc;
    }
    
    public enum DiceRarity
    {
        COMMON,
        UNCOMMON,
        RARE
    }
    
    public class TagRarity : EntityComponentDefinition
    {
        public DiceRarity rarity;
    }
    
    public class TagSides : EntityComponentDefinition
    {
        public int sides;
    }
    
    public class TagTint : EntityComponentDefinition
    {
        public Color color;
    }
    
    public class TagPrefab : EntityComponentDefinition
    {
        public DiceInteractiveObject prefab;
    }
    
    public class TagHealth : EntityComponentDefinition
    {
        public int health;
    }
    
    public class TagSprite : EntityComponentDefinition
    {
        public Sprite sprite;
    }
}