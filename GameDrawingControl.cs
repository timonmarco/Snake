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
        private Bitmap snakeHead;
        private Bitmap snakeBody;
        private Bitmap blockObstacle;
        private Bitmap snakeBodyCounterClockwise;
        private Bitmap snakeBodyClockwise;

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
            snakeHead = Resources.snakehead;
            snakeBody = Resources.snakebody;
            blockObstacle = Resources.block;
            snakeBodyCounterClockwise = Resources.CounterClockwise;
            snakeBodyClockwise = Resources.Clockwise;
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
            DrawObstacle(e.Graphics);
            if (GameLogic.CurrentState == GameState.Running)
            {
                DrawFood(e.Graphics);
            }
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
                g.DrawImage(blockObstacle, obstaclePosition.X, obstaclePosition.Y, obstacleSize.Width, obstacleSize.Height);
            }
        }

        private void DrawBodyParts(Graphics g)
        {
            int lastThreeSnakeElements = 0;
            var bodyParts = GameLogic.SnakeBodyParts.ToArray();
            for (int i = 0; i < bodyParts.Length; i++)
            {
                var part = bodyParts[i];
                var previousPart = i > 0 ? bodyParts[i - 1] : part;
                var nextPart = i < bodyParts.Length - 1 ? bodyParts[i + 1] : part;

                RectangleF rectS = coordHelper.ToDrawingRectangle(new Rectangle(part, new Size(1, 1)));
                if (lastThreeSnakeElements < 3)
                {
                    rectS.Inflate(new SizeF(coordHelper.FieldSize.Width / 20f, coordHelper.FieldSize.Height / 20f));
                }
                RectangleF rect = coordHelper.ToDrawingRectangle(new Rectangle(part, new Size(1, 1)));
                rect.Inflate(new SizeF(coordHelper.FieldSize.Width / 10f, coordHelper.FieldSize.Height / 10f));

                if (part == GameLogic.SnakeBodyParts.Last())
                {
                    float angle = CalculateSnakeheadAngle();
                    DrawRotatedImage(g, snakeHead, rect, angle);
                }
                else
                {
                    var (Image, Angle) = GetCurveSnakeBody(previousPart, part, nextPart);
                    DrawRotatedImage(g, Image, rectS, Angle);
                }
            }
        }
        private (Bitmap Image, float Angle) GetCurveSnakeBody(Point previousPart, Point part, Point nextPart)
        {
            var directionPartial = GetDirectionFromPoints(previousPart, part);
            var directionTotal = GetDirectionFromPoints(previousPart, nextPart);

            switch (directionTotal)
            {
                case Direction.Up:
                    return (snakeBody, 270f);
                case Direction.Down:
                    return (snakeBody, 90f);
                case Direction.Left:
                    return (snakeBody, 180f);
                case Direction.Right:
                    return (snakeBody, 0f);
                case Direction.UpLeft:
                    return directionPartial == Direction.Up
                        ? (snakeBodyCounterClockwise, 90f)
                        : (snakeBodyClockwise, 180f);
                case Direction.UpRight:
                    return directionPartial == Direction.Up
                        ? (snakeBodyClockwise, 270f)
                        : (snakeBodyCounterClockwise, 180f);
                case Direction.DownLeft:
                    return directionPartial == Direction.Down
                        ? (snakeBodyClockwise, 90f)
                        : (snakeBodyCounterClockwise, 0f);
                case Direction.DownRight:
                    return directionPartial == Direction.Down
                        ? (snakeBodyCounterClockwise, 270f)
                        : (snakeBodyClockwise, 0f);
                default:
                    return (snakeBody, 0f);
            }
        }

        private Direction GetDirectionFromPoints(Point a, Point b)
        {
            if (a.X == b.X && a.Y < b.Y)
                return Direction.Down;
            else if (a.X == b.X && a.Y > b.Y)
                return Direction.Up;
            else if (a.X < b.X && a.Y == b.Y)
                return Direction.Right;
            else if (a.X > b.X && a.Y == b.Y)
                return Direction.Left;
            else if (a.X > b.X && a.Y > b.Y)
                return Direction.UpLeft;
            else if (a.X < b.X && a.Y > b.Y)
                return Direction.UpRight;
            else if (a.X > b.X && a.Y < b.Y)
                return Direction.DownLeft;
            return Direction.DownRight;
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

        private float CalculateSnakeheadAngle()
        {
            Direction currentDirection = GameLogic.CurrentSnakeDirection;
            float angle = 0f;

            switch (currentDirection)
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
            return angle;
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            coordHelper.DisplaySize = ClientSize;
            Invalidate();
        }
    }
}
