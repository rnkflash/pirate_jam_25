using System;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _game.rnk.Scripts.inventory
{
    public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        
        string TryGetSomethingDesc()
        {
            if (state == null) return null;
            var desc = "";
            var face = state.face;
            var values = face.Get<TagValue>()?.values ?? Array.Empty<int>();
            if (face.Is<TagName>(out var tn)) desc += tn.loc + ". \n";
            if (face.Is<TagDescription>(out var td)) desc += td.loc.WithValues(values);
            return desc;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            G.hud.tooltip.Show(TryGetSomethingDesc());
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            G.hud.tooltip.Hide();
        }
    }
}