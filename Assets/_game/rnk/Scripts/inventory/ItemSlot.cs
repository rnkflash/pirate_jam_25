using System;
using _game.rnk.Scripts.tags;
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
            this.index = index;
            var view = slot.GetComponent<FaceView>();
                
            if (face.Is<TagSprite>(out var sprite))
            {
                view.image.SetAlpha(1f);
                view.image.sprite = sprite.sprite;
            }
            else
            {
                view.image.SetAlpha(0f);
            }
            
            if (face.Is<TagAction>(out var action))
            {
                if (action.action == ActionType.Blank)
                    view.valueText.text = "X";
                else
                    view.valueText.text = face.Get<TagValue>().value.ToString();
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