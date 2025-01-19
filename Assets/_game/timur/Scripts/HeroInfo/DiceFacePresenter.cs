namespace _game.HeroInfo
{
    public class DiceFacePresenter
    {
        private IDiceFaceView _view;
        private DiceFaceModel _diceFaceModel;
        public DiceFacePresenter(IDiceFaceView view, DiceFaceModel model)
        {
            _diceFaceModel = model;
            _view = view;
            _view.SetImage(model.image);
            _view.SetColor(model.colorValue);
        }
    }
}