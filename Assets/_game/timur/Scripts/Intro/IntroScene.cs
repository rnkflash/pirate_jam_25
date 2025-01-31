using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Подключаем для загрузки сцен

namespace _game.Intro
{
    public class IntroScene : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _images;
        [SerializeField] private List<string> _texts;
        [SerializeField] private Image _imageDisplay; // UI Image для отображения
        [SerializeField] private CanvasGroup _canvasGroup; // Для эффекта затемнения
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private List<int> _seconds;

        private int _currentIndex = 0;
        private Coroutine _currentCoroutine;
        private bool _canClick = true; // Флаг для блокировки клика

        private void Start()
        {
            _imageDisplay.sprite = null; // Очищаем изображение в начале
            _canvasGroup.alpha = 1; // Полная видимость в начале
            _currentCoroutine = StartCoroutine(ShowImagesWithFade());
        }

        public void OnClicked()
        {
            if (!_canClick) return; // Блокируем клик

            Debug.Log("Timur clicked");

            _canClick = false; // Блокируем повторный клик
            StartCoroutine(EnableClickAfterDelay(2f)); // Разблокируем клик через 2 секунды

            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }

            _currentIndex++; // Переход к следующему кадру

            if (_currentIndex >= _images.Count)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                return;
            }

            _currentCoroutine = StartCoroutine(ShowNextImage());
        }

        private IEnumerator EnableClickAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _canClick = true; // Разрешаем снова кликать
        }

        private IEnumerator ShowImagesWithFade()
        {
            while (_currentIndex < _images.Count)
            {
                yield return StartCoroutine(ShowNextImage());
                yield return new WaitForSeconds(_seconds[_currentIndex]);
                _currentIndex++;
            }
        }

        private IEnumerator ShowNextImage()
        {
            // 🔻 Плавное затемнение
            if (_currentIndex != 0)
                yield return StartCoroutine(FadeOut());

            _imageDisplay.sprite = _images[_currentIndex]; // Меняем изображение
            _text.text = _texts[_currentIndex];

            // 🔺 Плавное появление
            yield return StartCoroutine(FadeIn());
        }

        private IEnumerator FadeOut()
        {
            for (float t = 0; t < 1; t += Time.deltaTime / 0.5f) // 0.5 сек затемнение
            {
                _canvasGroup.alpha = 1 - t;
                yield return null;
            }
            _canvasGroup.alpha = 0;
        }

        private IEnumerator FadeIn()
        {
            for (float t = 0; t < 1; t += Time.deltaTime / 0.5f) // 0.5 сек проявление
            {
                _canvasGroup.alpha = t;
                yield return null;
            }
            _canvasGroup.alpha = 1;
        }
    }
}
