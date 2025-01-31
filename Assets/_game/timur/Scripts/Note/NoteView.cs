using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _game.Note
{
    public class NoteView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float animationDuration = 0.5f;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Transform _transform;

        private Vector3 originalScale;

        private void Awake()
        {
            originalScale = _transform.localScale;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public void Show(string text)
        {
            _text.text = text;
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            _transform.localScale = Vector3.zero; // Начинаем с нулевого размера
            _transform.DOScale(originalScale, animationDuration).SetEase(Ease.OutBack);
        }

        private void Update()
        {
            
            /*if (Input.GetKeyDown(KeyCode.Space))
            {
                Show("Здарова, тварына");
            }*/
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}