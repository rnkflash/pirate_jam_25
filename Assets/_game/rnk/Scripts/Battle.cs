using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.crawler;
using _game.rnk.Scripts.enums;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.tags.actions;
using _game.rnk.Scripts.tags.buffs;
using _game.rnk.Scripts.tags.interactor;
using _game.rnk.Scripts.util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _game.rnk.Scripts
{
    public class Battle : MonoBehaviour
    {
        public Interactor interactor;
        
        [NonSerialized] public int reRolls;
        [NonSerialized] public TurnPhase turnPhase;
        
        bool isWin;
        bool isEnabled;
        
        void Awake()
        {
            G.battle = this;

            interactor = new Interactor();
            interactor.Init();
            
        }
        
        void Update()
        {
            if (!isEnabled)
                return;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    StartCoroutine(WinSequence());
                }
            
                if (Input.GetKeyDown(KeyCode.L))
                {
                    StartCoroutine(LoseSequence());
                }    
            }
        }

        public void StartBattle(BattleEncounter encounter)
        {
            G.IsInBattle = true;
            StartCoroutine(StartingBattle(encounter));
        }
        
        IEnumerator StartingBattle(BattleEncounter encounter)
        {
            yield return G.ui.Say("Alas! We have been ambushed...");
            yield return G.ui.SmartWait(3f);
            yield return G.ui.Unsay();
            yield return G.ui.SmartWait(0.25f);
            yield return G.ui.Say("FIGHT!");
            yield return G.ui.SmartWait(3f);
            yield return G.ui.Unsay();
            
            isEnabled = true;
            G.run.battle = encounter;
            G.run.enemies.Clear();
            foreach (var enemy in encounter.enemies)
            {
                var model = enemy.body.GetEntity();
                var enemyState = new EnemyState()
                {
                    objInScene = enemy,
                    armor = 0,
                    health = model.Get<TagHealth>().health,
                    maxHealth = model.Get<TagHealth>().health,
                    bodyState = new BodyState() { model = model },
                };
                var dices = enemy.body.dices.Select(so =>
                    new DiceState()
                    {
                        owner = enemyState,
                        model = so.GetEntity()
                    }
                ).ToList();
                enemyState.diceStates = dices;
                G.run.enemies.Add(enemyState);
            }
            
            G.hud.ShowBattleHud();
            G.hud.battle.InitBattle();

            yield return new WaitForSeconds(0.5f);
            SetTurnPhase(TurnPhase.START_TURN);
        }

        public void FinishBattle()
        {
            G.run.enemies.Clear();
            
            isEnabled = false;
            G.hud.battle.FinishBattle();
            G.hud.HideBattleHud();
            
            G.IsInBattle = false;
            G.crawler.OnFinishEncounter();
            
            //TODO do other shit rewards and shit
        }

        void SetTurnPhase(TurnPhase newTurnPhase)
        {
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
                    G.hud.battle.RollButtonText.text = "Reroll x " + reRolls;
                    G.hud.battle.EndTurnButtonText.text = "Next phase";
                    G.hud.battle.DisableHud();
                    SetTurnPhase(TurnPhase.ENEMY_ROLL);
                    break;
                
                case TurnPhase.ENEMY_ROLL:
                    StartCoroutine(EnemyTurn());
                    break;

                case TurnPhase.FREE_ROLL:
                    StartCoroutine(PlayerFreeRoll());
                    break;

                case TurnPhase.RE_ROLL:
                    G.hud.battle.EnableHud();
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
                
                case TurnPhase.EXECUTE_BUFFS:
                    StartCoroutine(StartExecuteBuffs());
                    
                    break;

                case TurnPhase.CHECK_WIN:
                    StartCoroutine(CheckWin());
                    break;
            }
        }

        IEnumerator CheckWin()
        {
            isWin = G.run.enemies.FindAll(state => !state.dead).Count == 0;
            var allDead = G.run.characters.All(state => state.dead);

            if (allDead)
                yield return LoseSequence();
            else
            {
                if (!isWin)
                {
                    SetTurnPhase(TurnPhase.START_TURN);
                }
                else
                {
                    yield return WinSequence();
                }
            }
        }
        
        IEnumerator WinSequence()
        {
            G.audio.Play<SFX_Win>();
            
            G.hud.battle.DisableHud();
            G.hud.battle.HideEnemies();

            isWin = true;
            
            G.hud.battle.win.SetActive(true);

            yield return new WaitForSeconds(1.22f);

            G.hud.battle.win.SetActive(false);
                
            yield return new WaitForSeconds(1f);
            
            FinishBattle();
        }


        IEnumerator LoseSequence()
        {
            G.audio.Play<SFX_Lose>();
            G.camera.UIHit();
            G.hud.battle.defeat.SetActive(true);
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
            G.hud.battle.DisableHud();
            var enemyDices = GetEnemyDices();

            yield return new WaitForSeconds(1.0f);
            
            var dices = GetEnemyDices();
            var (allies, enemies) = GetAllTargets(typeof(EnemyState));

            foreach (var dice in dices)
            {
                var face = dice.state.face;
                if (face.Is<TagActionTargeting>(out var targeting))
                {
                    var targets = GetTargetsForAction(dice.state, enemies, allies);

                    if (targets.Count > 0)
                    {
                        switch (targeting.area)
                        {
                            case TargetArea.Single:
                                targets = new List<ITarget>() { targets[UnityEngine.Random.Range(0, targets.Count)] };
                                break;

                            case TargetArea.Row:
                                if (targeting.row == TargetRow.Both)
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
            var artefact = diceState.artefactOnFace();
            var face = artefact?.face ?? diceState.face;
            if (face.Is<TagActionTargeting>(out var targeting))
            {
                switch (targeting.side)
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
                switch (targeting.row)
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
            G.hud.battle.DisableHud();
            
            foreach (var dice in GetPlayerDices())
            {
                if (!dice.state.owner.dead)
                {
                    if (dice.GetTargets().Count > 0)
                        dice.Punch();
                    if (dice.zone == G.hud.battle.rollDicesZone)
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
                    if (dice.zone == G.hud.battle.rollDicesZone)
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
            var artefact = dice.state.artefactOnFace();
            var owner = dice.state.owner;
            var face = artefact?.face ?? dice.state.face;
            var targets = dice.GetTargets();
            var interactors = interactor.FindAll<IDiceFaceAction>();
            foreach (var f in interactors)
                yield return f.OnAction(targets, face, owner);
            
            foreach (var target in targets)
            {
                var damageable = target.GetView().GetComponent<Damageable>();
                if (damageable != null && damageable.state.dead)
                {
                    foreach (var diceState in damageable.state.diceStates)
                    {
                        diceState.interactiveObject.ClearTargets();
                    }
                }
            }

            yield return new WaitForSeconds(0.25f);
        }
        
        IEnumerator StartExecuteBuffs()
        {
            var interactors = interactor.FindAll<IBuffAction>();
            foreach (var buffState in G.run.buffs)
            {
                foreach (var f in interactors)
                    yield return f.OnBuffAction(buffState);
            }
        }

        IEnumerator StartPlayDicesPhase()
        {
            G.hud.battle.EndTurnButtonText.text = "Execute";
            G.hud.battle.DisableHud();

            yield return ReturnAllDices();

            G.hud.battle.EnableHud();
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
            return G.hud.battle.rollDicesZone.objects.Select(o => o).ToList();
        }
        
        public IEnumerator ReturnAllDices()
        {
            yield return ReturnDices(GetDicesOnRollZOne());
        }

        IEnumerator ReRollWithCheck()
        {
            reRolls--;
            G.hud.battle.RollButtonText.text = "Reroll x " + reRolls;
            if (reRolls <= 0)
                G.hud.battle.DisableRerollButton();
            
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
        
        public void OnDiceClickedInRollzone(DiceInteractiveObject dice)
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
                MoveDice(dice, G.hud.battle.rollDicesZone);
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

        public void EnemyClicked(EnemyState state)
        {
            if (state.dead) return;
            
            if (selectionMode)
            {
                selectionMode = false;
                var character = selectingTargetForDice.state.owner;
                var face = character.diceStates.First().overridenFace;
                switch (face.Get<TagActionTargeting>().area)
                {
                    case TargetArea.Single:
                        selected = new List<ITarget>() { state };
                        break;

                    case TargetArea.Row:
                        selected = new List<ITarget>();
                        selected.AddRange(G.run.enemies);
                        break;

                    case TargetArea.All:
                        selected = new List<ITarget>();
                        selected.AddRange(G.run.enemies);
                        break;
                }
            }
        }
        public void CharacterClicked(CharacterState state)
        {
            if (state.dead) return;
            
            if (selectionMode)
            {
                selectionMode = false;
                var character = selectingTargetForDice.state.owner;
                var face = character.diceStates.First().overridenFace;
                switch (face.Get<TagActionTargeting>().area)
                {

                    case TargetArea.Single:
                        selected = new List<ITarget>() { state };
                        break;

                    case TargetArea.Row:
                        selected = new List<ITarget>();
                        selected.AddRange(G.run.characters);
                        break;

                    case TargetArea.All:
                        selected = new List<ITarget>();
                        selected.AddRange(G.run.characters);
                        break;
                }
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
            if (selectingTargetForDice != null || dice.state.face.Get<TagActionBlank>() != null)
                yield break;
            
            G.hud.battle.DisableHud();
            
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
            
            G.hud.battle.EnableHud();
            
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
        public IEnumerator AddBuff(ITarget target, CMSEntity face, BaseCharacterState owner)
        {
            G.run.buffs.Add(new BuffState()
            {
                model = face,
                turnsLeft = face.Get<TagDuration>()?.turns ?? 99,
                target = target,
                castedBy = owner
            });
            
            //TODO update buffs views
            
            yield return new WaitForSeconds(0.25f);
        }
    }

}