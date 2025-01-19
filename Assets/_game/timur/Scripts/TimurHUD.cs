using System;
using System.Collections.Generic;
using _game.HeroInfo;
using DG.Tweening;
using UnityEngine;

namespace _game
{
    public class TimurHUD : MonoBehaviour
    {
        [SerializeField] private InfoView infoView;
        [SerializeField] private List<HeroPanelView> _heroPanelViews;
        
        [SerializeField] ItemDatabase _heroesDatabase;
        [SerializeField] ItemDatabase _weaponsDatabase;
        [SerializeField] private Transform _heroesPanel;
        private List<HeroPanelPresenter> _heroPanels = new();
        private SquadModel _squadModel;
        private InfoPresenter _infoPresenter;

        private void Awake()
        {
            List<WeaponModel> testHeroes = new();
            testHeroes.Add(new WeaponModel("Catalyst",  new BodyModel("Knight", 10, _heroesDatabase.items[0].image)));
            testHeroes.Add(new WeaponModel("Riffle", new BodyModel("Warrior", 15, _heroesDatabase.items[1].image)));
            testHeroes.Add(new WeaponModel("Sword", new BodyModel("Mage", 6, _heroesDatabase.items[2].image)));
            testHeroes.Add(new WeaponModel("Staff", new BodyModel("Thief", 3, _heroesDatabase.items[3].image)));
            _squadModel = new SquadModel(testHeroes);
            
            for (int i = 0; i < 4; i++)
            {
                var heroPanelPresenter = new HeroPanelPresenter(_heroPanelViews[i], _squadModel, i);
                _heroPanels.Add(heroPanelPresenter);
            }

            _infoPresenter = new InfoPresenter(infoView);
            _squadModel.OnClickWeapon += OnClickWeapon;
        }

        private void OnClickWeapon(int index, WeaponModel weaponModel)
        {
            _infoPresenter.Show(index, weaponModel);
        }

        public void Release()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var q = _heroesPanel.transform.position;
                q.y = -134f;
                _heroesPanel.transform.position = q;
                _heroesPanel.DOMoveY(134.43f, 0.2f)
                    .SetEase(Ease.OutBack);
            }
            //
            // if (Input.GetKeyDown(KeyCode.S))
            // {
            //     _testModel.health.Value = 10;
            // }
        }
    }
}