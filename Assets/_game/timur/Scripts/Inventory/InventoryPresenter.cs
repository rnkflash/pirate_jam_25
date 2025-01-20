using System.Collections.Generic;
using UnityEngine;

namespace _game.Inventory
{
    public class InventoryPresenter
    {
        private IInventoryView _view;
        private List<InventoryItemPresenter> _items = new();
        private bool _selected = false;
        public InventoryPresenter(IInventoryView view, Model model)
        {
            _view = view;
            for (int i = 0; i < model.inventory.Count; i++)
            {
                var q = GameObject.Instantiate(_view.GetItemPrefab(), _view.GetItemsParent());
                _items.Add(new InventoryItemPresenter(q.GetComponent<InventoryItemView>(), model, model.inventory[i].id));
            }
        }

        public void Click()
        {
            _selected = !_selected;
            _view.SetState(_selected);
        }

        public void Close()
        {
            _selected = false;
            _view.SetState(false);
        }
    }
}