using System;
using System.Collections.Generic;
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

        private void Awake()
        {
            _originalPosition = gameObject.transform.position;
        }


        private Vector3 _originalPosition;
        public void SetWeaponName(string name)
        {
            _weaponNameText.text = name;
        }

        public void SetWeaponImage(Sprite image)
        {
            _weaponImage.sprite = image;
        }

        public void SetDiceFacesView(DiceList diceFaces)
        {
            for (int i = 0; i < diceFaces.diceFaces.Count; i++)
            {
                _diceFaces[i].SetImage(diceFaces.diceFaces[i].image);
                _diceFaces[i].SetColor(diceFaces.diceFaces[i].colorValue);
            }
        }

        public void SetBodyImage(Image image)
        {
            
        }

        public void SetBodyName(string name)
        {
            _bodyNameText.text = name;
        }

        public void SetShowState(bool state)
        {
            Debug.Log("timur STATE " + state);
            if (state)
            {
                // Опустить объект на 30 единиц по оси Y от исходной позиции
                Vector3 loweredPosition = _originalPosition + new Vector3(0f, -30f, 0f);
                gameObject.transform.position = loweredPosition;

                // Настроить CanvasGroup до запуска анимации
                _canvasGroup.interactable = true;
                _canvasGroup.alpha = 1f;

                // Поднять объект обратно до исходной позиции по кривой
                gameObject.transform.DOMoveY(_originalPosition.y, 0.18f)
                    .SetEase(Ease.OutBack)
                    .OnStart(() =>
                    {
                        Debug.Log("Tween started (state true)");
                    })
                    .OnComplete(() =>
                    {
                        Debug.Log("Tween completed (state true)");
                        // Восстановить точную исходную позицию
                        gameObject.transform.position = _originalPosition;
                    });
            }
            else
            {
                // Вычислить цель для опускания: на 30 единиц ниже текущей позиции
                float targetY = gameObject.transform.position.y - 30f;

                // Запустить анимацию опускания объекта
                gameObject.transform.DOMoveY(targetY, 0.11f)
                    .SetEase(Ease.OutBounce)
                    .OnStart(() =>
                    {
                        Debug.Log("Tween started (state false)");
                    })
                    .OnComplete(() =>
                    {
                        Debug.Log("Tween completed (state false)");
                        // Отключить взаимодействие и скрыть объект
                        _canvasGroup.interactable = false;
                        _canvasGroup.alpha = 0f;
                        // Восстановить исходную позицию после завершения анимации
                        gameObject.transform.position = _originalPosition;
                    });
            }
        }


        public List<DiceFaceView> GetDiceFaces() => _diceFaces;
    }

    public interface IInfoView
    {
        void SetWeaponName(string name);
        void SetWeaponImage(Sprite image);
        void SetDiceFacesView(DiceList diceFaces);
        void SetBodyImage(Image image);
        void SetBodyName(string name);

        void SetShowState(bool state);

        List<DiceFaceView> GetDiceFaces();
    }
}