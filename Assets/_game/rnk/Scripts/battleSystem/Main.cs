using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.interactor;
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
        
        void OnClickDice(DiceInteractiveObject dice)
        {
            ReturnDice(dice);
        }

        IEnumerator ReRollDices()
        {
            var interactors = interactor.FindAll<IOnReroll>();
            foreach (var onReroll in interactors)
            {
                yield return onReroll.OnReroll();
            }
        }

        public IEnumerator RollAllDices()
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

        public void ReturnDice(DiceInteractiveObject dice)
        {
            var zone = dice.state.owner.diceZone;
            dice.transform.SetParent(zone.transform);
            zone.Claim(dice);
        }

        public void ReturnDiceToRollzone(DiceInteractiveObject dice)
        {
            dice.transform.SetParent(rollDicesZone.transform);
            rollDicesZone.Claim(dice);
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