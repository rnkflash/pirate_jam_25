using UnityEngine;

namespace _game.HeroInfo
{
    public class DiceFacePresenter
    {
        private IDiceFaceView _view;
        private DiceFaceModel _diceFaceModel;
        private bool _selected;
        public DiceFacePresenter(IDiceFaceView view, DiceFaceModel model)
        {
            _diceFaceModel = model;
            _view = view;
            _view.SetImage(model.sprite);
            _view.SetColor(model.colorValue);
            model.OnSelected += OnSelectedDice;
        }

        private void OnSelectedDice()
        {
            _selected = !_selected;
            _view.SetSelected(_selected);
        }

        public void Release()
        {
            _view.SetSelected(false);
        }
    }
}