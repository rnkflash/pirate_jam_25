using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _game.HeroInfo
{
    public class DiceFaceView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _colorImage;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _iconImage;

        public void SetSelected(bool state)
        {
            if (state)
            {
                _image.color = Color.white;   
            }
            else
            {
                _image.color = new Color32(0x65, 0x68, 0x97, 0xFF);
            }
        }

        public void SetIcon(Sprite image)
        {
            _iconImage.sprite = image;
        }

        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}