using System;
using UnityEngine;

namespace _game.rnk.Scripts.battleSystem
{
    public class Selectable : MonoBehaviour
    {
        public GameObject selection;
        [NonSerialized] public bool canBeSelected;

        public void EnableSelection(bool canBeSelected)
        {
            this.canBeSelected = canBeSelected;
            selection.SetActive(canBeSelected);
        }
    }
}