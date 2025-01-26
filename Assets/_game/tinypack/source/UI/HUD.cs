using System;
using System.Collections;
using _game;
using _game.rnk.Scripts.battleSystem;
using _game.tinypack.source.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Selectable = UnityEngine.UI.Selectable;

public class HUD : MonoBehaviour
{
    public TMP_Text RollButtonText;
    public TMP_Text EndTurnButtonText;
    
    public Button RollButton;
    public Button EndTurnButton;
    public UITooltip tooltip;

    public SpriteVFX meleeHit;
    public SpriteVFX armorHit;
    public SpriteVFX healHit;

    public TimurHUD timurHUD;
    
    void Awake()
    {
        G.hud = this;
        G.hud.tooltip.Hide();
    }

    void Start()
    {
        EndTurnButton.onClick.AddListener(OnClickEndTurn);
        RollButton.onClick.AddListener(OnClickRollButton);
    }

    void OnDestroy()
    {
        EndTurnButton.onClick.RemoveAllListeners();
        RollButton.onClick.RemoveAllListeners();
    }

    IEnumerator UpdateTMPText(TMP_Text tmpText, string text)
    {
        yield return G.ui.ScaleCountIn(tmpText.transform);
        tmpText.text = text;
        yield return G.ui.ScaleCountOut(tmpText.transform);
    }

    void OnClickEndTurn()
    {
        PunchEndTurn();
        G.main.EndTurnButton();
    }
    
    void OnClickRollButton()
    {
        PunchRollButton();
        G.main.ReRollButton();
    }

    public void DisableHud()
    {
        G.main.rollDicesZone.canDrag = false;
        EndTurnButton.interactable = false;
        RollButton.interactable = false;
    }

    public void EnableHud()
    {
        G.main.rollDicesZone.canDrag = true;
        EndTurnButton.interactable = true;
        RollButton.interactable = true;
    }
    
    public void Disable(Selectable uiElement)
    {
        uiElement.interactable = false;
    }

    public void Enable(Selectable uiElement)
    {
        uiElement.interactable = true;
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
        G.ui.Punch(EndTurnButton.transform);
    }
    
    public void PunchRollButton()
    {
        G.ui.Punch(RollButton.transform);
    }

    public void EnableInfoWindow(CharacterState state)
    {
        timurHUD.SetState(state);
    }
}