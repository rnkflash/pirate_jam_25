using System.Collections.Generic;

public class RunState
{
    public int level;
    public List<CardId> deck = new List<CardId>();
    public int drawSize = 3;
    public int health = 10;
    public int maxHealth = 10;

    public bool HasCard(string mID)
    {
        foreach (var db in deck)
            if (db.id == mID)
                return true;
        return false;
    }
}