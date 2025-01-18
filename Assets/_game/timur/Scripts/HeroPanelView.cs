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

        public IHeroHealthView GetHeroHealthView() => _heroHealthView;

        public void SetName(string name)
        {
            _nameText.text = name;
        }

        public void FakeClick()
        {
            Debug.Log("timur CLICK!");
        }
    }

    public interface IHeroPanelView
    {
        IHeroHealthView GetHeroHealthView();
        void SetName(string name);
    }
}