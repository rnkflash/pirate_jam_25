using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.artefacts;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.crawler;
using _game.rnk.Scripts.dice;
using _game.rnk.Scripts.dice.face;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.weapons;
using UnityEngine;

namespace _game.rnk.Scripts
{
    public class RunState
    {
        public BattleEncounter battle;
        public List<CharacterState> characters = new List<CharacterState>();
        public List<ArtefactState> inventory = new List<ArtefactState>();
        public List<EnemyState> enemies = new List<EnemyState>();
    }
    
    public class CharacterState : BaseCharacterState
    {
        public WeaponState weaponState;
        public CharacterView view;
        public override MonoBehaviour GetView() => view;
        public override BaseCharacterState GetState() => this;
    }
    
    public class EnemyState : BaseCharacterState
    {
        public GameObject graphic;
        public Transform uiPos;
        public EnemyView view;
        public override MonoBehaviour GetView() => view;
        public override BaseCharacterState GetState() => this;
    }
    
    public abstract class BaseCharacterState: ITarget
    {
        public bool dead;
        public bool backLine;
        public int armor;
        public int maxHealth;
        public int health;
        public DiceZone diceZone;
        public BodyState bodyState;
        public List<DiceState> diceStates = new List<DiceState>();
        public abstract MonoBehaviour GetView();
        public abstract BaseCharacterState GetState();
        public bool IsBackLine() => backLine;
    }

    public interface ITarget
    {
        public MonoBehaviour GetView();
        public BaseCharacterState GetState();
        public bool IsBackLine();
    }

    public class WeaponState
    {
        public WeaponBase model;
    }

    public class BodyState
    {
        public CMSEntity model;
    }

    public class DiceState
    {
        public BaseCharacterState owner;
        public CMSEntity model;
        
        public int rollValue;
        public DiceInteractiveObject interactiveObject;
        public List<ArtefactState> artefacts = new List<ArtefactState>();

        public CMSEntity face => model.Get<TagDefaultFaces>().faces.ElementAtOrDefault(rollValue) ?? new BlankFace();
    }

    public class ArtefactState
    {
        public int slotIdx;
        public ArtefactBase model;
    }
}