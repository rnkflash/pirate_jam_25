using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InteractiveObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image imageFace;
    public Image imageBg;
    public Transform scaleRoot;
    public CardState state;
    public MoveableBase moveable;
    public DraggableSmoothDamp draggable;
    public SortingGroup sortingGroup;

    public int order;

    [NonSerialized] public CardZone zone;
    bool isMouseOver;
    [NonSerialized] public float Width = 1;
    [SerializeField] float origWidth = 200;
    void Awake()
    {
        draggable = GetComponent<DraggableSmoothDamp>();
        Width = origWidth;
    }

    public void SetState(CardState cardState)
    {
        state = cardState;
        state.view = this;

        if (state.model.Is<TagCardView>(out var av))
        {
            imageFace.sprite = av.face;
            imageBg.sprite = av.bg;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (zone != null)
        {
            zone.OnClickDice?.Invoke(this);
        }
    }

    void Update()
    {
        if (sortingGroup != null)
            sortingGroup.sortingOrder = isMouseOver || draggable.isDragging ? 9999 : order;
    }

    Tween punchTwee;
    public void Punch()
    {
        punchTwee.Kill(true);
        punchTwee=transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
    }

    public void Leave()
    {
        if (zone != null)
            zone.Release(this);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (G.hover_card != null) return;
        if (G.drag_card != null) return;
        
        isMouseOver = true;
        Width = origWidth * 1.25f;
        
        if (scaleRoot)
        {
            G.hover_card = this;
            scaleRoot.DOKill();
            //scaleRoot.transform.localScale = Vector3.one * 1.25f;
            scaleRoot.DOScale(1.25f, 0.2f);
        }

        if (!draggable.isDragging)
        {
            var desc = TryGetSomethingDesc();
            if (!string.IsNullOrEmpty(desc))
                G.hud.tooltip.Show(desc);
        }
    }

    string TryGetSomethingDesc()
    {
        if (state != null)
        {
            var desc = "";
            if (state.model.Is<TagName>(out var tn)) desc += tn.loc + ". ";
            if (state.model.Is<TagDescription>(out var td)) desc += td.loc;
            return desc;
        }

        return null;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        G.hover_card = null;
        Width = origWidth;

        if (scaleRoot)
        {
            scaleRoot.DOKill();
            scaleRoot.DOScale(1f, 0.2f);
        }
        
        G.hud.tooltip.Hide();
    }

}