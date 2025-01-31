using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _game.rnk.Scripts.inventory
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        RectTransform rectTransform;
        CanvasGroup canvasGroup;
        public Canvas mainCanvas;
        Vector2 originalPosition;
        Transform originalParent;
        int originalSiblingIndex;
        [NonSerialized] public DroppableItemSlot slot;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            mainCanvas = FindObjectOfType<InventoryScreen>().GetComponent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            originalPosition = rectTransform.anchoredPosition;
            originalParent = transform.parent;
            originalSiblingIndex = transform.GetSiblingIndex();
        
            // Temporarily disable GridLayoutGroup during drag
            if (originalParent.GetComponent<GridLayoutGroup>())
                originalParent.GetComponent<GridLayoutGroup>().enabled = false;
            
            transform.SetParent(mainCanvas.transform); // Move to the top of the hierarchy
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.5f;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / transform.root.localScale.x;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var droppedOnObject = eventData.pointerCurrentRaycast.gameObject;
            if (droppedOnObject != null)
            {
                if (droppedOnObject.TryGetComponent<DroppableItemSlot>(out var droppedOnSlot))
                {
                    slot?.Release();
                    if (droppedOnSlot.isFree)
                        droppedOnSlot.Claim(this);
                    else
                        ReturnToOriginalPosition();
                }
                else if (slot!= null && droppedOnObject.TryGetComponent<DroppableInventoryPanel>(out var inventoryPanel))
                {
                    slot?.Release();
                    transform.SetParent(inventoryPanel.transform);
                }
                else
                    ReturnToOriginalPosition();
            }
            else
                ReturnToOriginalPosition();
            
            // Re-enable GridLayoutGroup after drag
            if (originalParent.GetComponent<GridLayoutGroup>())
                originalParent.GetComponent<GridLayoutGroup>().enabled = true;
            
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1.0f;
        }
        
        void ReturnToOriginalPosition()
        {
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
            transform.SetSiblingIndex(originalSiblingIndex);
        }
    }
}