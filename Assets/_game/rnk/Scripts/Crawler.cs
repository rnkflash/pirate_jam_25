using _game.rnk.Scripts.battleSystem;
using UnityEngine;

namespace _game.rnk.Scripts
{
    public class Crawler : MonoBehaviour
    {
        void Awake()
        {
            G.crawler = this;
        }
    }
}