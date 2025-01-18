using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace _game
{
    [Serializable]
    public class HeroPanelModel
    {
        public HeroPanelModel(string name, int health)
        {
            this.name = name;
            this.health = new ReactiveVar<int>(health);
            diceFaces = new DiceList(new List<DiceFaceModel>());
        }
        public string name;
        public Image image;
        public ReactiveVar<int> health;
        public DiceList diceFaces;
    }

    [Serializable]
    public class HeroSquadModel
    {
        public HeroSquadModel(List<HeroPanelModel> heroes)
        {
            this.heroes = heroes;
        }
        public List<HeroPanelModel> heroes;
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

        public DiceFaceModel(int value, FaceValueType valueType, Image colorImage)
        {
            state = FaceState.VALUE;
            this.value = value;
            this.colorImage = colorImage;
        }
        
        public FaceState state;
        public DiceArtefactModel artefact;
        public int? value;
        public Image image;
        public Image colorImage;
        
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