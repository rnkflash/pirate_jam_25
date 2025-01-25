using System;
using _game.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace _game.Inventory
{
    public class InventoryItemView : MonoBehaviour, IInventoryItemView
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _selectedImg;

        public void OnItemClicked()
        {
            OnClick?.Invoke();
        }

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public event Action OnClick;
        public void SetSelected(bool selected)
        {
            if (selected)
            {
                _selectedImg.color = UIUtils.unselectedFrame;
            }
            else
            {
                _selectedImg.color = UIUtils.selectedFrame;
            }
        }
    }

    public interface IInventoryItemView
    {
        void SetSprite(Sprite sprite);
        event Action OnClick;
        void SetSelected(bool selected);
    }
}