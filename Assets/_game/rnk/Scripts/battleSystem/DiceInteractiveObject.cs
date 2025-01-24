using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _game.rnk.Scripts.dice.face;
using _game.rnk.Scripts.tags;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
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
        public Transform lineRenderersRoot;

        [NonSerialized] public DiceState state;
        [NonSerialized] public DiceZone zone;
        [NonSerialized] public float Width = 1;
        
        Tween punchTween;
        bool isMouseOver;
        Color ownerColor;
        int lastRoll;

        List<ITarget> targets = new List<ITarget>();
        List<LineRendererUI> lineRenderers = new List<LineRendererUI>();
        
        void Awake()
        {
            draggable = GetComponent<DraggableSmoothDamp>();
            
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

            if (targets.Count > 0)
            {
                if (lineRenderers.Count < targets.Count)
                {
                    CreateLineRenderers(targets.Count - lineRenderers.Count);
                }
                else 
                if (lineRenderers.Count > targets.Count)
                {
                    DestroyLineRenderers(lineRenderers.Count - targets.Count);
                }

                for (int i = 0; i < targets.Count; i++)
                {
                    lineRenderers[i].UpdateLine(transform.position, targets[i].GetView().transform.position, ownerColor);
                }
            }
            else
            {
                if (lineRenderers.Count > 0)
                {
                    DestroyLineRenderers(lineRenderers.Count);
                }
            }
        }

        void DestroyLineRenderers(int count)
        {
            var toDelete = Math.Min(count, lineRenderers.Count);
            for (int i = 0; i < toDelete; i++)
            {
                var lr = lineRenderers[i];
                Destroy(lr.gameObject);
            }
            lineRenderers.RemoveRange(0, toDelete);
        }

        void CreateLineRenderers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = new GameObject();
                obj.transform.SetParent(lineRenderersRoot);
                obj.transform.localPosition = Vector3.zero;
                var img = obj.AddComponent<Image>();
                ((RectTransform)obj.transform).sizeDelta = new Vector2(1,1);
                var comp = obj.AddComponent<LineRendererUI>();
                comp.Init();
                lineRenderers.Add(comp);
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

                if (Mathf.Approximately(overrideScale, -1))
                {
                    scaleRoot.DOKill();
                    //scaleRoot.transform.localScale = Vector3.one * 1.25f;
                    scaleRoot.DOScale(1.25f, 0.2f);    
                }
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
            var face = state.face;
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
                if (Mathf.Approximately(overrideScale, -1))
                {
                    scaleRoot.DOKill();
                    scaleRoot.DOScale(1f, 0.2f);
                }
            }
        
            G.hud.tooltip.Hide();
        }

        float overrideScale = -1;
        public void SetScaleOverride(float scale)
        {
            overrideScale = scale;
            if (scaleRoot)
            {
                scaleRoot.DOKill();
                scaleRoot.DOScale(overrideScale, 0.2f);
            }
            
            if (scale == 1.0)
            {
                overrideScale = -1;    
            }
        }

        public IEnumerator Roll()
        {
            var rollTimes = Random.Range(2, 6);
            for (int i = 0; i < rollTimes; i++)
            {
                RollDice();
                G.audio.Play<SFX_Roll>();
                Punch();
                yield return new WaitForSeconds(0.2f);
            }
        }

        public void ChangeFace()
        {
            var face = state.face;
            if (face.Is<TagSprite>(out var sprite))
            {
                view.sprite.SetAlpha(1f);
                view.sprite.sprite = sprite.sprite;
            }
            else
            {
                view.sprite.SetAlpha(0f);
            }
            
            if (face.Is<TagAction>(out var action))
            {
                if (action.action == ActionType.Blank)
                    view.valueText.text = "X";
                else
                    view.valueText.text = action.value.ToString();
            }
        }

        void RollDice()
        {
            lastRoll = state.rollValue;
            while (state.rollValue == lastRoll)
            {
                lastRoll = state.rollValue;
                state.rollValue = 1 + Random.Range(0, state.model.Get<TagSides>().sides);    
            }
            ChangeFace();
        }

        public void Punch()
        {
            punchTween.Kill(true);
            punchTween=transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
        }

        public void SetTargets(List<ITarget> targets)
        {
            this.targets = targets;
        }
        
        public void ClearTargets()
        {
            targets.Clear();
        }
        public List<ITarget> GetTargets()
        {
            return targets;
        }
    }
}