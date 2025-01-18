using System;
using System.Collections.Generic;
using UnityEngine;

namespace _game
{
    public class TimurHUD : MonoBehaviour
    {
        [SerializeField] private List<HeroPanelView> _heroPanelViews;
        private List<HeroPanelPresenter> _heroPanels = new();
        private HeroSquadModel _testSquad;

        public void Awake()
        {
            List<HeroPanelModel> testHeroes = new();
            testHeroes.Add(new HeroPanelModel("Catalyst", 5));
            testHeroes.Add(new HeroPanelModel("Riffle", 7));
            testHeroes.Add(new HeroPanelModel("Sword", 12));
            testHeroes.Add(new HeroPanelModel("Staff", 2));
            _testSquad = new HeroSquadModel(testHeroes);
            
            for (int i = 0; i < 4; i++)
            {
                var heroPanelPresenter = new HeroPanelPresenter(_heroPanelViews[i], _testSquad.heroes[i]); 
                _heroPanels.Add(heroPanelPresenter);
            }
        }

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     _testModel.health.Value = 1;
            // }
            //
            // if (Input.GetKeyDown(KeyCode.S))
            // {
            //     _testModel.health.Value = 10;
            // }
        }
    }
}