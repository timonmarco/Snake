namespace Snake
{
    public class MenuItem
    {
        public string Name { get; set; }
        public string[] Options { get; set; }
        public int CurrentOptionIndex { get; set; }

        public string Value => Options[CurrentOptionIndex];

        public MenuItem(string name, params string[] options)
        {
            Name = name;
            Options = options;
            CurrentOptionIndex = 0;
        }

        public void SelectNextOption()
        {
            CurrentOptionIndex = (CurrentOptionIndex + 1) % Options.Length;
        }

        public void SelectPreviousOption()
        {
            CurrentOptionIndex = CurrentOptionIndex == 0 ? Options.Length - 1 : CurrentOptionIndex - 1;
        }
    }
}
