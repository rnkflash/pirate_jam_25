using System.Collections.Generic;
using UnityEngine;

namespace _game.HeroInfo
{
    public class InfoPresenter
    {
        private IInfoView _view;
        private List<DiceFacePresenter> _diceFacePresenters = new();
        private int _index = -1;
        private bool _state = false;

        public InfoPresenter(IInfoView view)
        {
            _view = view;
        }

        public void Show(int index, WeaponModel weaponModel)
        {
            if (index != _index || (index == _index && !_state))
            {
                _index = index;
                _view.SetWeaponName(weaponModel.name);
                _view.SetWeaponImage(weaponModel.image);
                _view.SetDiceFacesView(weaponModel.diceFaces);
                for(int i = 0; i < weaponModel.diceFaces.diceFaces.Count; i++)
                {
                    _diceFacePresenters.Add(new DiceFacePresenter(_view.GetDiceFaces()[i],weaponModel.diceFaces.diceFaces[i]));
                }

                _state = true;
                _view.SetShowState(true);
            }
            else
            {
                _state = false;
                _view.SetShowState(false);   
            }
        }
    }
}