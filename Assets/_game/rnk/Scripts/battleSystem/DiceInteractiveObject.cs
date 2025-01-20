using System;
using System.Collections;
using _game.rnk.Scripts.artefacts;
using _game.rnk.Scripts.dice.face;
using _game.rnk.Scripts.tags;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace _game.rnk.Scripts.battleSystem
{
    public class DiceInteractiveObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public DiceView view;
        public Transform scaleRoot;
        public MoveableBase moveable;
        public DraggableSmoothDamp draggable;
        public SortingGroup sortingGroup;
        public int order;
        [SerializeField] float origWidth = 200;

        [NonSerialized] public DiceState state;
        [NonSerialized] public DiceZone zone;
        [NonSerialized] public float Width = 1;

        Tween punchTween;
        bool isMouseOver;
        BlankFace blank = new BlankFace();
        LineRendererUI lineRendererUI;
        Color ownerColor;

        void Awake()
        {
            draggable = GetComponent<DraggableSmoothDamp>();
            lineRendererUI = GetComponentInChildren<LineRendererUI>();
            
            Width = origWidth;
        }

        public void SetState(DiceState diceState)
        {
            state = diceState;
            state.interactiveObject = this;
            CMSEntity colorProvider = diceState.owner.bodyState.model;
            if (diceState.owner is CharacterState characterState)
                colorProvider = characterState.weaponState.model;
            ownerColor = colorProvider.Get<TagTint>().color;
            view.bg.color = ownerColor;

            ChangeFace();
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

            if (state.owner.diceZone != null)
            {
                lineRendererUI.CreateLine(transform.position, state.owner.diceZone.transform.position, ownerColor);
            }
        }

        public void Leave()
        {
            if (zone != null)
                zone.Release(this);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (G.hover_dice != null) return;
            if (G.drag_dice != null) return;
        
            isMouseOver = true;
            Width = origWidth * 1.25f;
        
            if (scaleRoot)
            {
                G.hover_dice = this;
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
            if (state == null) return null;
            
            var desc = "";
            var face = GetFace();
            if (face.Is<TagName>(out var tn)) desc += tn.loc + ". \n";
            if (face.Is<TagDescription>(out var td)) desc += td.loc;
            return desc;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isMouseOver = false;
            G.hover_dice = null;
            Width = origWidth;

            if (scaleRoot)
            {
                scaleRoot.DOKill();
                scaleRoot.DOScale(1f, 0.2f);
            }
        
            G.hud.tooltip.Hide();
        }

        public void Roll()
        {
            StopAllCoroutines();
            StartCoroutine(RollAnimation());
        }

        IEnumerator RollAnimation()
        {
            var rollTimes = Random.Range(2, 6);
            for (int i = 0; i < rollTimes; i++)
            {
                RollDice();
                G.audio.Play<SFX_Roll>();
                Punch();
                yield return new WaitForSeconds(0.15f);
            }
        }

        public void ChangeFace()
        {
            var face = GetFace();
            if (face.Is<TagSprite>(out var sprite))
            {
                SpriteUtil.SetImageAlpha(view.sprite, 1f);
                view.sprite.sprite = sprite.sprite;
            }
            else
            {
                SpriteUtil.SetImageAlpha(view.sprite, 0f);
            }
            view.valueText.text = "X";
            
            if (face.Is<TagValue>(out var value))
            {
                view.valueText.text = value.value.ToString();
            }
        }

        FaceBase GetFace()
        {
            var faces = state.model.Get<TagDefaultFaces>().faces;
            return faces[state.rollValue] ?? blank;
        }

        int lastRoll;
        void RollDice()
        {
            lastRoll = state.rollValue;
            while (state.rollValue == lastRoll)
            {
                state.rollValue = 1 + Random.Range(0, state.model.Get<TagSides>().sides);    
            }
            ChangeFace();
        }

        public void Punch()
        {
            punchTween.Kill(true);
            punchTween=transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
        }

    }
}