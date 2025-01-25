using UnityEngine;
using UnityEngine.UI;

namespace _game.HeroInfo
{
    public class DiceFaceView : MonoBehaviour, IDiceFaceView
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _colorImage;

        public void SetSelected(bool state)
        {
            if (state)
            {
                Debug.Log("timur NOT RELEASE");
                _image.color = Color.white;   
            }
            else
            {
                Debug.Log("timur RELEASE");
                _image.color = new Color32(0x65, 0x68, 0x97, 0xFF);
            }
        }

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
        void SetSelected(bool state);
        void SetImage(Sprite image);
        void SetColor(Color color);
    }
}