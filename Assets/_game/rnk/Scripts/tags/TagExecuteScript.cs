using System;
using System.Collections;

namespace _game.rnk.Scripts.tags
{
    public class TagExecuteScript : EntityComponentDefinition
    {
        public Func<IEnumerator> toExecute;
    }
}