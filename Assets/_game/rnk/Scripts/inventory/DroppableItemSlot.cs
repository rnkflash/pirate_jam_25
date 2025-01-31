using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace _game.rnk.Scripts.inventory
{
    public class DroppableItemSlot : MonoBehaviour
    {
        [NonSerialized] public bool isFree;
        [NonSerialized] public DraggableItem item;
        public UnityAction<DraggableItem> onItemClaimed;
        public UnityAction<DraggableItem> onItemReleased;
            
        void Awake()
        {
            isFree = true;
            
        }

        public void Claim(DraggableItem draggableItem, bool silent = false)
        {
            if (!isFree)
                return;

            draggableItem.transform.SetParent(transform);
            var rectTransform = draggableItem.GetComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            
            item = draggableItem;
            isFree = false;

            draggableItem.slot = this;

            if (!silent)
                onItemClaimed?.Invoke(item);
        }

        public DraggableItem Release(bool silent = false)
        {
            if (isFree)
                return null;
            item.slot = null;
            var tempItem = item;
            item = null;
            isFree = true;
            
            if (!silent)
                onItemReleased?.Invoke(tempItem);
            return tempItem;
        }
    }
}