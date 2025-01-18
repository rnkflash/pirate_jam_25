using System;

namespace _game
{
    public class HeroPanelPresenter
    {
        private IHeroPanelView _view;
        public HeroHealthPresenter heroHealthPresenter { get; }

        public HeroPanelPresenter(IHeroPanelView view, HeroPanelModel model)
        {
            _view = view;
            _view.SetName(model.name);
            heroHealthPresenter = new HeroHealthPresenter(_view.GetHeroHealthView(), model.health);
        }
    }
}