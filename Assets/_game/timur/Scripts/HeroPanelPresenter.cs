using System;
using UnityEngine;

namespace _game
{
    public class HeroPanelPresenter
    {
        private IHeroPanelView _view;
        public HeroHealthPresenter heroHealthPresenter { get; }
        private SquadModel _squadModel;
        private int _index;

        public HeroPanelPresenter(IHeroPanelView view, SquadModel squadModel, int index)
        {
            _index = index;
            _squadModel = squadModel;
            _view = view;
            _view.SetName(_squadModel.weapons[index].name);
            _view.SetSprite(_squadModel.weapons[index].body.sprite);
            _view.OnClick += OnHeroClick;
            heroHealthPresenter = new HeroHealthPresenter(_view.GetHeroHealthView(), _squadModel.weapons[index].body.health);
        }

        private void OnHeroClick()
        {
            Debug.Log("timur hero click");
            _squadModel.ClickOnWeapon(_index);
        }

        public void Release()
        {
            _view.OnClick -= OnHeroClick;
        }
    }
}