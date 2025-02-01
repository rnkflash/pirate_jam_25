using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.crawler;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.ui;
using UnityEngine;

namespace _game.rnk.Scripts
{
    public class RunState
    {
        public BattleEncounter battle;
        public List<CharacterState> characters = new List<CharacterState>();
        public List<ArtefactState> inventory = new List<ArtefactState>();
        public List<EnemyState> enemies = new List<EnemyState>();
        public List<BuffState> buffs = new List<BuffState>();
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
        public Enemy objInScene;
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

    public class BuffState
    {
        public int turnsLeft;
        public ITarget target;
        public BaseCharacterState castedBy;
        public CMSEntity model;

        public BuffView view;
    }

    public interface ITarget
    {
        public MonoBehaviour GetView();
        public BaseCharacterState GetState();
        public bool IsBackLine();
    }

    public class WeaponState
    {
        public CMSEntity model;
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

        public int sides => model.Get<TagSides>().sides;
        public CMSEntity face => GetFace(rollValue);
        public CMSEntity[] faces => Enumerable.Range(0, sides).Select(i => GetFace(i)).ToArray();
        public CMSEntity GetFace(int index) => model.Get<TagDefaultFaces>().faces.ElementAtOrDefault(index) ?? model.Get<TagDefaultFaces>().faces.Last();

        public ArtefactState artefactOnFace() => artefactOnFace(rollValue);
        public ArtefactState artefactOnFace(int index) => artefacts.FirstOrDefault(state => state.slot == index);

        public CMSEntity overridenFace => artefactOnFace()?.face ?? face;
    }

    public class ArtefactState
    {
        public int slot;
        public CMSEntity model;

        public CMSEntity face => model.Get<TagOverrideFace>()?.face;
    }
}