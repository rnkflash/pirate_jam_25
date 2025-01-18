using System.Collections.Generic;
using UnityEngine;

public abstract class CardBase : CMSEntity
{
    public CardBase()
    {
        Define<TagPrefab>().prefab = "prefab/card_view".Load<InteractiveObject>();
        Define<TagDescription>().loc = $"Regular card";
        Define<TagRarity>().rarity = DiceRarity.COMMON;
        Define<TagCost>().cost = new Dictionary<ResourceType, int> { { ResourceType.GOLD, 1 } };
    }
}

public class TagDescription : EntityComponentDefinition
{
    public string loc;
}

public class TagName : EntityComponentDefinition
{
    public string loc;
}

public class TagCardView : EntityComponentDefinition
{
    public Sprite bg;
    public Sprite face;
}

public class TagCost : EntityComponentDefinition
{
    public Dictionary<ResourceType, int> cost;
}

public class TagCardBuilding : EntityComponentDefinition
{
    public Building building;
    public int level;
}

public enum Building
{
    BARRACKS,
    ARCHERY_RANGE,
    MAGE_GUILD,
    GOLDMINE,
    MANA_LAB,
    SAWMILL
}

public enum ResourceType
{
    GOLD,
    WOOD,
    MANA
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
    public InteractiveObject prefab;
}

public class TagThriving : EntityComponentDefinition
{
}