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
    
    public class CharacterState
    {
        public int health;
        public DiceZone zone;
        public WeaponState weaponState;
        public BodyState bodyState;
        public List<DiceState> diceStates = new List<DiceState>();
        public CharacterView view;
    }
    
    public class EnemyState
    {
        public bool backLine;
        public int health;
        public DiceZone zone;
        public BodyState bodyState;
        public List<DiceState> diceStates = new List<DiceState>();
        //public EnemyView view;
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
        public DiceInteractiveObject interactiveObject;
        public CharacterState owner;
        public DiceBase model;
        public List<ArtefactState> artefacts = new List<ArtefactState>();
    }

    public class ArtefactState
    {
        public ArtefactBase model;
    }
}