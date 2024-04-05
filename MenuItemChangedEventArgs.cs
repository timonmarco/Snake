using System;

namespace Snake
{
    public class MenuItemChangedEventArgs : EventArgs
    {
        public string ItemName { get; set; }
    }
}
