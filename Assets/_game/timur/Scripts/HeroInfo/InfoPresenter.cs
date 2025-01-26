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
        private WeaponModel _currentWeaponModel;
        private int _currentWeaponSelected;

        public InfoPresenter(IInfoView view, Model model, InventoryPresenter inventoryPresenter)
        {
            _view = view;
            _model = model;
            _model.OnClickItem += OnClickItem;
            _inventoryPresenter = inventoryPresenter;
        }
        
        public void Show(int index, WeaponModel weaponModel)
        {
            if (index != _index || (index == _index && !_state))
            {
                _currentWeaponModel = weaponModel;
                _heroHealthPresenter = new HeroHealthPresenter(_view.GetHeroHealthView(), weaponModel.body.health);
                _index = index;
                _view.SetWeaponName(weaponModel.name);
                _view.SetWeaponImage(weaponModel.body.sprite);
                _view.SetDiceFacesView(weaponModel.diceFaces);
                foreach (var dicePresenter in _diceFacePresenters)
                {
                    //dicePresenter.Release();
                }
                _diceFacePresenters.Clear();
                for(int i = 0; i < weaponModel.diceFaces.Count; i++)
                {
                    //_diceFacePresenters.Add(new DiceFacePresenter(_view.GetDiceFaces()[i],weaponModel.diceFaces[i]));
                }

                _state = true;
                _view.SetShowState(true);
                _inventoryPresenter.Click();
            }
            else
            {
                foreach (var dicePresenter in _diceFacePresenters)
                {
                    //dicePresenter.Release();
                }
                _diceFacePresenters.Clear();
                _heroHealthPresenter = null;
                _state = false;
                _view.SetShowState(false);
                if (index == _index)
                {
                    _inventoryPresenter.Close();
                }
            }
        }

        private void OnClickItem(int indexItem)
        {
            var empties = _currentWeaponModel.diceFaces.FindAll(x => x.state == DiceFaceModel.FaceState.EMPTY);
            foreach (var empty in empties)
            {
                empty.OnSelected?.Invoke();
            }
        }
    }
}