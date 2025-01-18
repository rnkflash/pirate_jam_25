using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public CardZone handZone;
    public Transform deckPos;
    public Transform discardPos;
    public RectTransform cardUsageRect;

    public List<GameObject> partsOfHudToDisable;

    public Interactor interactor;

    public List<CardState> deck = new List<CardState>();
    public List<CardState> hand = new List<CardState>();
    public List<CardState> discard = new List<CardState>();
    
    public CMSEntity levelEntity;
    List<string> levelSeq = new List<string>()
    {
        E.Id<Level0>(),
    };

    void Awake()
    {
        interactor = new Interactor();
        interactor.Init();

        if (G.run == null)
        {
            G.run = new RunState();

            G.run.maxHealth = 15;
            G.run.health = G.run.maxHealth;

            //G.run.deck.Add(new CardId(E.Id<ArcheryRangeCard>()));
        }

        G.main = this;
    }

    IEnumerator Start()
    {
        CMS.Init();

        G.hud.DisableHud();
        G.ui.DisableInput();

        G.fader.FadeOut();
        
        yield return G.ui.Unsay();

        /*
        if (G.run.level < levelSeq.Count)
            yield return LoadLevel(CMS.Get<CMSEntity>(levelSeq[G.run.level]));
        else
            SceneManager.LoadScene("_game/tinypack/end_screen");
        */

        deck.Clear();
        hand.Clear();
        discard.Clear();

        foreach(var cardId in G.run.deck)
        {
            AddCardToDeck(cardId);
        }
        
        deck.Shuffle();
        
        yield return DrawHand();

        G.ui.EnableInput();
        G.hud.FixTrackers();
        G.hud.EnableHud();

    }
    public void EndTurn()
    {
        StartCoroutine(EndTurnCoroutine());
    }

    IEnumerator EndTurnCoroutine()
    {
        G.hud.DisableHud();

        /*var obstacles = interactor.FindAll<IOnEndTurnObstacle>();

        foreach (var ob in obstacles)
        {
            foreach (var challengeActive in challengesActive)
            {
                if (!challengeActive.IsComplete())
                {
                    yield return ob.OnEndTurn(challengeActive);
                }
            }
        }*/

        /*foreach (var f in field.objects)
        {
            var endTurn = G.main.interactor.FindAll<IOnEndTurnFieldDice>();
            foreach (var et in endTurn)
                yield return et.OnEndTurnInField(f.state);
        }*/

        yield return DiscardHand();

        yield return DrawHand();

        G.camera.UIHit();

        G.hud.EnableHud();
    }


    public void StartDrag(DraggableSmoothDamp draggableSmoothDamp)
    {
        G.drag_card = draggableSmoothDamp.GetComponent<InteractiveObject>();
        G.audio.Play<SFX_Animal>();
    }

    public void StopDrag()
    {
        TryPlayCard(G.drag_card);
        G.drag_card = null;
    }

    IEnumerator DrawHand()
    {
        G.audio.Play<SFX_DiceDraw>();
        
        for (var i = 0; i < G.run.drawSize; i++)
        {
            if (deck.Count == 0)
            {
                yield return ShuffleDiscard();
            }
            
            if (deck.Count > 0)
            {
                var cardState = deck.Pop();
                hand.Add(cardState);
                var obj = CreateCardObject(cardState, deckPos.position);
                handZone.Claim(obj);
                obj.scaleRoot.localScale = Vector3.zero;
                obj.scaleRoot.DOScale(1.0f, 0.2f);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator ShuffleDiscard()
    {
        var shuffleAmount = discard.Count;
        for (int i = 0; i < shuffleAmount; i++)
        {
            StartCoroutine(ShuffleDiscardCard());
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitUntil(() => discard.Count == 0);
        yield return new WaitForSeconds(0.2f);
        
        deck.Shuffle();
    }

    IEnumerator ShuffleDiscardCard()
    {
        var cardState = discard.Pop();
        var obj = CreateCardObject(cardState, discardPos.position);
        obj.scaleRoot.localScale = Vector3.zero;

        yield return new WaitForEndOfFrame();
            
        obj.moveable.targetPosition = deckPos.position;
            
        yield return DOTween.Sequence().Append(
            obj.scaleRoot.DOScale(1.0f, 0.25f)
        ).Append(
            obj.scaleRoot.DOScale(0.0f, 0.25f)
        ).WaitForCompletion();

        Destroy(obj.gameObject);
        cardState.view = null;
        deck.Add(cardState);
    }

    IEnumerator DiscardHand()
    {
        G.audio.Play<SFX_Kill>();
        foreach (var cardState in hand)
        {
            yield return DiscardCard(cardState, false);
        }
        hand.Clear();
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator DiscardCard(CardState cardState, bool removeFromHand = true)
    {
        cardState.view.Leave();
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
        }
    }

    public IEnumerator LoadLevel(CMSEntity entity)
    {
        levelEntity = entity;

        if (levelEntity.Is<TagExecuteScript>(out var exs))
        {
            yield return exs.toExecute();
        }
    }

    public void TryPlayCard(InteractiveObject card)
    {
        var mousePosition = Input.mousePosition;
        Vector2 localMousePosition = cardUsageRect.InverseTransformPoint(mousePosition);
        if (cardUsageRect.rect.Contains(localMousePosition))
        {
            StartCoroutine(PlayCard(card));  
        }
    }

    IEnumerator PlayCard(InteractiveObject card)
    {
        G.audio.Play<SFX_Animal>();

        yield return new WaitForEndOfFrame();
        
        yield return DiscardCard(card.state);

        if (hand.Count == 0)
        {
            yield return new WaitForSeconds(0.25f);
            G.hud.PunchEndTurn();
        }
    }

    public void AddCardToDeck(CardId cardId)
    {
        var cardState = CreateCardState(cardId.id);
        deck.Add(cardState);
    }

    public InteractiveObject CreateCardObject(CardState cardState, Vector3 startPos)
    {
        var instance = Instantiate(cardState.model.Get<TagPrefab>().prefab, handZone.transform);
        instance.SetState(cardState);
        instance.moveable.targetPosition = instance.transform.position = startPos;
        return instance;
    }

    public CardState CreateCardState(string t)
    {
        var card = CMS.Get<CMSEntity>(t);
        var state = new CardState();
        state.model = card;
        return state;
    }

    bool isWin;
    bool skip;

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
            G.run.level = 0;
            SceneManager.LoadScene(GameSettings.MAIN_SCENE);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(WinSequence());
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            G.run.level++;
            SceneManager.LoadScene(GameSettings.MAIN_SCENE);
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            G.run = null;
            SceneManager.LoadScene(0);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(DrawHand());
            G.feel.UIPunchSoft();
        }
#endif
    }

    public IEnumerator CheckForWin()
    {
        yield break;
        
        //yield return WinSequence();
    }

    IEnumerator WinSequence()
    {
        if (isWin)
        {
            Debug.Log("<color=red>double trigger win sequence lol</color>");
            yield break;
        }

        G.hud.DisableHud();

        isWin = true;

        G.ui.win.SetActive(true);
        G.audio.Play<SFX_Win>();

        yield return new WaitForSeconds(1.22f);

        G.ui.win.SetActive(false);

        // yield return storage.Clear();
        /*yield return field.Clear();
        yield return hand.Clear();*/

        G.run.level++;

        //if (!IsFinal()) yield return ShowPicker();

        G.fader.FadeIn();

        yield return new WaitForSeconds(1f);

        if (!IsFinal())
            SceneManager.LoadScene(GameSettings.MAIN_SCENE);
        else
        {
            G.audio.Play<SFX_Magic>();
            SceneManager.LoadScene("_game/tinypack/end_screen");
        }
    }

    bool IsFinal()
    {
        return G.run.level >= levelSeq.Count || levelEntity.Is<TagIsFinal>();
    }

    public class IntOutput
    {
        public int dmg;
    }

    IEnumerator Loss()
    {
        G.ui.defeat.SetActive(true);
        yield return new WaitForSeconds(1f);
        G.run = null;
        SceneManager.LoadScene(GameSettings.MAIN_SCENE);
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

interface IOnEndTurnFieldDice
{
    public IEnumerator OnEndTurnInField(CardState state);
}
