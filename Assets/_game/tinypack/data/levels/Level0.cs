using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Math;

public class Level0 : CMSEntity
{
    public Level0()
    {
        Define<TagExecuteScript>().toExecute = Script;
    }

    IEnumerator Script()
    {
        G.main.HideHud();
        
        yield return G.ui.Say("Use spells to destroy the enemy kingdom.");
        yield return G.main.SmartWait(5f);
        yield return G.ui.Say("Destroy the ice and conquer the lands to restore the former might of Frostland!");
        yield return G.main.SmartWait(3f);
        yield return G.ui.Say("Restore the kingdom by breaking the curse of the Eternal Ice!");
        yield return G.main.SmartWait(3f);
        yield return G.ui.Say("Become the sole ruler of the kingdom!");
        yield return G.main.SmartWait(3f);
        yield return G.ui.Unsay();
        
        G.main.ShowHud();
    }
}

public class TagExecuteScript : EntityComponentDefinition
{
    public Func<IEnumerator> toExecute;
}

public class TagIsFinal : EntityComponentDefinition
{
}