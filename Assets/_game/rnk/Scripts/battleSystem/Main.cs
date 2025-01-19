using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.dice;
using DG.Tweening;
using Unity.VisualScripting;
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
    
        public CMSEntity level = new TestBattleLevel();

        CharacterView characterViewPrefab;
        
        bool isWin;
        bool skip;
        
        void Awake()
        {
            characterViewPrefab = "prefab/CharacterView".Load<CharacterView>();
            
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
            
            rollDicesZone.OnClickDice += OnClickDice;

            G.main = this;
        }

        void OnDestroy()
        {
            rollDicesZone.OnClickDice -= OnClickDice;
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

            StartCoroutine(RollAllDices());

        }
        public void EndTurn()
        {
            StartCoroutine(EndTurnCoroutine());
        }
        
        public void ReRoll()
        {
            StartCoroutine(ReRollDices());
        }

        IEnumerator EndTurnCoroutine()
        {
            G.hud.DisableHud();

            yield return ReturnAllDices();

            yield return ResetRerolls();

            G.camera.UIHit();

            G.hud.EnableHud();

            yield return new WaitForSeconds(0.25f);
            
            yield return RollAllDices();
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
        
        void OnClickDice(DiceInteractiveObject dice)
        {
            ReturnDice(dice);
        }

        IEnumerator ReRollDices()
        {
            foreach (var dice in rollDicesZone.objects)
            {
                dice.Roll();
            }
            
            yield return new WaitForSeconds(0.2f);
        }

        IEnumerator RollAllDices()
        {
            yield return new WaitForEndOfFrame();
            
            G.audio.Play<SFX_DiceDraw>();

            var dices = G.run.characters.SelectMany(
                cs => cs.diceStates.Select(ds => ds.interactiveObject)
            ).Reverse().ToList();
            
            foreach (var dice in dices)
            {
                dice.transform.SetParent(rollDicesZone.transform);
                rollDicesZone.Claim(dice);
                dice.Punch();
            }
            
            yield return new WaitForSeconds(0.2f);
            
            foreach (var dice in dices)
            {
                dice.Roll();    
            }
        }

        IEnumerator ReturnAllDices()
        {
            var copy = rollDicesZone.objects.Select(o => o).ToList();
            foreach (var dice in copy)
            {
                ReturnDice(dice);
            }
            
            yield return new WaitForSeconds(0.2f);
        }

        void ReturnDice(DiceInteractiveObject dice)
        {
            var zone = dice.state.owner.view.diceZone;
            dice.transform.SetParent(zone.transform);
            zone.Claim(dice);
        }

        public void ReturnDiceToRollzone(DiceInteractiveObject dice)
        {
            dice.transform.SetParent(rollDicesZone.transform);
            rollDicesZone.Claim(dice);
        }
        
        IEnumerator ResetRerolls()
        {
            /*var shuffleAmount = discard.Count;
            for (int i = 0; i < shuffleAmount; i++)
            {
                StartCoroutine(ShuffleDiscardCard());
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitUntil(() => discard.Count == 0);*/
            yield return new WaitForSeconds(0.2f);
        }

        IEnumerator ReturnDice(DiceState diceState)
        {
            /*cardState.view.Leave();
            cardState.view.moveable.targetPosition = discardPos.position;
            cardState.view.scaleRoot.localScale = Vector3.one;
            cardState.view.scaleRoot.DOScale(0.0f, 0.2f);
            
            yield return new WaitForSeconds(0.2f);

            cardState.view.scaleRoot.DOKill();
            Destroy(cardState.view.gameObject);
            cardState.view = null;
            discard.Add(cardState);

            if (removeFromHand)
            {
                hand.Remove(cardState);
            }*/
            yield return new WaitForSeconds(0.2f);
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
        }

        public CharacterView CreateCharacterView(CharacterState characterState)
        {
            var character = Instantiate(characterViewPrefab, charactersRoot);
            characterState.view = character;
            character.SetState(characterState);
            return character;
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