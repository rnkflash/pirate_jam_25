using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.ui;
using UnityEngine;

namespace _game.rnk.Scripts.util
{
    public static class ViewExtensions
    {
        public static BuffList GetBuffList(this MonoBehaviour mono)
        {
            return (mono as IHasBuffList)?.GetBuffList();
        }
    }
}