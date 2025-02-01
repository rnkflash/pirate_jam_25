using System;
using _game.rnk.Scripts.tags;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _game.rnk.Scripts.ui
{
    public class BuffView : MonoBehaviour
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
            Punch();
        }

        public void Punch()
        {
            punchTween.Kill(true);
            punchTween = transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
        }
        public void Remove()
        {
            buffList.DestroyView(this);
        }
    }
}