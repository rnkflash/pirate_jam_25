using System;
using _game.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _game
{
    public class HeroPanelView : MonoBehaviour, IHeroPanelView, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _heroImage;
        [SerializeField] private HeroHealthView _heroHealthView;
        [SerializeField] private Image _frameImage;
        [SerializeField] private Image _bgImage;
        private Tween _currentTween;
        private bool _selected;
        private bool _highlighted;
        private RectTransform _rectTransform;
        private Vector2 _originalAnchoredPosition;

        private void Start()
        {
            // Получаем RectTransform и сохраняем исходную позицию
            _rectTransform = GetComponent<RectTransform>();
            _originalAnchoredPosition = _rectTransform.anchoredPosition;
        }

        public void SetSprite(Sprite sprite)
        {
            _heroImage.sprite = sprite;
        }

        public event Action OnClick;
        
        public void OnPointerEnter()
        {
            _highlighted = true;
            // _currentTween?.Kill();
            _bgImage.color = new Color32(0x20, 0x20, 0x20, 0xFF);
            // _currentTween = transform.DOScale(1.1f, 0.2f).OnComplete(() =>
            // {
            // }).SetEase(Ease.OutElastic);
        }
        
        public void OnPointerExit()
        {
            _highlighted = false;
             if (!_selected)
             {
            //     _currentTween?.Kill();
                 _bgImage.color = Color.white;
            //     _currentTween = transform.DOScale(1f, 0.1f);
             }
        }

        public void SetState(bool state)
        {
            _frameImage.color = state ? UIUtils.selectedFrame : UIUtils.unselectedFrame;
        }

        public void SetSelectedVariable(bool selected)
        {
            if (_selected && !selected)
            {
                _selected = false;
                if(!_highlighted)
                    OnPointerExit();
            }
            else
            {
                _selected = selected;
            }
        }

        public IHeroHealthView GetHeroHealthView() => _heroHealthView;

        public void SetName(string name)
        {
            _nameText.text = name;
        }

        public void FakeClick()
        {
            OnClick?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Прекращаем текущий твин, если он активен
            SetState(true);
            _currentTween?.Kill();
            Vector2 targetPosition = new Vector2(
                _originalAnchoredPosition.x - 8, // Влево на 15
                _originalAnchoredPosition.y - 16  // Вниз на 15
            );
            // Двигаем объект вниз
            _currentTween = _rectTransform.DOAnchorPos(targetPosition, 0.2f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SetState(false);
            // Прекращаем текущий твин, если он активен
            _currentTween?.Kill();
            // Возвращаем объект в исходное положение
            _currentTween = _rectTransform.DOAnchorPos(_originalAnchoredPosition, 0.1f);
        }
    }

    public interface IHeroPanelView
    {
        void SetState(bool state);
        void SetSelectedVariable(bool selected);
        IHeroHealthView GetHeroHealthView();
        void SetName(string name);
        void SetSprite(Sprite sprite);
        event Action OnClick;
    }
}