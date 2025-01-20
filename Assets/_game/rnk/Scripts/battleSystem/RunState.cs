using System.Collections.Generic;
using _game.rnk.Scripts.artefacts;
using _game.rnk.Scripts.body;
using _game.rnk.Scripts.dice;
using _game.rnk.Scripts.weapons;

namespace _game.rnk.Scripts.battleSystem
{
    public class RunState
    {
        public CMSEntity battleLevel;
        public List<CharacterState> characters = new List<CharacterState>();
        public List<ArtefactState> inventory = new List<ArtefactState>();
        public List<EnemyState> enemies = new List<EnemyState>();
    }
    
    public class CharacterState : BaseCharacterState
    {
        public WeaponState weaponState;
    }
    
    public class EnemyState : BaseCharacterState
    {
        public bool backLine;
    }
    
    public abstract class BaseCharacterState
    {
        public int armor;
        public int maxHealth;
        public int health;
        public DiceZone diceZone;
        public BodyState bodyState;
        public List<DiceState> diceStates = new List<DiceState>();
    }

    public class WeaponState
    {
        public WeaponBase model;
    }

    public class BodyState
    {
        public BodyBase model;
    }

    public class DiceState
    {
        public int rollValue;
        public DiceInteractiveObject interactiveObject;
        public BaseCharacterState owner;
        public DiceBase model;
        public List<ArtefactState> artefacts = new List<ArtefactState>();
    }

    public class ArtefactState
    {
        public int slotIdx;
        public ArtefactBase model;
    }
}