using UnityEngine;

namespace _game.Inventory
{
    public class InventoryItemPresenter
    {
        private IInventoryItemView _view;
        private Model _model;
        private int _id;
        public InventoryItemPresenter(IInventoryItemView view, Model model, int id)
        {
            _model = model;
            _view = view;
            _id = id;
            _view.OnClick += OnItemClicked;
            _view.SetSprite(_model.inventory.Find(x => x.id == id).sprite);
        }

        private void OnItemClicked()
        {
            _model.ClickOnItem(_id);
        }

        public void Release()
        {
            _view.OnClick -= OnItemClicked;
        }
    }
}