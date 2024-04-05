using System;

namespace Snake
{
    public class Menu : IMenu
    {
        private int selectedMenuItemIndex;
        public MenuItem DifficultyMenuItem { get; set; }
        public MenuItem ObstaclesMenuItem { get; set; }
        public MenuItem WallsMenuItem { get; set; }
        public MenuItem SoundMenuItem { get; set; }

        public event EventHandler<MenuItemChangedEventArgs> MenuItemChanged;

        public MenuItem[] MenuItems { get; }

        public MenuItem SelectedMenuItem => MenuItems[selectedMenuItemIndex];

        public GameDifficulty GameDifficulty => (GameDifficulty)DifficultyMenuItem.CurrentOptionIndex;
        public WallCollisionOption WallCollisionOption => (WallCollisionOption)WallsMenuItem.CurrentOptionIndex;
        public ObstacleOption ObstacleOption => (ObstacleOption)ObstaclesMenuItem.CurrentOptionIndex;
        public SoundOption SoundOption => (SoundOption)SoundMenuItem.CurrentOptionIndex;

        public bool Obstacles => ObstaclesMenuItem.CurrentOptionIndex == 0;

        public Menu()
        {
            DifficultyMenuItem = new MenuItem("Difficulty", "Easy", "Medium", "Hard", "Nightmare");
            ObstaclesMenuItem = new MenuItem("Obstacles", "On", "Off");
            WallsMenuItem = new MenuItem("Walls", "On", "Off");
            SoundMenuItem = new MenuItem("Sound", "On", "Off");
            MenuItems = new MenuItem[]
            {
            DifficultyMenuItem,
            ObstaclesMenuItem,
            WallsMenuItem,
            SoundMenuItem
            };
        }

        public void SelectNextMenuItem()
        {
            selectedMenuItemIndex = (selectedMenuItemIndex + 1) % MenuItems.Length;
        }

        public void SelectPreviousMenuItem()
        {
            selectedMenuItemIndex = (selectedMenuItemIndex == 0) ? MenuItems.Length - 1 : selectedMenuItemIndex - 1;
        }

        public void SelectNextOption()
        {
            SelectedMenuItem.SelectNextOption();
            OnMenuItemChanged(SelectedMenuItem.Name);
        }

        public void SelectPreviousOption()
        {
            SelectedMenuItem.SelectPreviousOption();
            OnMenuItemChanged(SelectedMenuItem.Name);
        }

        protected virtual void OnMenuItemChanged(string menuItemName)
        {
            MenuItemChanged?.Invoke(this, new MenuItemChangedEventArgs { ItemName = menuItemName });
        }
    }
}
