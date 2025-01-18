using System.Collections.Generic;
using UnityEngine;

public class BarrackCard : CardBase
{
    public BarrackCard()
    {
        Define<TagName>().loc = "Barracks";
        Define<TagDescription>().loc = "Produces soldiers.";
        Define<TagCardView>().bg = SpriteUtil.Load("art/cards", "card_bg_1");
        Define<TagCardView>().face = SpriteUtil.Load("art/cards", "sword");
        Define<TagCost>().cost = new() { { ResourceType.GOLD, 1 } };
        Define<TagCardBuilding>().building = Building.BARRACKS;
        Define<TagCardBuilding>().level = 1;
    }
}

public class ArcheryRangeCard : CardBase
{
    public ArcheryRangeCard()
    {
        Define<TagName>().loc = "Archery Range";
        Define<TagDescription>().loc = "Produces archers.";
        Define<TagCardView>().bg = SpriteUtil.Load("art/cards", "card_bg_1");
        Define<TagCardView>().face = SpriteUtil.Load("art/cards", "bow");
        Define<TagCost>().cost = new() { { ResourceType.WOOD, 1 } };
        Define<TagCardBuilding>().building = Building.ARCHERY_RANGE;
        Define<TagCardBuilding>().level = 1;
    }
}

public class MageGuildCard : CardBase
{
    public MageGuildCard()
    {
        Define<TagName>().loc = "Mage Guild";
        Define<TagDescription>().loc = "Produces mages.";
        Define<TagCardView>().bg = SpriteUtil.Load("art/cards", "card_bg_1");
        Define<TagCardView>().face = SpriteUtil.Load("art/cards", "staff");
        Define<TagCost>().cost = new() { { ResourceType.MANA, 1 } };
        Define<TagCardBuilding>().building = Building.MAGE_GUILD;
        Define<TagCardBuilding>().level = 1;
    }
}