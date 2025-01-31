using System;
using UnityEngine;
using UnityEngine.UI;

namespace _game.rnk.Scripts.inventory
{
    public class InventoryItem : MonoBehaviour
    {
        public Image image;
        public Image bg;

        [NonSerialized] public ArtefactState state;

        [NonSerialized] public DraggableItem draggable;

        void Awake()
        {
            draggable = GetComponent<DraggableItem>();
        }

        public void SetState(ArtefactState state)
        {
            this.state = state;
        }

        public void FreeState()
        {
            state = null;
        }
        
    }
}