using _game.rnk.Scripts.battleSystem;
using DG.Tweening;
using UnityEngine;

namespace _game.Inventory
{
    public class InventoryView : MonoBehaviour, IInventoryView
    {
        [SerializeField] private InventoryItemView _item;
        [SerializeField] private Transform _itemHolderTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Vector2 _originalAnchoredPosition;
        private CharacterState _state;
        private bool _shown;
        public InventoryItemView GetItemPrefab() => _item;
        public Transform GetItemsParent() => _itemHolderTransform;

        public void SetState(CharacterState state)
        {
            var similarClick = state == _state;
            _state = state;
            if (similarClick)
            {
                _shown = false;
                SetState(false);
            }
            else if(!_shown)
            {
                _shown = true;
                SetState(true);
            }
        }
        
        public void SetState(bool state)
        {
            if (state)
            {
                //pokazat anim sleva napravo
                Vector2 loweredPosition = _originalAnchoredPosition + new Vector2(-30f, 0f);
                _rectTransform.anchoredPosition = loweredPosition;

                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.alpha = 1f;

                _rectTransform.DOAnchorPosX(_originalAnchoredPosition.x, 0.18f)
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
                float targetY = _rectTransform.anchoredPosition.x - 30f;

                Sequence sequence = DOTween.Sequence();

                sequence.Join(
                    _rectTransform.DOAnchorPosX(targetY, 0.05f)
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
                        _canvasGroup.blocksRaycasts = false;
                        _canvasGroup.alpha = 0f;
                        _rectTransform.anchoredPosition = _originalAnchoredPosition;
                    });
            }
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalAnchoredPosition = _rectTransform.anchoredPosition;
        }
    }

    public interface IInventoryView
    {
        InventoryItemView GetItemPrefab();
        Transform GetItemsParent();
        void SetState(bool state);
    }
}