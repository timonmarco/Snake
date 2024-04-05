namespace Snake
{
    public interface IMenu
    {
        MenuItem[] MenuItems { get; }
        MenuItem SelectedMenuItem { get; }
        void SelectNextMenuItem();
        void SelectPreviousMenuItem();
    }
}