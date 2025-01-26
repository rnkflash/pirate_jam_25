using System;
using System.Collections.Generic;
using _game.Inventory;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.tags;
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

        [SerializeField] private TextMeshProUGUI _healtText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _heroIcon;
        [SerializeField] private TextMeshProUGUI _loreText;
        
        private Vector2 _originalAnchoredPosition;
        private RectTransform _rectTransform;
        private bool _show;
        private CharacterState _state;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalAnchoredPosition = _rectTransform.anchoredPosition;
        }
        
        public void SetState(CharacterState state)
        {
            var similarClick = state == _state;
            _state = state;
            if (!similarClick)
            {
                ResetDiceIcons();
                _show = false;
            }
            _show = !_show;
            if (similarClick && !_show)
            {
                ResetDiceIcons();
            }
            SetShowState(_show);
            _healtText.text = state.health + "/" + state.maxHealth + " +" + state.armor;
            _healtText.color = state.weaponState.model.Get<TagTint>().color;
            _nameText.text = state.weaponState.model.Get<TagName>().loc;
            _loreText.text = state.weaponState.model.Get<TagDescription>().loc;
            _heroIcon.sprite = state.bodyState.model.Get<TagSprite>().sprite;
            var q = state.diceStates;
            foreach (var qq in q)
            {
                var ww = qq.model.Get<TagDefaultFaces>();
                for(int i = 0; i < 6; i++)
                {
                    _diceFaces[i].SetIcon(ww.faces[i].Get<TagSprite>().sprite);
                    _diceFaces[i].SetText(ww.faces[i].Get<TagAction>().value.ToString());
                    i++;
                }
            }
        }

        private void ResetDiceIcons()
        {
            foreach (var diceFaceView in _diceFaces)
            {
                diceFaceView.SetIcon(null);
                diceFaceView.SetText("");
            }
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
                // _diceFaces[i].SetImage(diceFaces[i].sprite);
                // _diceFaces[i].SetColor(diceFaces[i].colorValue);
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
                Vector2 loweredPosition = _originalAnchoredPosition + new Vector2(0f, -50f);
                _rectTransform.anchoredPosition = loweredPosition;

                _canvasGroup.interactable = true;
                _canvasGroup.alpha = 1f;

                _rectTransform.DOAnchorPosY(_originalAnchoredPosition.y, 0.2f)
                    .SetEase(Ease.OutBack)
                    .OnStart(() =>
                    {
                    })
                    .OnComplete(() =>
                    {
                        _rectTransform.anchoredPosition = _originalAnchoredPosition;
                    });
            }
            else
            {
                float targetY = _rectTransform.anchoredPosition.y - 50f;

                Sequence sequence = DOTween.Sequence();

                sequence.Join(
                    _rectTransform.DOAnchorPosY(targetY, 0.05f)
                        .SetEase(Ease.OutBounce)
                );

                sequence.Join(
                    _canvasGroup.DOFade(0f, 0.05f)
                );

                sequence.OnStart(() =>
                    {
                        Debug.Log("Tween started (state false)");
                    })
                    .OnComplete(() =>
                    {
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
        
        [SerializeField] private Image _left;
        [SerializeField] private Image _right;
        [SerializeField] private CanvasGroup _leftCanvasGroup;
        [SerializeField] private CanvasGroup _rightCanvasGroup;
        public void OnClickLeft()
        {
            _leftCanvasGroup.alpha = 1;
            _leftCanvasGroup.interactable = true;
            _leftCanvasGroup.blocksRaycasts = true;
            _rightCanvasGroup.alpha = 0;
            _rightCanvasGroup.interactable = false;
            _rightCanvasGroup.blocksRaycasts = false;
            _left.gameObject.SetActive(true);
            _right.gameObject.SetActive(false);
        }

        public void OnClickRight()
        {
            _rightCanvasGroup.alpha = 1;
            _rightCanvasGroup.interactable = true;
            _rightCanvasGroup.blocksRaycasts = true;
            _leftCanvasGroup.alpha = 0;
            _leftCanvasGroup.interactable = false;
            _leftCanvasGroup.blocksRaycasts = false;
            _left.gameObject.SetActive(false);
            _right.gameObject.SetActive(true);
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