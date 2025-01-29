using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _game.Dialog
{
    public class DialogWindow : MonoBehaviour
    {
        public float delayBetweenCharacters = 0.07f; // Задержка между символами
        public float delayBetweenTexts = 3.0f; // Задержка между текстами
        [SerializeField] private TextMeshProUGUI _text;
        public float slideDuration = 0.5f; // Длительность анимации падения окна
        public float slideOffset = 500f; // Насколько высоко окно начинается (смещение сверху)
        
        [SerializeField] private RectTransform _rectTransform; // RectTransform окна для анимации
        [SerializeField] private GameObject _window;
        [SerializeField] private Sprite _1;
        [SerializeField] private Sprite _2;
        [SerializeField] private Sprite _3;
        [SerializeField] private Image _image;

        private IEnumerator ShowTextCoroutine(List<DialogData> dialogDatas)
        {
            // Анимация падения окна
            _image.sprite = dialogDatas[0].sprite;
            yield return SlideWindowIntoPlace().WaitForCompletion();
            
            // Ждем 1 секунду перед печатью текста
            yield return new WaitForSeconds(1f);

            // Печатаем тексты
            for (int i = 0; i < dialogDatas.Count; i++)
            {
                yield return StartCoroutine(TypeText(dialogDatas[i].text, dialogDatas[i].sprite));

                // После последнего текста ждем delayBetweenTexts и выключаем объект
                if (i == dialogDatas.Count - 1)
                {
                    yield return new WaitForSeconds(delayBetweenTexts);
                    yield return SlideWindowOutPlace().WaitForCompletion();
                }
                else
                {
                    yield return new WaitForSeconds(delayBetweenTexts);
                }
            }
        }

        // Анимация падения окна
        private Tween SlideWindowIntoPlace()
        {
            // Сохраняем начальную позицию окна
            Vector2 originalPosition = new Vector2(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y - slideOffset);

            // Анимация перемещения окна в исходную позицию
            return _rectTransform.DOAnchorPos(originalPosition, slideDuration).SetEase(Ease.InBack );
        }
        
        private Tween SlideWindowOutPlace()
        {
            // Сохраняем начальную позицию окна
            Vector2 originalPosition = new Vector2(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y + slideOffset);

            // Анимация перемещения окна в исходную позицию
            return _rectTransform.DOAnchorPos(originalPosition, slideDuration).SetEase(Ease.OutBack );
        }

        // Печать текста по одной букве
        private IEnumerator TypeText(string text, Sprite sprite)
        {
            _image.sprite = sprite;
            _text.text = ""; // Очищаем текст перед печатью
            for (int i = 0; i < text.Length; i++)
            {
                _text.text += text[i]; // Добавляем по одному символу
                yield return new WaitForSeconds(delayBetweenCharacters); // Задержка между символами
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                string[] texts = new string[]
                {
                    "Мне кажется я уже видел это ранее..",
                    "Конечно!",
                    "Тот самый сон..."
                };
                var q1 = new DialogData(_1, "Кажется мы что-то забыли..");
                var q2 = new DialogData(_2, "А мне так не кажется, успокойся");
                var q3 = new DialogData(_1, "Я почти спокоен");
                var q4 = new DialogData(_3, "Так, кажется видно куда нам дальше");
                List<DialogData> qq = new();
                qq.Add(q1);
                qq.Add(q2);
                qq.Add(q3);
                qq.Add(q4);
                

                StartCoroutine(ShowTextCoroutine(qq));
            }
        }

        public class DialogData
        {
            public DialogData(Sprite sprite, string text)
            {
                this.sprite = sprite;
                this.text = text;
            }
            public Sprite sprite;
            public string text;
        }
    }
}