namespace _game.rnk.Scripts.battleSystem
{
    public interface ISelectable
    {
        public bool IsSelected();
        public void SetSelected(bool newValue);
    }
}