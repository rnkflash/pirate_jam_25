using UnityEngine;

namespace _game
{
    public class HeroHealthView : MonoBehaviour, IHeroHealthView
    {
        [SerializeField] private GameObject _healthObject;
        [SerializeField] private Transform _healthTransform;
        
        public void HealthChanged(int health)
        {
            Debug.Log($"timur {_healthTransform.childCount} {health}");
            

            // Удаляем лишние объекты
            for (int i = _healthTransform.childCount - 1; i >= health; i--)
            {
                DestroyImmediate(_healthTransform.GetChild(i).gameObject);
            }

            // Создаем недостающие объекты
            for (int i = _healthTransform.childCount; i < health; i++)
            {
                Instantiate(_healthObject, _healthTransform);
            }
        }
    }

    public interface IHeroHealthView
    {
        void HealthChanged(int health);
    }
}