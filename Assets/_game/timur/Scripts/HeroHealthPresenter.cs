namespace _game
{
    public class HeroHealthPresenter
    {
        private IHeroHealthView _view;
        private ReactiveVar<int> _health;
        public HeroHealthPresenter(IHeroHealthView view, ReactiveVar<int> health)
        {
            _view = view;
            _view.HealthChanged(health.Value);
            _health = health;
            _health.Subscribe(OnHealthChanged);
        }

        private void OnHealthChanged(int health)
        {
            _view.HealthChanged(health);
        }

        public void Release()
        {
            _health.Unsubscribe(OnHealthChanged);
        }
    }
}