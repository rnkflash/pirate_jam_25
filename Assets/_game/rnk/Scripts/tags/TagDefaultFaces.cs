using System.Collections.Generic;
using _game.rnk.Scripts.dice.face;

namespace _game.rnk.Scripts.tags
{
    public class TagDefaultFaces : EntityComponentDefinition
    {
        public Dictionary<int, FaceBase> faces;
    }
}