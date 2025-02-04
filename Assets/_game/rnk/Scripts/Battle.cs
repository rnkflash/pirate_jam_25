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
        [NonSerialized] public Interactor interactor;
        [NonSerialized] public int reRolls;
        [NonSerialized] public TurnPhase turnPhase;
        
        bool isEnabled;
        
        void Awake()
        {
            G.battle = this;
            G.IsInBattle = false;

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

        void NextTurnPhase()
        {
            SetTurnPhase(turnPhase.Next());
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
            
            G.audio.Play<SFX_DiceDraw>();
            
            G.hud.ShowBattleHud();
            G.hud.battle.InitBattle();

            yield return new WaitForSeconds(0.5f);
            SetTurnPhase(TurnPhase.START_TURN);
        }

        public void FinishBattle()
        {
            G.hud.tooltip.Hide();
            G.run.enemies.Clear();
            RemoveBuffsAll();
            G.run.buffs.Clear();

            isEnabled = false;
            G.hud.battle.FinishBattle();
            G.hud.HideBattleHud();
            
            G.IsInBattle = false;
            G.crawler.OnFinishEncounter();
            
            //TODO do other shit rewards and shit
        }

        void ResetRerolls()
        {
            reRolls = 2;
            G.hud.battle.RollButtonText.text = "Reroll x " + reRolls;
            G.hud.battle.EndTurnButtonText.text = "Next phase";
        }

        void SetTurnPhase(TurnPhase newTurnPhase)
        {
            turnPhase = newTurnPhase;

            switch (turnPhase)
            {
                case TurnPhase.START_TURN:
                    ResetRerolls();
                    G.hud.battle.DisableHud();
                    NextTurnPhase();
                    break;
                
                case TurnPhase.EXECUTE_PLAYER_BUFFS:
                    StartCoroutine(StartExecuteBuffs(G.run.characters));
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

                case TurnPhase.EXECUTE_PLAYER_DICES:
                    StartCoroutine(StartExecutePhase(G.run.characters, true));
                    break;
                
                case TurnPhase.EXECUTE_ENEMY_BUFFS:
                    StartCoroutine(StartExecuteBuffs(G.run.enemies));
                    break;
                
                case TurnPhase.EXECUTE_ENEMY_DICES:
                    StartCoroutine(StartExecutePhase(G.run.enemies));
                    break;
                
                case TurnPhase.END_TURN:
                    SetTurnPhase(TurnPhase.START_TURN);
                    break;
            }
        }

        IEnumerator CheckWin()
        {
            var enemiesDead = G.run.enemies.FindAll(state => !state.dead).Count == 0;
            var allDead = G.run.characters.All(state => state.dead);

            if (allDead)
                yield return LoseSequence();
            else
            {
                if (!enemiesDead)
                {
                    NextTurnPhase();
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

            G.hud.battle.win.SetActive(true);

            yield return new WaitForSeconds(1.22f);

            G.hud.battle.win.SetActive(false);
            
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
            
            yield return new WaitForSeconds(0.25f);
            
            var dices = GetDices(G.run.enemies);
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
            
            yield return new WaitForSeconds(0.25f);

            //next phase
            NextTurnPhase();
        }

        List<ITarget> GetTargetsForAction(DiceState diceState, List<ITarget> enemies, List<ITarget> allies)
        {
            var targets = new List<ITarget>();
            var artefact = diceState.artefactOnFace();
            var face = artefact?.face ?? diceState.face;
            
            var interactors = interactor.FindAll<IModifyTargetList>();
            var modifiedEnemyList = new List<ITarget>();
            foreach (var interactor in interactors)
            {
                modifiedEnemyList = interactor.ModifyTargetList(enemies);
            }
            
            if (face.Is<TagActionTargeting>(out var targeting))
            {
                switch (targeting.side)
                {
                    case TargetSide.Enemy:
                        targets.AddRange(modifiedEnemyList);
                        break;

                    case TargetSide.Ally:
                        targets.AddRange(allies);
                        break;

                    case TargetSide.Both:
                        targets.AddRange(modifiedEnemyList);
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
            var dices = GetDices(G.run.characters);

            yield return ThrowDices(dices);

            yield return new WaitForSeconds(0.2f);
            
            yield return RollDices(dices);
            
            yield return new WaitForSeconds(0.2f);
            
            NextTurnPhase();
        }

        IEnumerator EnemyTurn()
        {
            var enemyDices = GetDices(G.run.enemies);

            yield return ThrowDices(enemyDices);

            yield return new WaitForSeconds(0.2f);
            
            yield return RollDices(enemyDices);
            
            yield return new WaitForSeconds(0.2f);
            
            yield return ReturnDices(enemyDices);
            
            //each enemy dice plays with random target

            //next phase
            NextTurnPhase();
        }
        
        IEnumerator StartExecutePhase<T>(List<T> characters, bool onRollDiceZone = false) where T : BaseCharacterState
        {
            G.hud.battle.DisableHud();
            var dices = GetDices(characters, onRollDiceZone);
            foreach (var dice in dices)
            {
                if (!dice.state.owner.dead)
                {
                    if (!onRollDiceZone || dice.zone == G.hud.battle.rollDicesZone)
                    {
                        dice.Punch();
                        if (dice.zone == G.hud.battle.rollDicesZone)
                            yield return ReturnDices(new List<DiceInteractiveObject>() { dice });
                        
                        yield return DiceAction(dice);
                    }
                }
                dice.ClearTargets();
            }

            yield return ReturnAllDices();

            yield return new WaitForSeconds(0.5f);
            
            yield return CheckWin();

        }

        int[] ApplyDamageModifiers(int[] values, BaseCharacterState owner)
        {
            var modifiedValues = new int[values.Length];
            values.CopyTo(modifiedValues , 0);
            var buffsOnOwner = GetBuffsOnCharacter(owner);
            foreach (var buff in buffsOnOwner)
            {
                var dmgMods = interactor.FindAll<IDamageModifier>();
                foreach (var f in dmgMods)
                    for (int i = 0; i < values.Length; i++)
                        modifiedValues[i] = f.ModifyDamage(buff.model, modifiedValues[i]);
            }
            return modifiedValues;
        }
        
        IEnumerator DiceAction(DiceInteractiveObject dice)
        {
            var owner = dice.state.owner;
            var face = dice.state.overridenFace;
            var targets = dice.GetTargets();
            var values = face.Get<TagValue>().values;

            var modifiedValues = ApplyDamageModifiers(values, owner);
            
            var diceActions = interactor.FindAll<IDiceFaceAction>();
            foreach (var f in diceActions)
                yield return f.OnAction(targets, face, owner, modifiedValues);
            
            foreach (var target in targets)
            {
                if (target.GetState().dead)
                    RemoveBuffs(target.GetState());
                
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

        List<BuffState> GetBuffsOnCharacter<T>(T character) where T : BaseCharacterState
        {
            return G.run.buffs.FindAll(state => state.target.GetState() == character);
        }
        
        List<BuffState> GetBuffsOnCharacters<T>(List<T> characters) where T : BaseCharacterState
        {
            return G.run.buffs.FindAll(state => characters.Contains(state.target.GetState()));
        }
        
        IEnumerator StartExecuteBuffs<T>(List<T> baseCharacterStates) where T : BaseCharacterState
        {
            var interactors = interactor.FindAll<IBuffAction>();
            var expiredBuffs = new List<BuffState>();
            var buffs = GetBuffsOnCharacters(baseCharacterStates);
            foreach (var buffState in buffs)
            {
                foreach (var f in interactors)
                {
                    yield return f.OnBuffAction(buffState);
                }
                buffState.turnsLeft -= 1;
                
                buffState.view.UpdateState();
                buffState.view.Punch();

                if (buffState.turnsLeft <= 0)
                    expiredBuffs.Add(buffState);
            }

            foreach (var expiredBuff in expiredBuffs)
            {
                expiredBuff.view.Remove();
                G.run.buffs.Remove(expiredBuff);
            }

            yield return new WaitForSeconds(0.25f);
            
            yield return ResetArmor(baseCharacterStates);

            yield return CheckWin();
        }

        IEnumerator StartPlayDicesPhase()
        {
            G.hud.battle.EndTurnButtonText.text = "Execute";
            G.hud.battle.DisableHud();

            yield return ReturnAllDices();

            G.hud.battle.EnableHud();
        }
        
        List<DiceInteractiveObject> GetDices<T>(List<T> characters, bool orderedByRollZone = false) where T : BaseCharacterState
        {
            var unordered = characters.FindAll(state => !state.dead).SelectMany(
                cs => cs.diceStates.Select(ds => ds.interactiveObject)
            ).ToList();

            if (orderedByRollZone)
            {
                var originalOrdered = G.hud.battle.rollDicesZone.objects;
                var orderedDices = unordered.OrderBy(o => originalOrdered.IndexOf(o)).ToList();
                orderedDices.Reverse();
                return orderedDices;
            }
            
            unordered.Reverse();
            return unordered;
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
                
                NextTurnPhase();                            
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
            G.audio.Play<SFX_Woosh>();
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
            G.audio.Play<SFX_Woosh>();
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
            if (selectingTargetForDice != null || dice.state.overridenFace.Get<TagActionBlank>() != null)
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
                    NextTurnPhase();
                    break;

                case TurnPhase.PLAYER_TARGETING:
                    NextTurnPhase();
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
        public void AddBuff(ITarget target, CMSEntity buff, BaseCharacterState owner)
        {
            var buffState = new BuffState()
            {
                model = buff,
                turnsLeft = buff.Get<TagDuration>()?.turns ?? 99,
                target = target,
                castedBy = owner
            };
            
            G.run.buffs.Add(buffState);

            target.GetView().GetBuffList().AddBuff(buffState);
        }
        
        public List<BuffState> GetBuffs(BaseCharacterState character)
        {
            return G.run.buffs.FindAll(state => state.target.GetState() == character);
        }
        
        void RemoveBuffs(BaseCharacterState character)
        {
            var removedBuffs = G.run.buffs.FindAll(state => state.target.GetState() == character);
            foreach (var buff in removedBuffs)
            {
                RemoveBuff(buff);
            }
        }

        void RemoveBuffsAll()
        {
            var removedBuffs = new List<BuffState>(G.run.buffs);
            foreach (var buff in removedBuffs)
            {
                RemoveBuff(buff);
            }
        }
        
        void RemoveBuff(BuffState buff)
        {
            buff.view.Remove();
            G.run.buffs.Remove(buff);
        }

        public IEnumerator Damage(ITarget target, BaseCharacterState owner, int damage)
        {
            var damageable = target.GetView().GetComponent<Damageable>();
            if (damageable && !damageable.state.dead)
            {
                var modifiedValue = damage;
                foreach (var buff in GetBuffsOnCharacter(target.GetState()))
                {
                    var dmgMods = interactor.FindAll<IReceiveDamageModifier>();
                    foreach (var f in dmgMods)
                        modifiedValue = f.ModifyDamage(buff.model, modifiedValue);

                }
                
                yield return damageable.Hit(modifiedValue);
            }
        }
        
        IEnumerator ResetArmor<T>(List<T> characters) where T : BaseCharacterState
        {
            foreach (var c in characters)
            {
                if (c.armor != 0)
                {
                    c.GetView().GetComponent<Damageable>().PunchText();
                    c.armor = 0;
                    c.GetView().GetComponent<Damageable>().UpdateView();
                    
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
        public List<ITarget> GetEnemies(BaseCharacterState owner)
        {
            return GetAllTargets(owner.GetType()).enemies;
        }
        
        public List<ITarget> GetAllies(BaseCharacterState owner)
        {
            return GetAllTargets(owner.GetType()).allies;
        }
        public void RetargetDices(List<DiceState> enemyDices, ITarget target)
        {
            foreach (var dice in enemyDices)
            {
                if (dice.interactiveObject.GetTargets().Count > 0)
                    dice.interactiveObject.SetTargets(new List<ITarget>() {target} );
            }
        }
    }

}