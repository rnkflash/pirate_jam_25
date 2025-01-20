using System;
using _game.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _game
{
    public class HeroPanelView : MonoBehaviour, IHeroPanelView
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _heroImage;
        [SerializeField] private HeroHealthView _heroHealthView;
        [SerializeField] private Image _frameImage;

        public void SetSprite(Sprite sprite)
        {
            _heroImage.sprite = sprite;
        }

        public event Action OnClick;

        public void SetState(bool state)
        {
            _frameImage.color = state ? UIUtils.selectedFrame : UIUtils.unselectedFrame;
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
    }

    public interface IHeroPanelView
    {
        void SetState(bool state);
        IHeroHealthView GetHeroHealthView();
        void SetName(string name);
        void SetSprite(Sprite sprite);
        event Action OnClick;
    }
}