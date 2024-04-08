using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Snake
{
    public partial class MainForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GameLogic GameLogic { get; } = new GameLogic();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Menu GameMenu { get; } = new Menu();

        public MainForm()
        {
            InitializeComponent();
            GameLogic.UpdateIntervalChanged += GameLogic_UpdateIntervalChanged;
            gameDrawingControl1.GameLogic = GameLogic;
            gameDrawingControl1.Menu = GameMenu;
            GameMenu.MenuItemChanged += GameMenu_MenuItemChanged;
            GameLogic.UpdateSnakeLivescore += GameLogic_UpdateSnakeLivescore;
            SetupGameLogic();
        }

        private void GameLogic_UpdateSnakeLivescore(object sender, EventArgs e)
        {
            Text = $"SNAKE Score: {GameLogic.Livescore}";
        }

        private void SetupGameLogic()
        {
            GameLogic.WallCollisionEnabled = GameMenu.WallCollisionEnabled;
            GameLogic.Difficulty = GameMenu.GameDifficulty;
            GameLogic.ObstaclesEnabled = GameMenu.Obstacles;
        }

        private void GameLogic_UpdateIntervalChanged(object sender, EventArgs e)
        {
            timer.Interval = GameLogic.UpdateInterval;
        }

        private void GameMenu_MenuItemChanged(object sender, MenuItemChangedEventArgs e)
        {
            if (e.ItemName == GameMenu.WallsMenuItem.Name)
            {
                GameLogic.WallCollisionEnabled = GameMenu.WallCollisionEnabled;
            }
            if (e.ItemName == GameMenu.DifficultyMenuItem.Name)
            {
                GameLogic.Difficulty = GameMenu.GameDifficulty;
            }
            if (e.ItemName == GameMenu.ObstaclesMenuItem.Name)
            {
                GameLogic.ObstaclesEnabled = GameMenu.Obstacles;
            }
            //if (e.Itemname == GameMenu.SoundMenuItem.Name)
            //{
            //    GameLogic.
            //}
            gameDrawingControl1.Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                if (GameLogic.CurrentState != GameState.Running)
                {
                    GameLogic.EscapeMenu();
                }
                return true;
            }

            if (keyData == Keys.Enter)
            {
                if (GameLogic.CurrentState != GameState.Running)
                {
                    GameLogic.ResetGame();
                }
                return true;
            }
            if (GameLogic.CurrentState == GameState.Running && ProcessSnakeKeys(keyData))
                return true;

            if (GameLogic.CurrentState == GameState.StartScreen && ProcessMenuKeys(keyData))
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool ProcessSnakeKeys(Keys keyData)
        {
            if (keyData == Keys.Up && GameLogic.CurrentSnakeDirection != Direction.Down)
            {
                GameLogic.NextSnakeDirection = Direction.Up;
                return true;
            }

            if (keyData == Keys.Left && GameLogic.CurrentSnakeDirection != Direction.Right)
            {
                GameLogic.NextSnakeDirection = Direction.Left;
                return true;
            }

            if (keyData == Keys.Right && GameLogic.CurrentSnakeDirection != Direction.Left)
            {
                GameLogic.NextSnakeDirection = Direction.Right;
                return true;
            }

            if (keyData == Keys.Down && GameLogic.CurrentSnakeDirection != Direction.Up)
            {
                GameLogic.NextSnakeDirection = Direction.Down;
                return true;
            }
            return false;
        }

        private bool ProcessMenuKeys(Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                GameMenu.SelectPreviousMenuItem();
                return true;
            }

            if (keyData == Keys.Down)
            {
                GameMenu.SelectNextMenuItem();
                return true;
            }

            if (keyData == Keys.Left)
            {
                GameMenu.SelectPreviousOption();
                return true;
            }

            if (keyData == Keys.Right)
            {
                GameMenu.SelectNextOption();
                return true;
            }
            return false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            GameLogic.Update();
            gameDrawingControl1.Invalidate();
        }
    }
}
