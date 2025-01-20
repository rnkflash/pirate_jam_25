using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _game
{
    public class Model : ISquadModel
    {
        public Model(List<WeaponModel> weapons, List<InventoryItem> inventory)
        {
            this.weapons = weapons;
            this.inventory = inventory;
        }
        
        public List<WeaponModel> weapons;
        public List<InventoryItem> inventory; 
        
        public void ClickOnWeapon(int index)
        {
            OnClickWeapon?.Invoke(index, weapons[index]);
        }

        public void ClickOnItem(int id)
        {
            OnClickItem?.Invoke(id);
        }

        public event Action<int, WeaponModel> OnClickWeapon;
        public event Action<int> OnClickItem;
    }
    
    public interface ISquadModel
    {
        //все изменения извне происходит через этот интерфейс
    }
    
    [Serializable]
    public class WeaponModel
    {
        public WeaponModel(string name, Sprite sprite, BodyModel bodyModel, List<DiceFaceModel> diceFaceModels)
        {
            this.name = name;
            this.image = sprite;
            diceFaces = diceFaceModels;
            body = bodyModel;
        }
        public string name;
        public Sprite image;
        public BodyModel body;
        public List<DiceFaceModel> diceFaces;
    }

    [Serializable]
    public class BodyModel
    {
        public BodyModel(string name, int health, Sprite sprite)
        {
            this.name = name;
            this.health = new ReactiveVar<int>(health);
            this.sprite = sprite;
        }
        
        public string name;
        public Sprite sprite;
        public ReactiveVar<int> health;
    }

    [Serializable]
    public class DiceFaceModel
    {
        public FaceState state;
        public DiceArtefactModel artefact;
        public int? value;
        public Sprite sprite;
        public Color colorValue;
        
        public DiceFaceModel( Sprite sprite)
        {
            state = FaceState.EMPTY;
            this.sprite = sprite;
        }

        public DiceFaceModel(DiceArtefactModel artefact)
        {
            state = FaceState.ARTIFACT;
            this.artefact = artefact;
            sprite = artefact.sprite;
        }

        public DiceFaceModel(int value, Color color, Sprite sprite)
        {
            state = FaceState.VALUE;
            this.value = value;
            this.sprite = sprite;
            colorValue = color;
        }
        
        public enum FaceState
        {
            EMPTY,
            ARTIFACT,
            VALUE
        }

        public class DiceArtefactModel
        {
            public string name;
            public string description;
            public Sprite sprite;
        }
    }

    public class InventoryItem
    {
        public InventoryItem(int id, string name, string description, Sprite sprite)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.sprite = sprite;
        }
        
        public int id;
        public string name;
        public string description;
        public Sprite sprite;
    }
}