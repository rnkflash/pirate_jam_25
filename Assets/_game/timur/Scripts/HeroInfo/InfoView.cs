using System;
using System.Collections.Generic;
using _game.Inventory;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _game.HeroInfo
{
    public class InfoView : MonoBehaviour, IInfoView
    {
        [Header("WeaponSection")]
        [SerializeField] private TextMeshProUGUI _weaponNameText;
        [SerializeField] private Image _weaponImage;
        
        [Header("DicesSection")]
        [SerializeField] private List<DiceFaceView> _diceFaces;
        
        [Header("BodySection")]
        [SerializeField] private Image _bodyImage;
        [SerializeField] private HeroHealthView _heroHealthView;

        [Header("BodyInfoSection")] 
        [SerializeField] private TextMeshProUGUI _bodyNameText;

        [Header("General")] 
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Inventory")] 
        [SerializeField] private InventoryView _view;
        
        private Vector2 _originalAnchoredPosition;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalAnchoredPosition = _rectTransform.anchoredPosition;
        }
        
        public void SetWeaponName(string name)
        {
            _weaponNameText.text = name;
        }

        public void SetWeaponImage(Sprite image)
        {
            _weaponImage.sprite = image;
        }

        public void SetDiceFacesView(List<DiceFaceModel> diceFaces)
        {
            for (int i = 0; i < diceFaces.Count; i++)
            {
                _diceFaces[i].SetImage(diceFaces[i].sprite);
                _diceFaces[i].SetColor(diceFaces[i].colorValue);
            }
        }

        public void SetBodyImage(Image image)
        {
            
        }

        public void SetBodyName(string name)
        {
            _bodyNameText.text = name;
        }

        public HeroHealthView GetHeroHealthView() => _heroHealthView;

        public void SetShowState(bool state)
        {
            if (state)
            {
                // Смещаем панель вниз на 30 по оси Y относительно родителя
                Vector2 loweredPosition = _originalAnchoredPosition + new Vector2(0f, -30f);
                _rectTransform.anchoredPosition = loweredPosition;

                _canvasGroup.interactable = true;
                _canvasGroup.alpha = 1f;

                _rectTransform.DOAnchorPosY(_originalAnchoredPosition.y, 0.18f)
                    .SetEase(Ease.OutBack)
                    .OnStart(() =>
                    {
                        Debug.Log("Tween started (state true)");
                    })
                    .OnComplete(() =>
                    {
                        // Гарантируем восстановление исходной позиции
                        _rectTransform.anchoredPosition = _originalAnchoredPosition;
                    });
            }
            else
            {
                // Вычисляем целевую позицию по оси Y для опускания
                float targetY = _rectTransform.anchoredPosition.y - 30f;

                // Запускаем параллельные tween: для движения и для затухания
                Sequence sequence = DOTween.Sequence();

                // Добавляем tween перемещения по оси Y используя RectTransform
                sequence.Join(
                    _rectTransform.DOAnchorPosY(targetY, 0.11f)
                        .SetEase(Ease.OutBounce)
                );

                // Добавляем tween для изменения прозрачности CanvasGroup
                sequence.Join(
                    _canvasGroup.DOFade(0f, 0.11f)
                );

                sequence.OnStart(() =>
                    {
                        Debug.Log("Tween started (state false)");
                    })
                    .OnComplete(() =>
                    {
                        // По завершении отключаем взаимодействие и восстанавливаем позицию
                        _canvasGroup.interactable = false;
                        _canvasGroup.alpha = 0f;
                        _rectTransform.anchoredPosition = _originalAnchoredPosition;
                    });
            }
        }
        
        public List<DiceFaceView> GetDiceFaces() => _diceFaces;
        public event Action OnInventoryClicked;
        public IInventoryView GetInventoryView() => _view;

        public void OnInventoryButtonClicked()
        {
            OnInventoryClicked?.Invoke();
        }
    }

    public interface IInfoView
    {
        void SetWeaponName(string name);
        void SetWeaponImage(Sprite image);
        void SetDiceFacesView(List<DiceFaceModel> diceFaces);
        void SetBodyImage(Image image);
        void SetBodyName(string name);
        HeroHealthView GetHeroHealthView();

        void SetShowState(bool state);

        List<DiceFaceView> GetDiceFaces();
        
        event Action OnInventoryClicked;
        IInventoryView GetInventoryView();
    }
}