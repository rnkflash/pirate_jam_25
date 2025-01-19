using System;
using System.Collections;
using _game.rnk.Scripts.artefacts;
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
        
        void Awake()
        {
            draggable = GetComponent<DraggableSmoothDamp>();
            Width = origWidth;
        }

        public void SetState(DiceState diceState)
        {
            state = diceState;
            state.interactiveObject = this;

            view.bg.color = diceState.owner.weaponState.model.Get<TagTint>().color;
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
            StartCoroutine(ActuallyRoll());
        }

        IEnumerator ActuallyRoll()
        {
            var value = 1 + Random.Range(0, state.model.Get<TagSides>().sides);
            
            view.valueText.text = value.ToString();
            
            G.audio.Play<SFX_Roll>();

            Punch();
            
            yield return new WaitForSeconds(0.20f);

            bool valueOverwritten = false;
            var rollFill = G.main.interactor.FindAll<IRollFilter>();
            foreach (var rfilter in rollFill)
            {
                var newValue = rfilter.OverwriteRoll(state, value);
                if (newValue != value)
                {
                    valueOverwritten = true;
                    value = newValue;    
                }
            }

            if (valueOverwritten)
            {
                view.valueText.text = value.ToString();
            
                G.audio.Play<SFX_Roll>();
            
                Punch();
            
                yield return new WaitForSeconds(0.20f);
            }
        } 

        public void Punch()
        {
            punchTween.Kill(true);
            punchTween=transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
        }

    }
}