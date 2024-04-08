using Snake.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Snake
{
    public partial class GameDrawingControl : UserControl
    {
        private CoordinationSystemHelper coordHelper;
        private GameLogic gameLogic;

        private Bitmap apple;
        private Bitmap snakehead;
        private Bitmap snakebody;
        private Bitmap poopObstacle;
        private Bitmap TopToLeftRightToTop;
        private Bitmap TopToRightLeftToTop;
        private Bitmap BottomToRightLeftToBottom;
        private Bitmap BottomToLeftRightToBottom;

        private Font gameOverFont = new Font("Neon Pixel-7", 140f, FontStyle.Bold);
        private Font restartFont = new Font("Nintendo DS BIOS", 30f, FontStyle.Bold);
        private Font StartGameHeaderFont = new Font("Neon Pixel-7", 190f, FontStyle.Bold);

        private Brush menuBrush = Brushes.Purple;
        private Brush startMenuBrush = Brushes.Maroon;
        private Brush gameOverBackgroundBrush = new SolidBrush(Color.FromArgb(100, Color.White));

        public IMenu Menu { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GameLogic GameLogic
        {
            get => gameLogic;
            set
            {
                if (gameLogic == value) return;
                gameLogic = value;
                OnGameLogicChanged();
            }
        }

        public GameDrawingControl()
        {
            InitializeComponent();
            apple = Resources.apfel;
            snakehead = Resources.snakehead;
            snakebody = Resources.snakebody;
            poopObstacle = Resources.poop;
            TopToLeftRightToTop = Resources.TopToLeftRightToTop;

            coordHelper = new CoordinationSystemHelper(ClientSize, new Size(20, 20));
        }

        private void OnGameLogicChanged()
        {
            coordHelper.GameFieldSize = GameLogic.GameFieldSize;
        }

        public void SelectNextMenuItem()
        {
            Menu.SelectNextMenuItem();
            Invalidate();
        }

        public void SelectPreviousMenuItem()
        {
            Menu.SelectPreviousMenuItem();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
                return;

            e.Graphics.Clear(BackColor);
            DrawGrid(e.Graphics);
            DrawBodyParts(e.Graphics);
            DrawFood(e.Graphics);
            DrawObstacle(e.Graphics);
            if (GameLogic.CurrentState == GameState.Gameover)
                DrawGameOver(e.Graphics);
            if (GameLogic.CurrentState == GameState.StartScreen)
            {
                DrawStart(e.Graphics);
                DrawMenu(e.Graphics);
            }
        }

        private void DrawGameOver(Graphics g)
        {
            g.FillRectangle(gameOverBackgroundBrush, 0, 0, ClientSize.Width, ClientSize.Height);
            var screenCenter = new PointF(ClientSize.Width / 2, ClientSize.Height / 2);
            var textSize = DrawTextCenter(g, "GAME OVER", gameOverFont, Brushes.Maroon, screenCenter);
            var textPoint = new PointF(screenCenter.X, screenCenter.Y + textSize.Height + 10f);

            textSize = DrawTextCenter(g, $"Highscore: {GameLogic.Highscore}", restartFont, Brushes.Black, textPoint);
            textPoint = new PointF(textPoint.X, textPoint.Y + textSize.Height + 10f);

            textSize = DrawTextCenter(g, "Press ENTER to restart.", restartFont, Brushes.Black, textPoint);
            textPoint = new PointF(textPoint.X, textPoint.Y + textSize.Height + 10f);

            textSize = DrawTextCenter(g, "Press ESCAPE to go to Startscreen.", restartFont, Brushes.Black, textPoint);
            textPoint = new PointF(textPoint.X, textPoint.Y + textSize.Height);
        }

        //private Color GetRandomColor(int color)
        //{
        //    RGB rgbColor = ColorHelper.ColorConverter
        //}

        private void DrawMenu(Graphics g)
        {
            var textPoint = 650f;
            foreach (var item in Menu.MenuItems)
            {
                var text = $"{item.Name}: {item.Value}";
                Brush brush = menuBrush;

                if (item == Menu.SelectedMenuItem)
                {
                    brush = Brushes.Maroon;
                }
                var textSize = DrawTextCenter(g, text, restartFont, brush, new PointF(ClientSize.Width / 2, textPoint));
                textPoint += textSize.Height + 20;
            }
        }
        private void DrawStart(Graphics g)
        {
            g.FillRectangle(gameOverBackgroundBrush, 0, 0, ClientSize.Width, ClientSize.Height);
            var screenCenter = new PointF(ClientSize.Width / 2, ClientSize.Height / 2 - 100);
            var textSize = DrawTextCenter(g, "SNAKE", StartGameHeaderFont, menuBrush, screenCenter);
            var textPoint = new PointF(screenCenter.X, screenCenter.Y + 150f);


            textSize = DrawTextCenter(g, $"Highscore: {GameLogic.Highscore}", restartFont, startMenuBrush, textPoint);
            textPoint = new PointF(textPoint.X, textPoint.Y + textSize.Height + 10f);

            textSize = DrawTextCenter(g, "Press ENTER to Start.", restartFont, startMenuBrush, textPoint);
        }

        private SizeF DrawTextCenter(Graphics g, string text, Font font, Brush brush, PointF point)
        {
            var textSize = g.MeasureString(text, font);
            var textPoint = new PointF(point.X - textSize.Width / 2, point.Y - textSize.Height / 2);
            g.DrawString(text, font, brush, textPoint);
            return textSize;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do Nothing
        }

        private void DrawGrid(Graphics g)
        {
            bool isChess = false;

            for (int row = 0; row < GameLogic.GameFieldSize.Height; row++)
            {
                for (int col = 0; col < GameLogic.GameFieldSize.Width; col++)
                {
                    PointF cellStart = coordHelper.ToDrawingPoint(new Point(col, row));
                    SizeF cellSize = coordHelper.ToDrawingSize(new Size(1, 1));

                    if (isChess)
                    {
                        g.FillRectangle(Brushes.PaleGreen, cellStart.X, cellStart.Y, cellSize.Width, cellSize.Height);
                    }
                    else
                    {
                        g.FillRectangle(Brushes.LightGreen, cellStart.X, cellStart.Y, cellSize.Width, cellSize.Height);
                    }

                    isChess = !isChess;
                }

                if (GameLogic.GameFieldSize.Width % 2 == 0)
                {
                    isChess = !isChess;
                }
            }
        }

        private void DrawFood(Graphics g)
        {
            PointF foodPosition = coordHelper.ToDrawingPoint(GameLogic.Food);

            SizeF foodSize = coordHelper.ToDrawingSize(new Size(1, 1));
            g.DrawImage(apple, foodPosition.X, foodPosition.Y, foodSize.Width, foodSize.Height);
        }

        private void DrawObstacle(Graphics g)
        {
            foreach (Point obstacle in GameLogic.Obstacles)
            {
                PointF obstaclePosition = coordHelper.ToDrawingPoint(obstacle);
                SizeF obstacleSize = coordHelper.ToDrawingSize(new Size(1, 1));
                g.DrawImage(poopObstacle, obstaclePosition.X, obstaclePosition.Y, obstacleSize.Width, obstacleSize.Height);
            }
        }

        private void DrawBodyParts(Graphics g)
        {
            int lastThreeSnakeElements = 0;

            foreach (Point part in GameLogic.SnakeBodyParts)
            {

                RectangleF rectS = coordHelper.ToDrawingRectangle(new Rectangle(part, new Size(1, 1)));

                if (lastThreeSnakeElements < 3)
                {
                    rectS.Inflate(new SizeF(coordHelper.FieldSize.Width / 30f, -coordHelper.FieldSize.Height / 20f));
                }

                RectangleF rect = coordHelper.ToDrawingRectangle(new Rectangle(part, new Size(1, 1)));
                rect.Inflate(new SizeF(coordHelper.FieldSize.Width / 10f, coordHelper.FieldSize.Height / 10f));
                if (part == GameLogic.SnakeBodyParts.Last())
                {
                    float angle = 0f;
                    switch (GameLogic.CurrentSnakeDirection)
                    {
                        case Direction.Up:
                            angle = 270f;
                            break;
                        case Direction.Down:
                            angle = 90f;
                            break;
                        case Direction.Left:
                            angle = 180f;
                            break;
                        case Direction.Right:
                            angle = 0f;
                            break;
                    }
                    DrawRotatedImage(g, snakehead, rect, angle);
                }
                else
                {
                    int currentIndex = GameLogic.SnakeBodyParts.ToList().IndexOf(part);
                    Point nextPart = GameLogic.SnakeBodyParts.ElementAt(currentIndex + 1);
                    float angle = CalculateAngle(part, nextPart);
                    DrawRotatedImage(g, snakebody, rectS, angle);
                }
            }
        }

        private void DrawRotatedImage(Graphics g, Bitmap image, RectangleF rect, float angle)
        {
            using (Matrix rotationMatrix = new Matrix())
            {
                rotationMatrix.RotateAt(angle, new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2));
                g.Transform = rotationMatrix;
                g.DrawImage(image, rect);
                g.ResetTransform();
            }
        }

        private float CalculateAngle(Point currentPart, Point nextPart)
        {
            if (nextPart.X == currentPart.X + 1)
                return 0f;
            else if (nextPart.X == currentPart.X - 1)
                return 180f;
            else if (nextPart.Y == currentPart.Y + 1)
                return 90f;
            else
                return 270f;
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            coordHelper.DisplaySize = ClientSize;
            Invalidate();
        }
    }
}
