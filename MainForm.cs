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
        }

        private void GameLogic_UpdateIntervalChanged(object sender, EventArgs e)
        {
            timer.Interval = GameLogic.UpdateInterval;
            Text = $"Snake {GameLogic.Highscore}";
        }

        private void GameMenu_MenuItemChanged(object sender, MenuItemChangedEventArgs e)
        {
            if (e.ItemName == GameMenu.WallsMenuItem.Name)
            {
                GameLogic.WallCollision = (WallCollisionOption)GameMenu.WallsMenuItem.CurrentOptionIndex;
            }
            if (e.ItemName == GameMenu.DifficultyMenuItem.Name)
            {
                GameLogic.Difficulty = GameMenu.GameDifficulty;
            }
            if (e.ItemName == GameMenu.ObstaclesMenuItem.Name)
            {
                GameLogic.ObstaclesEnabled = (ObstacleOption)GameMenu.ObstaclesMenuItem.CurrentOptionIndex == 0;
            }
            gameDrawingControl1.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
