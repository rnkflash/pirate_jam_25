using System;
using _game.rnk.Scripts.tags;
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
            image.sprite = state.face.Get<TagSprite>().sprite;
        }

        public void FreeState()
        {
            state = null;
        }
        
    }
}