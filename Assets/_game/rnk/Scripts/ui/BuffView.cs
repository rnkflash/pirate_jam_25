using System;
using _game.rnk.Scripts.tags;
using _game.rnk.Scripts.util;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _game.rnk.Scripts.ui
{
    public class BuffView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Image image;
        public TMP_Text text;
        
        [NonSerialized] public BuffState state;
        [NonSerialized] public BuffList buffList;
        Tween punchTween;
        
        public void SetState(BuffList buffList, BuffState state)
        {
            this.buffList = buffList;
            this.state = state;
            state.view = this;

            image.sprite = state.model.Get<TagSprite>().sprite;
            UpdateState();
        }
        
        public void FreeState()
        {
            state.view = null;
            state = null;
            buffList = null;
        }

        public void UpdateState()
        {
            text.text = state.turnsLeft.ToString();
        }

        public void Punch()
        {
            punchTween.Kill(true);
            punchTween = transform.DOPunchScale(new Vector3(1.0f, 1.0f, 1.0f), 0.2f);
        }
        public void Remove()
        {
            buffList.DestroyView(this);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            G.hud.tooltip.Show(TryGetSomethingDesc());
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            G.hud.tooltip.Hide();
        }
        
        string TryGetSomethingDesc()
        {
            if (state == null) return null;
            var desc = "";
            var model = state.model;
            var values = model.Get<TagValue>()?.values ?? Array.Empty<int>();
            if (model.Is<TagName>(out var tn)) desc += tn.loc + ". \n";
            if (model.Is<TagDescription>(out var td)) desc += td.loc.WithValues(values);
            return desc;
        }
    }
}