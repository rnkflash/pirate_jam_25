using UnityEngine;

namespace _game.Inventory
{
    public class InventoryView : MonoBehaviour, IInventoryView
    {
        [SerializeField] private InventoryItemView _item;
        [SerializeField] private Transform _itemHolderTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        public InventoryItemView GetItemPrefab() => _item;
        public Transform GetItemsParent() => _itemHolderTransform;
        public void SetState(bool state)
        {
            _canvasGroup.interactable = state;
            _canvasGroup.blocksRaycasts = !state;
            _canvasGroup.alpha = state ? 1f : 0f;
        }
    }

    public interface IInventoryView
    {
        InventoryItemView GetItemPrefab();
        Transform GetItemsParent();
        void SetState(bool state);
    }
}