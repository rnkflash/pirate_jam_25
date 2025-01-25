using System;
using System.Collections.Generic;
using _game.HeroInfo;
using _game.Inventory;
using _game.Utils;
using DG.Tweening;
using UnityEngine;

namespace _game
{
    public class TimurHUD : MonoBehaviour
    {
        [SerializeField] private InfoView infoView;
        [SerializeField] private InventoryView iventoryView;
        [SerializeField] private List<HeroPanelView> _heroPanelViews;
        
        [SerializeField] ItemDatabase _heroesDatabase;
        [SerializeField] ItemDatabase _weaponsDatabase;
        [SerializeField] ItemDatabase _diceDatabase;
        [SerializeField] private Transform _heroesPanel;
        [SerializeField] private ItemDatabase _itemsDatabase;
        private List<HeroPanelPresenter> _heroPanels = new();
        private Model _model;
        private InfoPresenter _infoPresenter;
        private InventoryPresenter _inventoryPresenter;

        private void Awake()
        {
            List<WeaponModel> testHeroes = new();
            List<DiceFaceModel> diceFaceModels0 = new()
            {
                new DiceFaceModel(_diceDatabase.items[6].sprite),
                new DiceFaceModel(1, UIUtils.blue, _diceDatabase.items[0].sprite),
                new DiceFaceModel(1, UIUtils.blue, _diceDatabase.items[0].sprite),
                new DiceFaceModel(3, UIUtils.blue, _diceDatabase.items[2].sprite),
                new DiceFaceModel(4, UIUtils.red, _diceDatabase.items[3].sprite),
                new DiceFaceModel(5, UIUtils.grey, _diceDatabase.items[4].sprite),
            };
            List<DiceFaceModel> diceFaceModels1 = new()
            {
                new DiceFaceModel(_diceDatabase.items[6].sprite),
                new DiceFaceModel(_diceDatabase.items[6].sprite),
                new DiceFaceModel(_diceDatabase.items[6].sprite),
                new DiceFaceModel(_diceDatabase.items[6].sprite),
                new DiceFaceModel(6, UIUtils.blue, _diceDatabase.items[5].sprite),
                new DiceFaceModel(6, UIUtils.red, _diceDatabase.items[5].sprite),
            };
            List<DiceFaceModel> diceFaceModels2 = new()
            {
                new DiceFaceModel(3, UIUtils.grey, _diceDatabase.items[2].sprite),
                new DiceFaceModel(4, UIUtils.grey, _diceDatabase.items[3].sprite),
                new DiceFaceModel(5, UIUtils.red, _diceDatabase.items[4].sprite),
                new DiceFaceModel(3, UIUtils.red, _diceDatabase.items[2].sprite),
                new DiceFaceModel(4, UIUtils.red, _diceDatabase.items[3].sprite),
                new DiceFaceModel(5, UIUtils.blue, _diceDatabase.items[4].sprite),
            };
            List<DiceFaceModel> diceFaceModels3 = new()
            {
                new DiceFaceModel(4, UIUtils.red, _diceDatabase.items[3].sprite),
                new DiceFaceModel(5, UIUtils.grey, _diceDatabase.items[4].sprite),
                new DiceFaceModel(3, UIUtils.grey, _diceDatabase.items[2].sprite),
                new DiceFaceModel(4, UIUtils.red, _diceDatabase.items[3].sprite),
                new DiceFaceModel(_diceDatabase.items[6].sprite),
                new DiceFaceModel(_diceDatabase.items[6].sprite),
            };
            testHeroes.Add(new WeaponModel("Catalyst", _weaponsDatabase.items[0].sprite,  
                new BodyModel("Knight", 10, _heroesDatabase.items[0].sprite),
                diceFaceModels0));
            testHeroes.Add(new WeaponModel("Riffle", _weaponsDatabase.items[1].sprite, 
                new BodyModel("Warrior", 15, _heroesDatabase.items[1].sprite),
                diceFaceModels1));
            testHeroes.Add(new WeaponModel("Sword", _weaponsDatabase.items[2].sprite,
                new BodyModel("Mage", 6, _heroesDatabase.items[2].sprite),
                diceFaceModels2));
            testHeroes.Add(new WeaponModel("Staff", _weaponsDatabase.items[3].sprite,
                new BodyModel("Thief", 3, _heroesDatabase.items[3].sprite),
                diceFaceModels3));

            List<InventoryItem> inventory = new List<InventoryItem>();
            inventory.Add(new InventoryItem(_itemsDatabase.items[0].id, "Item1", "some", _itemsDatabase.items[0].sprite));
            inventory.Add(new InventoryItem(_itemsDatabase.items[1].id, "Item1", "some", _itemsDatabase.items[1].sprite));
            inventory.Add(new InventoryItem(_itemsDatabase.items[2].id, "Item1", "some", _itemsDatabase.items[2].sprite));
            inventory.Add(new InventoryItem(_itemsDatabase.items[3].id, "Item1", "some", _itemsDatabase.items[3].sprite));
            _model = new Model(testHeroes, inventory);
            
            for (int i = 0; i < 4; i++)
            {
                var heroPanelPresenter = new HeroPanelPresenter(_heroPanelViews[i], _model, i);
                _heroPanels.Add(heroPanelPresenter);
            }
            
            _inventoryPresenter = new InventoryPresenter(infoView.GetInventoryView(), _model);
            _infoPresenter = new InfoPresenter(infoView, _model, _inventoryPresenter);
            _model.OnClickWeapon += OnClickWeapon;
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