using System;
using UnityEngine;
using UnityEngine.UI;

namespace _game.Inventory
{
    public class InventoryItemView : MonoBehaviour, IInventoryItemView
    {
        [SerializeField] private Image _image;

        public void OnItemClicked()
        {
            OnClick?.Invoke();
        }

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public event Action OnClick;
    }

    public interface IInventoryItemView
    {
        void SetSprite(Sprite sprite);
        event Action OnClick;
    }
}