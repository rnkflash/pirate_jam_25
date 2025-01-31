using System;
using DG.Tweening;
using UnityEngine;

namespace _game.rnk.Scripts.battleSystem
{
    public class Selectable : MonoBehaviour
    {
        public GameObject selection;
        [NonSerialized] public bool canBeSelected;

        public void EnableSelection(bool canBeSelected)
        {
            this.canBeSelected = canBeSelected;
            selection.SetActive(canBeSelected);
        }
        
        private RectTransform uiElement;
        public float minScale = 0.9f;
        public float maxScale = 1.1f;
        public float duration = 1f;

        private void Start()
        {
            uiElement = selection.GetComponent<RectTransform>();
            StartLoopingScale();
        }

        private void StartLoopingScale()
        {
            uiElement.DOScale(minScale, duration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => uiElement.DOScale(maxScale, duration)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(StartLoopingScale));
        }
    }
}