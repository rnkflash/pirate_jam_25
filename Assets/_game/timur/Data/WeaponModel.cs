using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _game
{
    public class SquadModel : ISquadModel
    {
        public SquadModel(List<WeaponModel> weapons)
        {
            this.weapons = weapons;
            
        }
        
        public List<WeaponModel> weapons;
        
        public void ClickOnWeapon(int index)
        {
            OnClickWeapon?.Invoke(index, weapons[index]);
        }

        public event Action<int, WeaponModel> OnClickWeapon;
    }
    
    public interface ISquadModel
    {
        //все изменения извне происходит через этот интерфейс
    }
    
    [Serializable]
    public class WeaponModel
    {
        public WeaponModel(string name, BodyModel bodyModel)
        {
            this.name = name;
            diceFaces = new DiceList(new List<DiceFaceModel>());
            body = bodyModel;
        }
        public string name;
        public Sprite image;
        public BodyModel body;
        public DiceList diceFaces;
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

    public class DiceList
    {
        public DiceList(List<DiceFaceModel> diceFaces)
        {
            this.diceFaces = diceFaces;
        }
        
        public List<DiceFaceModel> diceFaces;
    }

    [Serializable]
    public class DiceFaceModel
    {
        public FaceState state;
        public DiceArtefactModel artefact;
        public int? value;
        public Image image;
        public Color colorValue;
        
        public DiceFaceModel()
        {
            state = FaceState.EMPTY;
        }

        public DiceFaceModel(DiceArtefactModel artefact)
        {
            state = FaceState.ARTIFACT;
            this.artefact = artefact;
            image = artefact.Image;
        }

        public DiceFaceModel(int value, FaceValueType valueType, Color colorValue)
        {
            state = FaceState.VALUE;
            this.value = value;
            this.colorValue = colorValue;
        }
        
        public enum FaceState
        {
            EMPTY,
            ARTIFACT,
            VALUE
        }
        
        public enum FaceValueType
        {
            GREY,
            RED,
            BLUE
        }

        public class DiceArtefactModel
        {
            public string name;
            public string description;
            public Image Image;
        }
    }
}