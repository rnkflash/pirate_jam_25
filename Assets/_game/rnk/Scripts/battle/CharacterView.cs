using System;
using _game.rnk.Scripts.tags;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _game.rnk.Scripts.battleSystem
{
    public class CharacterView : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Image avatarImage;
        public TMP_Text nameText;
        public TextMeshProUGUI healthText;
        public DiceZone diceZone;
        public Damageable damageable;

        [NonSerialized] public CharacterState state;
        private Tween _currentTween;
        private Vector2 _originalAnchoredPosition;
        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetState(CharacterState characterState)
        {
            state = characterState;
            avatarImage.sprite = state.bodyState.model.Get<TagSprite>().sprite;
            nameText.text = state.weaponState.model.Get<TagName>().loc;
            healthText.text = state.health + "/" + state.maxHealth + " +" + state.armor;

            nameText.color = state.weaponState.model.Get<TagTint>().color;
            
            damageable.SetState(state);
            healthText.color = state.weaponState.model.Get<TagTint>().color;

            diceZone.OnClickDice += OnDiceClick;

            characterState.diceZone = diceZone;
            characterState.view = this;
        }

        void OnDestroy()
        {
            diceZone.OnClickDice -= OnDiceClick;
        }
        
        void OnDiceClick(DiceInteractiveObject dice)
        {
            G.battle.OnDiceClickInCharacterView(this, dice);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (G.IsInBattle)
                G.battle.CharacterClicked(state);
            else
                G.crawler.CharacterClicked(state);
        }

        public void EnterBattleMode()
        {
            diceZone.gameObject.SetActive(true);
        }
        
        public void FinishBattleMode()
        {
            diceZone.gameObject.SetActive(false);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;
            _originalAnchoredPosition = _rectTransform.anchoredPosition;
            _currentTween?.Kill();
            Vector2 targetPosition = new Vector2(
                _originalAnchoredPosition.x - 8,
                _originalAnchoredPosition.y - 16
            );
            _currentTween = _rectTransform.DOAnchorPos(targetPosition, 0.2f);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            _currentTween?.Kill();
            _currentTween = _rectTransform.DOAnchorPos(_originalAnchoredPosition, 0.1f).OnComplete(() =>
            {
                transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;
            });
        }
    }
}