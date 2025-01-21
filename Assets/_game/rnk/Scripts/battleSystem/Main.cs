using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.enums;
using _game.rnk.Scripts.interactor;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _game.rnk.Scripts.battleSystem
{

    public class Main : MonoBehaviour
    {
        public DiceZone rollDicesZone;
        public List<GameObject> partsOfHudToDisable;

        public Interactor interactor;

        public Transform charactersRoot;
        public Transform enemiesRoot;
    
        public CMSEntity level = new TestBattleLevel();

        CharacterView characterViewPrefab;
        EnemyView enemyViewPrefab;

        [NonSerialized] public int reRolls;
        [NonSerialized] public TurnPhase turnPhase;
        
        bool isWin;
        bool skip;
        
        void Awake()
        {
            characterViewPrefab = "prefab/CharacterView".Load<CharacterView>();
            enemyViewPrefab = "prefab/EnemyView".Load<EnemyView>();
            
            interactor = new Interactor();
            interactor.Init();

            if (G.run == null)
            {
                G.run = new RunState();
                G.run.battleLevel = new TestBattleLevel();
            }
            else
            {
                G.run.characters.Clear();
                G.run.inventory.Clear();
            }
            
            rollDicesZone.OnClickDice += OnClickDiceInRollzone;

            G.main = this;
        }

        void OnDestroy()
        {
            rollDicesZone.OnClickDice -= OnClickDiceInRollzone;
        }

        IEnumerator Start()
        {
            CMS.Init();

            G.hud.DisableHud();
            G.ui.DisableInput();

            G.fader.FadeOut();
        
            yield return G.ui.Unsay();

            if (G.run.battleLevel != null)
                yield return LoadLevel(G.run.battleLevel);
        
            G.ui.EnableInput();
            G.hud.EnableHud();

            SetTurnPhase(TurnPhase.START_TURN);
        }

        void SetTurnPhase(TurnPhase newTurnPhase)
        {
            Debug.Log("SetTurnPhase " + newTurnPhase);
            
            turnPhase = newTurnPhase;

            switch (turnPhase)
            {
                case TurnPhase.START_TURN:
                    reRolls = 2;
                    G.hud.RollButtonText.text = "Reroll x " + reRolls;
                    G.hud.EndTurnButtonText.text = "Next phase";
                    G.hud.DisableHud();
                    SetTurnPhase(TurnPhase.ENEMY_ROLL);
                    break;
                
                case TurnPhase.ENEMY_ROLL:
                    StartCoroutine(EnemyTurn());
                    break;

                case TurnPhase.FREE_ROLL:
                    StartCoroutine(PlayerFreeRoll());
                    break;

                case TurnPhase.RE_ROLL:
                    G.hud.EnableHud();
                    
                    break;
                
                case TurnPhase.ENEMY_PLAY:
                    StartCoroutine(EnemyPlay());
                    break;

                case TurnPhase.PLAY_DICES:
                    StartCoroutine(StartPlayDicesPhase());
                    break;

                case TurnPhase.EXECUTE_DICES:
                    StartCoroutine(StartExecutePhase());
                    
                    break;

                case TurnPhase.CHECK_WIN:
                    SetTurnPhase(TurnPhase.START_TURN);
                    break;
            }
        }
        IEnumerator EnemyPlay()
        {
            G.hud.DisableHud();
            var enemyDices = G.run.enemies.SelectMany(
                cs => cs.diceStates.Select(ds => ds.interactiveObject)
            ).Reverse().ToList();

            yield return new WaitForSeconds(1.0f);
            
            //each enemy dice plays with random target

            //next phase
            SetTurnPhase(TurnPhase.PLAY_DICES);
            
        }
        IEnumerator PlayerFreeRoll()
        {
            var dices = G.run.characters.SelectMany(
                cs => cs.diceStates.Select(ds => ds.interactiveObject)
            ).Reverse().ToList();

            yield return ThrowDices(dices);

            yield return new WaitForSeconds(0.2f);
            
            yield return RollDices(dices);
            
            yield return new WaitForSeconds(0.2f);
            
            SetTurnPhase(TurnPhase.RE_ROLL);
        }
        
        IEnumerator EnemyTurn()
        {
            var enemyDices = G.run.enemies.SelectMany(
                cs => cs.diceStates.Select(ds => ds.interactiveObject)
            ).Reverse().ToList();

            yield return ThrowDices(enemyDices);

            yield return new WaitForSeconds(0.2f);
            
            yield return RollDices(enemyDices);
            
            yield return new WaitForSeconds(0.2f);
            
            yield return ReturnDices(enemyDices);
            
            //each enemy dice plays with random target

            //next phase
            SetTurnPhase(TurnPhase.FREE_ROLL);

        }
        
        IEnumerator StartExecutePhase()
        {
            G.hud.DisableHud();

            yield return ReturnAllDices();

            yield return new WaitForSeconds(0.5f);
            
            G.camera.UIHit();
            
            yield return new WaitForSeconds(1);
            
            SetTurnPhase(TurnPhase.CHECK_WIN);

        }
        
        IEnumerator StartPlayDicesPhase()
        {
            G.hud.EndTurnButtonText.text = "Execute";
            G.hud.DisableHud();

            yield return ReturnAllDices();

            G.hud.Enable(G.hud.EndTurnButton);
        }

        List<DiceInteractiveObject> GetDicesOnRollZOne()
        {
            return G.main.rollDicesZone.objects.Select(o => o).ToList();
        }
        
        public IEnumerator ReturnAllDices()
        {
            yield return ReturnDices(GetDicesOnRollZOne());
        }

        public void EndTurnButton()
        {
            switch (turnPhase)
            {
                case TurnPhase.RE_ROLL:
                    SetTurnPhase(TurnPhase.ENEMY_PLAY);
                    break;

                case TurnPhase.PLAY_DICES:
                    SetTurnPhase(TurnPhase.EXECUTE_DICES);
                    break;
            }
        }
        
        public void ReRollButton()
        {
            switch (turnPhase)
            {
                case TurnPhase.RE_ROLL:
                {
                    if (reRolls > 0)
                    {
                        reRolls--;
                        G.hud.RollButtonText.text = "Reroll x " + reRolls;
                        if (reRolls <= 0)
                            G.hud.Disable(G.hud.RollButton);
                    
                        StartCoroutine(RollDices(GetDicesOnRollZOne()));
                    }
                    break;
                }
            }
        }

        public IEnumerator PlayDice(DiceState diceState)
        {
            var onPlays = interactor.FindAll<IOnPlayDice>();
            foreach (var onPlay in onPlays)
            {
                yield return onPlay.OnPlayDice(diceState);
            }
        }

        IEnumerator EndTurnCoroutine()
        {
            var onEndTurns = interactor.FindAll<IOnEndTurn>();
            foreach (var onEndTurn in onEndTurns)
            {
                yield return onEndTurn.OnEndTurn();
            }
        }


        public void StartDrag(DraggableSmoothDamp draggableSmoothDamp)
        {
            G.drag_dice = draggableSmoothDamp.GetComponent<DiceInteractiveObject>();
            G.audio.Play<SFX_Animal>();
        }

        public void StopDrag()
        {
            G.drag_dice = null;
        }
        
        void OnClickDiceInRollzone(DiceInteractiveObject dice)
        {
            switch (turnPhase)
            {
                case TurnPhase.RE_ROLL:
                    StartCoroutine(ReturnDices(new List<DiceInteractiveObject>() { dice }));
                    break;

                case TurnPhase.PLAY_DICES:
                    
                    break;
            }
        }

        IEnumerator ThrowDices(List<DiceInteractiveObject> dices)
        {
            if (dices.Count == 0) yield break;
            
            yield return new WaitForEndOfFrame();
            G.audio.Play<SFX_DiceDraw>();
            foreach (var dice in dices)
            {
                MoveDice(dice, rollDicesZone);
                dice.Punch();
            }
            
            yield return new WaitUntil(() => {
                return dices.All(d => !d.moveable.IsMoving());
            });
        }
        
        IEnumerator ReturnDices(List<DiceInteractiveObject> dices)
        {
            if (dices.Count == 0) yield break;
            
            yield return new WaitForEndOfFrame();
            G.audio.Play<SFX_DiceDraw>();
            foreach (var dice in dices)
            {
                MoveDice(dice, dice.state.owner.diceZone);
                dice.Punch();
            }

            yield return new WaitUntil(() => {
                return dices.All(d => !d.moveable.IsMoving());
            });
        }

        IEnumerator RollDices(List<DiceInteractiveObject> dices)
        {
            if (dices.Count == 0) yield break;
            yield return this.WaitAll(dices.Select(d => d.Roll()).ToArray());
        }
        
        void MoveDice(DiceInteractiveObject dice, DiceZone zone)
        {
            dice.transform.SetParent(zone.transform);
            zone.Claim(dice);
        }

        public IEnumerator LoadLevel(CMSEntity entity)
        {
            level = entity;

            if (level.Is<TagExecuteScript>(out var exs))
            {
                yield return exs.toExecute();
            }
            
            foreach (var characterState in G.run.characters)
            {
                CreateCharacterView(characterState);
            }
            
            foreach (var enemyState in G.run.enemies)
            {
                CreateEnemyView(enemyState);
            }
        }

        public CharacterView CreateCharacterView(CharacterState characterState)
        {
            var character = Instantiate(characterViewPrefab, charactersRoot);
            character.SetState(characterState);
            return character;
        }
        
        public EnemyView CreateEnemyView(EnemyState enemyState)
        {
            var enemy = Instantiate(enemyViewPrefab, enemiesRoot);
            enemy.SetState(enemyState);
            return enemy;
        }

        void Update()
        {
            foreach (var poh in partsOfHudToDisable)
                poh.SetActive(G.hud.gameObject.activeSelf);

            if (Input.GetMouseButtonDown(0))
            {
                skip = true;
            }

            G.ui.debug_text.text = "";

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.R))
            {
                G.run.battleLevel = new TestBattleLevel();
                SceneManager.LoadScene(GameSettings.MAIN_SCENE);
            }
#endif
        }
        
        public void ShowHud()
        {
            G.hud.gameObject.SetActive(true);
        }
    
        public void HideHud()
        {
            G.hud.gameObject.SetActive(false);
        }

        public IEnumerator SmartWait(float f)
        {
            skip = false;
            while (f > 0 && !skip)
            {
                f -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        public void EnemyClicked(EnemyState state)
        {
            Debug.Log("enemy was clicked " + state.bodyState.model.Get<TagName>().loc);
        }
        public void CharacterClicked(CharacterState state)
        {
            Debug.Log("character was clicked " + state.weaponState.model.Get<TagName>().loc);
        }
        public void OnDiceClickInCharacterView(CharacterView view, DiceInteractiveObject dice)
        {
            switch (turnPhase)
            {
                case TurnPhase.RE_ROLL:
                    StartCoroutine(ThrowDices(new List<DiceInteractiveObject>() { dice })); 
                    break;

                case TurnPhase.PLAY_DICES:
                    
                    //get dice face
                    //get targets
                    //highlight targets
                    //wait for click target or elsewhere
                    //if click target set dice target and throw dice
                    //if dice is clicked in rollzone it resets target and returned to owner
                    //
                    
                    break;
            }
        }
        public void OnDiceClickInEnemyView(EnemyView enemyView, DiceInteractiveObject dice)
        {
            
        }
    }

    public class Lifetime : MonoBehaviour
    {
        public float ttl = 5f;

        void Update()
        {
            ttl -= Time.deltaTime;

            if (ttl < 0)
                Destroy(gameObject);
        }
    }
}