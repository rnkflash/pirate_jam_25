using UnityEngine;
using UnityEngine.UI;

namespace _game.HeroInfo
{
    public class DiceFaceView : MonoBehaviour, IDiceFaceView
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _colorImage;

        public void SetImage(Sprite image)
        {
            _image.sprite = image;
        }

        public void SetColor(Color color)
        {
            _colorImage.color = color;
        }
    }

    public interface IDiceFaceView
    {
        void SetImage(Sprite image);
        void SetColor(Color color);
    }
}