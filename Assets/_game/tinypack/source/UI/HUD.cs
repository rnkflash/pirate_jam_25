using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI DiceView;
    public Button EndTurn;
    public UITooltip tooltip;

    public Slider Health;
    public TMP_Text HealthValue;
    
    public TMP_Text DeckValue;
    [NonSerialized] public int deckCount;
    
    public TMP_Text DiscardValue;
    [NonSerialized] public int discardCount;

    bool trackersStarted;

    void Awake()
    {
        G.hud = this;
        G.hud.tooltip.Hide();
    }

    void Start()
    {
        EndTurn.onClick.AddListener(OnClickEndTurn);

        trackersStarted = true;
        StartTrackers();
    }

    void StartTrackers()
    {
        StartCoroutine(TrackHealth());
        StartCoroutine(TrackDeck());
        StartCoroutine(TrackDiscard());
    }

    void StopTrackers()
    {
        StopAllCoroutines();
    }

    void OnDisable()
    {
        if (trackersStarted)
        {
            StopTrackers();
        }
    }

    void OnEnable()
    {
        if (trackersStarted)
        {
            StartTrackers();    
        }
    }

    public void FixTrackers()
    {
        var prefix = "Deck (";
        var postfix = ")";
        DeckValue.text = prefix + G.main.deck.Count + postfix;
        
        prefix = "Discard (";
        postfix = ")";
        DiscardValue.text = prefix + G.main.discard.Count + postfix;
    }

    IEnumerator TrackDeck()
    {
        var prefix = "Deck (";
        var postfix = ")";
        deckCount = G.main.deck.Count;
        yield return UpdateTMPText(DeckValue, prefix + deckCount + postfix);
        while (true)
        {
            if (deckCount > G.main.deck.Count)
            {
                deckCount--;
                yield return UpdateTMPText(DeckValue, prefix + deckCount + postfix);
            }

            if (deckCount < G.main.deck.Count)
            {
                deckCount++;
                yield return UpdateTMPText(DeckValue, prefix + deckCount + postfix);
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    IEnumerator TrackDiscard()
    {
        var prefix = "Discard (";
        var postfix = ")";
        discardCount = G.main.discard.Count;
        yield return UpdateTMPText(DiscardValue, prefix + discardCount + postfix);
        
        while (true)
        {
            if (discardCount > G.main.discard.Count)
            {
                discardCount--;
                yield return UpdateTMPText(DiscardValue, prefix + discardCount + postfix);
            }

            if (discardCount < G.main.discard.Count)
            {
                discardCount++;
                yield return UpdateTMPText(DiscardValue, prefix + discardCount + postfix);
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    IEnumerator TrackHealth()
    {
        Health.value = G.run.maxHealth / 2;
        yield return UpdateTMPText(HealthValue, Health.value + "/" + G.run.maxHealth);
        
        while (true)
        {
            if (Health.value > G.run.health)
            {
                Health.value--;
                yield return UpdateTMPText(HealthValue, Health.value + "/" + G.run.maxHealth);
            }

            if (Health.value < G.run.health)
            {
                Health.value++;
                yield return UpdateTMPText(HealthValue, Health.value + "/" + G.run.maxHealth);
            }
            
            Health.maxValue = G.run.maxHealth;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator UpdateTMPText(TMP_Text tmpText, string text)
    {
        yield return G.ui.ScaleCountIn(tmpText.transform);
        tmpText.text = text;
        yield return G.ui.ScaleCountOut(tmpText.transform);
    }

    void OnClickEndTurn()
    {
        G.main.EndTurn();
    }

    public void DisableHud()
    {
        G.main.handZone.canDrag = false;
        EndTurn.interactable = false;
    }

    public void EnableHud()
    {
        G.main.handZone.canDrag = true;
        EndTurn.interactable = true;
    }

    void Update()
    {
        
    }

    public static Vector2 MousePositionToCanvasPosition(Canvas canvas, RectTransform rectTransform)
    {
        Vector2 localPoint;
        Vector2 screenPosition = Input.mousePosition;
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            screenPosition,
            uiCamera,
            out localPoint
        );

        return localPoint;
    }

    public Vector2 MousePos()
    {
        return MousePositionToCanvasPosition(G.hud.GetComponent<Canvas>(), G.hud.GetComponent<RectTransform>());
    }

    public void PunchEndTurn()
    {
        G.ui.Punch(EndTurn.transform);
    }
}