using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _game.tinypack.source.UI
{
    public class SpriteVFX : MonoBehaviour
    {
        Image img;
        Tween tween;
        Sequence alphaTween;

        void Start()
        {
            img = GetComponent<Image>();
            img.SetAlpha(0.0f);
        }

        public void Bahni(Vector3 pos)
        {
            transform.position = pos;
            
            img.SetAlpha(0.0f);
            alphaTween?.Kill(true);
            alphaTween = DOTween.Sequence();
            alphaTween.Append(img.DOFade(1.0f, 0.25f));
            alphaTween.Append(img.DOFade(0.0f, 0.25f));
            alphaTween.OnComplete(() => {
                alphaTween = null;
            });
            
            tween?.Kill(true);
            tween=transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).OnComplete(() => {
                tween = null;
            });
        }
        
    }
}