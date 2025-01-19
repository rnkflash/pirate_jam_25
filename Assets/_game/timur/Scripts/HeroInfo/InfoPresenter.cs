using System.Collections.Generic;
using _game.Inventory;
using UnityEngine;

namespace _game.HeroInfo
{
    public class InfoPresenter
    {
        private IInfoView _view;
        private List<DiceFacePresenter> _diceFacePresenters = new();
        private int _index = -1;
        private bool _state = false;
        private HeroHealthPresenter _heroHealthPresenter;
        private InventoryPresenter _inventoryPresenter;
        private Model _model;

        public InfoPresenter(IInfoView view, Model model )
        {
            _view = view;
            _view.OnInventoryClicked += OnInventoryClicked;
            _model = model;
            _inventoryPresenter = new InventoryPresenter(_view.GetInventoryView(), _model);
        }

        private void OnInventoryClicked()
        {
            _inventoryPresenter.Click();
        }

        public void Show(int index, WeaponModel weaponModel)
        {
            if (index != _index || (index == _index && !_state))
            {
                _heroHealthPresenter = new HeroHealthPresenter(_view.GetHeroHealthView(), weaponModel.body.health);
                _index = index;
                _view.SetWeaponName(weaponModel.name);
                _view.SetWeaponImage(weaponModel.body.sprite);
                _view.SetDiceFacesView(weaponModel.diceFaces);
                for(int i = 0; i < weaponModel.diceFaces.Count; i++)
                {
                    _diceFacePresenters.Add(new DiceFacePresenter(_view.GetDiceFaces()[i],weaponModel.diceFaces[i]));
                }

                _state = true;
                _view.SetShowState(true);
            }
            else
            {
                _heroHealthPresenter = null;
                _state = false;
                _view.SetShowState(false);  
                _inventoryPresenter.Close();
            }
        }
    }
}