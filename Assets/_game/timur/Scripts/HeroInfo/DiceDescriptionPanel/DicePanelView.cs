using UnityEngine;
using UnityEngine.UI;

namespace _game.HeroInfo.DiceDescriptionPanel
{
    public class DicePanelView : MonoBehaviour, IDicePanelView
    {
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

    public interface IDicePanelView
    {
        
    }
}