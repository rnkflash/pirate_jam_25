using System;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.tags.actions;
using _game.rnk.Scripts.util;
using UnityEngine;
using UnityEngine.Events;

namespace _game.rnk.Scripts.inventory
{
    public class ItemSlot : MonoBehaviour
    {
        [NonSerialized] public DroppableItemSlot slot;
        [NonSerialized] public int index;

        public UnityAction<InventoryItem, int> onItemPut;
        public UnityAction<InventoryItem, int> onItemRemoved;

        void Awake()
        {
            slot = GetComponent<DroppableItemSlot>();

            slot.onItemClaimed += OnItemClaimed;
            slot.onItemReleased += OnItemReleased;
        }

        void OnDestroy()
        {
            slot.onItemClaimed -= OnItemClaimed;
            slot.onItemReleased -= OnItemReleased;
        }
        
        void OnItemClaimed(DraggableItem draggableItem)
        {
            onItemPut?.Invoke(draggableItem.GetComponent<InventoryItem>(), index);
        }
        void OnItemReleased(DraggableItem draggableItem)
        {
            onItemRemoved?.Invoke(draggableItem.GetComponent<InventoryItem>(), index);
        }

        public void SetFace(CMSEntity face)
        {
            var view = slot.GetComponent<FaceView>();

            if (face.Is<TagActionBlank>(out var blank))
            {
                view.valueText.text = "X";
                view.image.SetAlpha(0f);
            }
            else
            {
                if (face.Is<TagSprite>(out var sprite))
                {
                    view.image.SetAlpha(1f);
                    view.image.sprite = sprite.sprite;
                }
                else
                {
                    view.image.SetAlpha(0f);
                }
                
                if (face.Is<TagValue>(out var tagValue))
                {
                    view.valueText.text = face.WithTagValues();
                }
                else
                {
                    view.valueText.text = "X";
                }
            }
        }
        public void Free()
        {
            var item = slot.Release(true);
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
    }
}