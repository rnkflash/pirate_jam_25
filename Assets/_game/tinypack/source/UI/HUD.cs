using System;
using System.Collections;
using _game.rnk.Scripts.battleSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Button RollButton;
    public Button EndTurn;
    public UITooltip tooltip;

    void Awake()
    {
        G.hud = this;
        G.hud.tooltip.Hide();
    }

    void Start()
    {
        EndTurn.onClick.AddListener(OnClickEndTurn);
        RollButton.onClick.AddListener(OnClickRollButton);
    }

    void OnDestroy()
    {
        EndTurn.onClick.RemoveAllListeners();
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
        G.main.EndTurn();
    }
    
    void OnClickRollButton()
    {
        G.main.ReRoll();
    }

    public void DisableHud()
    {
        G.main.rollDicesZone.canDrag = false;
        EndTurn.interactable = false;
    }

    public void EnableHud()
    {
        G.main.rollDicesZone.canDrag = true;
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