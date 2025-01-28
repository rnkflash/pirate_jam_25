using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.enums;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _game.rnk.Scripts
{

    public class Battle : MonoBehaviour
    {
        public DiceZone rollDicesZone;
        public Interactor interactor;
        public CMSEntity level;
        
        [NonSerialized] public int reRolls;
        [NonSerialized] public TurnPhase turnPhase;
        
        bool isWin;
        bool skip;
        bool isEnabled;
        
        void Awake()
        {
            G.battle = this;

            interactor = new Interactor();
            interactor.Init();

            rollDicesZone.OnClickDice += OnDiceClickedInRollzone;
        }

        void Start()
        {
            //SetTurnPhase(TurnPhase.START_TURN);
        }
        
        void OnDestroy()
        {
            rollDicesZone.OnClickDice -= OnDiceClickedInRollzone;
        }
        
        public IEnumerator LoadLevel(CMSEntity entity)
        {
            level = entity;

            if (level.Is<tags.TagExecuteScript>(out var exs))
            {
                yield return exs.toExecute();
            }
        }

        void Update()
        {
            if (!isEnabled)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                skip = true;
            }

            G.ui.debug_text.text = "";

            if (Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftControl))
            {
                isWin = true;
                EndTurn();
            }
            
            if (Input.GetKeyDown(KeyCode.L) && Input.GetKeyDown(KeyCode.LeftControl))
            {
                StartCoroutine(LoseSequence());
            }
        }

        void SetTurnPhase(TurnPhase newTurnPhase)
        {
            Debug.Log("SetTurnPhase " + newTurnPhase);
            
            turnPhase = newTurnPhase;

            switch (turnPhase)
            {
                case TurnPhase.START_TURN:
                    foreach (var c in G.run.characters)
                    {
                        c.armor = 0;
                        c.GetView().GetComponent<Damageable>().UpdateView();
                    }
                    foreach (var c in G.run.enemies)
                    {
                        c.armor = 0;
                        c.GetView().GetComponent<Damageable>().UpdateView();
                    }
                    
                    reRolls = 2;
                    G.hud.battlehud.RollButtonText.text = "Reroll x " + reRolls;
                    G.hud.battlehud.EndTurnButtonText.text = "Next phase";
                    G.hud.battlehud.DisableHud();
                    SetTurnPhase(TurnPhase.ENEMY_ROLL);
                    break;
                
                case TurnPhase.ENEMY_ROLL:
                    StartCoroutine(EnemyTurn());
                    break;

                case TurnPhase.FREE_ROLL:
                    StartCoroutine(PlayerFreeRoll());
                    break;

                case TurnPhase.RE_ROLL:
                    G.hud.battlehud.EnableHud();
                    break;
                
                case TurnPhase.ENEMY_TARGETING:
                    StartCoroutine(EnemyPickTargets());
                    break;

                case TurnPhase.PLAYER_TARGETING:
                    StartCoroutine(StartPlayDicesPhase());
                    break;

                case TurnPhase.EXECUTE_DICES:
                    StartCoroutine(StartExecutePhase());
                    
                    break;

                case TurnPhase.CHECK_WIN:
                    isWin = G.run.enemies.FindAll(state => !state.dead).Count == 0;
                    if (G.run.characters.All(state => state.dead))
                        StartCoroutine(LoseSequence());
                    else
                        EndTurn();
                    break;
            }
        }

        void EndTurn()
        {
            if (!isWin)
            {
                SetTurnPhase(TurnPhase.START_TURN);
            }
            else
            {
                StartCoroutine(WinSequence());
            }
        }

        IEnumerator WinSequence()
        {
            
            G.hud.battlehud.DisableHud();

            isWin = true;

            G.ui.win.SetActive(true);

            yield return new WaitForSeconds(1.22f);

            G.ui.win.SetActive(false);
                
            G.fader.FadeIn();

            yield return new WaitForSeconds(1f);
                
            G.run.battleLevel = new NextBattleLevel();
                
            SceneManager.LoadScene(GameSettings.MAIN_SCENE);
        }

        IEnumerator LoseSequence()
        {
            G.audio.Play<SFX_Lose>();
            G.camera.UIHit();
            G.ui.defeat.SetActive(true);
            yield return new WaitForSeconds(5f);
            G.run = null;
            SceneManager.LoadScene(GameSettings.MAIN_SCENE);
        }

        (List<ITarget> allies, List<ITarget> enemies) GetAllTargets(Type baseCharacterState)
        {
            var allies = new List<ITarget>();
            var enemies = new List<ITarget>();
            if (baseCharacterState == typeof(CharacterState))
            {
                allies.AddRange(G.run.characters.FindAll(state => !state.dead));
                enemies.AddRange(G.run.enemies.FindAll(state => !state.dead));
            }
            else
            {
                allies.AddRange(G.run.enemies.FindAll(state => !state.dead));
                enemies.AddRange(G.run.characters.FindAll(state => !state.dead));
            }

            return (allies, enemies);
        }
        
        IEnumerator EnemyPickTargets()
        {
            G.hud.battlehud.DisableHud();
            var enemyDices = GetEnemyDices();

            yield return new WaitForSeconds(1.0f);
            
            var dices = GetEnemyDices();
            var (allies, enemies) = GetAllTargets(typeof(EnemyState));

            foreach (var dice in dices)
            {
                var face = dice.state.face;
                if (face.Is<TagAction>(out var action))
                {
                    if (action.action == ActionType.Blank)
                        continue;
                    var targets = GetTargetsForAction(dice.state, enemies, allies);

                    if (targets.Count > 0)
                    {
                        switch (action.area)
                        {
                            case TargetArea.Single:
                                targets = new List<ITarget>() { targets[UnityEngine.Random.Range(0, targets.Count)] };
                                break;

                            case TargetArea.Row:
                                if (action.row == TargetRow.Both)
                                {
                                    var rows = targets.Select(target => target.IsBackLine()).Distinct().Count();
                                    if (rows > 1)
                                    {
                                        var randomRow = UnityEngine.Random.Range(0, 2);
                                        targets = targets.FindAll(target => target.IsBackLine() != (randomRow == 0));
                                    }
                                }
                                break;
                        }    
                    }

                    if (targets.Count > 0)
                        dice.SetTargets(targets);
                }
            }

            //next phase
            SetTurnPhase(TurnPhase.PLAYER_TARGETING);
            
        }

        List<ITarget> GetTargetsForAction(DiceState diceState, List<ITarget> enemies, List<ITarget> allies)
        {
            var targets = new List<ITarget>();
            var face = diceState.face;
            if (face.Is<TagAction>(out var action) && action.action != ActionType.Blank)
            {
                switch (action.side)
                {
                    case TargetSide.Enemy:
                        targets.AddRange(enemies);
                        break;

                    case TargetSide.Ally:
                        targets.AddRange(allies);
                        break;

                    case TargetSide.Both:
                        targets.AddRange(enemies);
                        targets.AddRange(allies);
                        break;

                    case TargetSide.Self:
                        targets.Add(diceState.owner);
                        break;

                    case TargetSide.None:
                        break;
                }

                var frontline = targets.FindAll(target => !target.IsBackLine());
                var backline = targets.FindAll(target => target.IsBackLine());
                switch (action.row)
                {
                    case TargetRow.Front:
                        targets = frontline.Count == 0 ? backline : frontline;
                        break;

                    case TargetRow.Back:
                        targets = backline.Count == 0 ? frontline : backline;
                        break;
                }
            }
            return targets;
        }
        
        IEnumerator PlayerFreeRoll()
        {
            var dices = GetPlayerDices();

            yield return ThrowDices(dices);

            yield return new WaitForSeconds(0.2f);
            
            yield return RollDices(dices);
            
            yield return new WaitForSeconds(0.2f);
            
            SetTurnPhase(TurnPhase.RE_ROLL);
        }

        IEnumerator EnemyTurn()
        {
            var enemyDices = GetEnemyDices();

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
            G.hud.battlehud.DisableHud();
            
            foreach (var dice in GetPlayerDices())
            {
                if (!dice.state.owner.dead)
                {
                    if (dice.GetTargets().Count > 0)
                        dice.Punch();
                    if (dice.zone == rollDicesZone)
                        yield return ReturnDices(new List<DiceInteractiveObject>() { dice });
                    yield return DiceAction(dice);
                    
                }
                dice.ClearTargets();
            }
            foreach (var dice in GetEnemyDices())
            {
                if (!dice.state.owner.dead)
                {
                    if (dice.GetTargets().Count > 0)
                        dice.Punch();
                    if (dice.zone == rollDicesZone)
                        yield return ReturnDices(new List<DiceInteractiveObject>() { dice });
                    yield return DiceAction(dice);
                }
                dice.ClearTargets();
            }

            yield return ReturnAllDices();

            yield return new WaitForSeconds(0.5f);
            
            SetTurnPhase(TurnPhase.CHECK_WIN);

        }
        IEnumerator DiceAction(DiceInteractiveObject dice)
        {
            if (dice.state.face.Is<TagAction>(out var action))
            {
                switch (action.action)
                {
                    case ActionType.Attack:
                        foreach (var target in dice.GetTargets())
                        {
                            var damageable = target.GetView().GetComponent<Damageable>();
                            if (damageable)
                            {
                                yield return damageable.Hit(action.value);
                                if (damageable.state.dead)
                                {
                                    foreach (var diceState in damageable.state.diceStates)
                                    {
                                        diceState.interactiveObject.ClearTargets();
                                    }
                                }
                            }
                        }
                        break;

                    case ActionType.Heal:
                        foreach (var target in dice.GetTargets())
                        {
                            var damageable = target.GetView().GetComponent<Damageable>();
                            if (damageable)
                            {
                                yield return damageable.Heal(action.value);
                            }
                        }
                        break;

                    case ActionType.Def:
                        foreach (var target in dice.GetTargets())
                        {
                            var damageable = target.GetView().GetComponent<Damageable>();
                            if (damageable)
                            {
                                yield return damageable.Armor(action.value);
                            }
                        }
                        break;
                }
            }

            yield return new WaitForSeconds(0.25f);
        }

        IEnumerator StartPlayDicesPhase()
        {
            G.hud.battlehud.EndTurnButtonText.text = "Execute";
            G.hud.battlehud.DisableHud();

            yield return ReturnAllDices();

            G.hud.battlehud.EnableHud();
        }

        List<DiceInteractiveObject> GetPlayerDices()
        {
            return G.run.characters.FindAll(state => !state.dead).SelectMany(
                cs => cs.diceStates.Select(ds => ds.interactiveObject)
            ).Reverse().ToList();
        }
        
        List<DiceInteractiveObject> GetEnemyDices()
        {
            return G.run.enemies.FindAll(state => !state.dead).SelectMany(
                cs => cs.diceStates.Select(ds => ds.interactiveObject)).Reverse().ToList();
        }
        
        List<DiceInteractiveObject> GetDicesOnRollZOne()
        {
            return G.battle.rollDicesZone.objects.Select(o => o).ToList();
        }
        
        public IEnumerator ReturnAllDices()
        {
            yield return ReturnDices(GetDicesOnRollZOne());
        }

        IEnumerator ReRollWithCheck()
        {
            reRolls--;
            G.hud.battlehud.RollButtonText.text = "Reroll x " + reRolls;
            if (reRolls <= 0)
                G.hud.battlehud.DisableRerollButton();
            
            yield return RollDices(GetDicesOnRollZOne());

            yield return new WaitForEndOfFrame();
            
            if (reRolls <= 0)
            {
                yield return ReturnAllDices();
                
                SetTurnPhase(TurnPhase.ENEMY_TARGETING);                            
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
        
        void OnDiceClickedInRollzone(DiceInteractiveObject dice)
        {
            if (dice.state.owner.dead)
                return;
            
            switch (turnPhase)
            {
                case TurnPhase.RE_ROLL:
                    StartCoroutine(ReturnDices(new List<DiceInteractiveObject>() { dice }));
                    break;

                case TurnPhase.PLAYER_TARGETING:
                    dice.ClearTargets();
                    StartCoroutine(ReturnDices(new List<DiceInteractiveObject>() { dice }));
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
            if (state.dead) return;
            
            if (selectionMode)
            {
                selectionMode = false;
                selected = new List<ITarget>() { state };
                return;
            }
        }
        public void CharacterClicked(CharacterState state)
        {
            if (state.dead) return;
            
            if (selectionMode)
            {
                selectionMode = false;
                selected = new List<ITarget>() { state };
                return;
            }
        }
        public void OnDiceClickInCharacterView(CharacterView view, DiceInteractiveObject dice)
        {
            if (dice.state.owner.dead)
                return;
            
            switch (turnPhase)
            {
                case TurnPhase.RE_ROLL:
                    StartCoroutine(ThrowDices(new List<DiceInteractiveObject>() { dice })); 
                    break;

                case TurnPhase.PLAYER_TARGETING:
                    StartCoroutine(SelectTargetForDice(dice));
                    break;
            }
        }

        DiceInteractiveObject selectingTargetForDice;
        bool selectionMode;
        List<ITarget> selected;
        IEnumerator SelectTargetForDice(DiceInteractiveObject dice)
        {
            if (selectingTargetForDice != null || dice.state.face.Get<TagAction>().action == ActionType.Blank)
                yield break;
            
            G.hud.battlehud.DisableHud();
            
            selectingTargetForDice = dice;
            
            dice.SetScaleOverride(1.25f);

            var (allies, enemies) = GetAllTargets(typeof(CharacterState));
            var targets = GetTargetsForAction(dice.state, enemies, allies);

            if (targets.Count == 0)
            {
                selectingTargetForDice = null;
                yield break;
            }
            
            foreach (var target in targets)
            {
                target.GetView().GetComponent<Selectable>().EnableSelection(true);
            }
            selectionMode = true;

            yield return new WaitUntil(() => !selectionMode);
            
            foreach (var target in targets)
            {
                target.GetView().GetComponent<Selectable>().EnableSelection(false);
            }
            
            dice.SetTargets(selected);
            
            dice.SetScaleOverride(1.0f);

            yield return ThrowDices(new List<DiceInteractiveObject>() {dice});
            
            G.hud.battlehud.EnableHud();
            
            selectingTargetForDice = null;
        }
        public void OnDiceClickInEnemyView(EnemyView enemyView, DiceInteractiveObject dice)
        {
            if (dice.state.owner.dead)
                return;
        }
        
        public void EndTurnButton()
        {
            switch (turnPhase)
            {
                case TurnPhase.RE_ROLL:
                    SetTurnPhase(TurnPhase.ENEMY_TARGETING);
                    break;

                case TurnPhase.PLAYER_TARGETING:
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
                        StartCoroutine(ReRollWithCheck());
                    break;
                }
            }
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