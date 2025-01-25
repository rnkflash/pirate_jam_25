using System;
using UnityEngine;

namespace _game
{
    public class HeroPanelPresenter
    {
        private IHeroPanelView _view;
        public HeroHealthPresenter heroHealthPresenter { get; }
        private Model _model;
        private int _index;
        private bool _selected;

        public HeroPanelPresenter(IHeroPanelView view, Model model, int index)
        {
            _index = index;
            _model = model;
            _model.OnClickWeapon += OnHeroClickk;
            _view = view;
            _view.SetName(_model.weapons[index].name);
            _view.SetSprite(_model.weapons[index].body.sprite);
            _view.OnClick += OnHeroClick;
            heroHealthPresenter = new HeroHealthPresenter(_view.GetHeroHealthView(), _model.weapons[index].body.health);
        }

        private void OnHeroClickk(int index, WeaponModel _)
        {
            if (index != _index && _selected)
            {
                _selected = !_selected;
                _view.SetState(_selected);
                _view.SetSelectedVariable(_selected);
            }
        }

        private void OnHeroClick()
        {
            _selected = !_selected;
            _view.SetState(_selected);
            _view.SetSelectedVariable(_selected);
            _model.ClickOnWeapon(_index);
        }

        public void Release()
        {
            _view.OnClick -= OnHeroClick;
        }
    }
}