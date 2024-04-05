using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Snake
{
    public partial class GameLogic
    {
        public event EventHandler UpdateIntervalChanged;

        private static Random random = new Random();
        private GameDifficulty difficulty;
        private int updateInterval;

        public bool ObstaclesEnabled { get; set; } = false;

        public Size GameFieldSize { get; set; } = new Size(20, 20);

        public Size SingleFieldSize { get; set; } = new Size(1, 1);

        public Queue<Point> SnakeBodyParts { get; set; } = new Queue<Point>();

        public List<Point> Obstacles { get; set; } = new List<Point>();

        public int Highscore { get; set; }

        public int UpdateInterval
        {
            get => updateInterval;
            private set
            {
                if (updateInterval == value) return;
                updateInterval = value;
                OnUpdateIntervalChanged();
            }
        }

        public WallCollisionOption WallCollision { get; set; }

        public ObstacleOption ObstaclesOption { get; set; }

        public GameDifficulty Difficulty
        {
            get => difficulty;
            set
            {
                if (difficulty != value)
                {
                    difficulty = value;
                    OnDifficultyChanged();
                }
            }
        }

        public GameState CurrentState { get; set; }

        public Point Food { get; set; }

        public Point Obstacle { get; set; }

        public Direction CurrentSnakeDirection { get; private set; } = Direction.Right;

        public Direction NextSnakeDirection { get; set; } = Direction.Right;

        public GameLogic()
        {
            ResetGame();
            CurrentState = GameState.StartScreen;
        }

        public void SnakeHighscore()
        {
            if (CurrentState == GameState.Gameover)
            {
                if (SnakeBodyParts.Count > Highscore)
                {
                    Highscore = SnakeBodyParts.Count - 3;
                }
            }
        }

        public void ResetGame()
        {
            CurrentState = GameState.Running;
            SnakeBodyParts.Clear();
            Obstacles.Clear();
            CurrentSnakeDirection = Direction.Right;
            NextSnakeDirection = Direction.Right;
            SnakeBodyParts.Enqueue(new Point(5, 5));
            SnakeBodyParts.Enqueue(new Point(6, 5));
            SnakeBodyParts.Enqueue(new Point(7, 5));
            GenerateFood();
        }

        public void GenerateFood()
        {
            bool validPosition = false;
            while (!validPosition)
            {
                Food = new Point(random.Next(0, GameFieldSize.Width), random.Next(0, GameFieldSize.Height));
                validPosition = !SnakeBodyParts.Contains(Food);
            }
        }

        public void GenerateObstacle()
        {
            bool validPosition = false;
            Point obstacle;
            do
            {
                obstacle = new Point(random.Next(0, GameFieldSize.Width), random.Next(0, GameFieldSize.Height));
                validPosition = !SnakeBodyParts.Contains(obstacle) && !Obstacles.Contains(obstacle);
            } while (!validPosition);

            Obstacles.Add(obstacle);
        }

        public void Update()
        {
            CurrentSnakeDirection = NextSnakeDirection;
            if (CurrentState != GameState.Running)
                return;

            var lastItem = SnakeBodyParts.Last();
            Point newHead = new Point(lastItem.X, lastItem.Y);
            switch (CurrentSnakeDirection)
            {
                case Direction.Up:
                    newHead.Y--;
                    break;
                case Direction.Down:
                    newHead.Y++;
                    break;
                case Direction.Left:
                    newHead.X--;
                    break;
                case Direction.Right:
                    newHead.X++;
                    break;
            }

            if (SnakeBodyCollision(newHead))
            {
                CurrentState = GameState.Gameover;
                SnakeHighscore();
                return;
            }

            if (OutsideGameFieldRange(newHead))
            {
                if (WallCollision == WallCollisionOption.WallsOff)
                {
                    newHead = WallsOffPosition(newHead);
                }
                else
                {
                    CurrentState = GameState.Gameover;
                    SnakeHighscore();
                    return;
                }
            }

            SnakeBodyParts.Enqueue(newHead);
            if (SnakeBodyParts.Contains(Food))
            {
                GenerateFood();
                if (Difficulty == GameDifficulty.Nightmare)
                {
                    UpdateInterval -= 10;
                }

                if (ObstaclesEnabled && SnakeBodyParts.Count % 5 == 0)
                {
                    GenerateObstacle();
                }
            }
            else if (SnakeBodyParts.Count > 1)
            {
                SnakeBodyParts.Dequeue();
            }
        }

        private Point WallsOffPosition(Point position)
        {
            int x = position.X;
            int y = position.Y;

            if (x < 0)
                x = GameFieldSize.Width - 1;
            else if (x >= GameFieldSize.Width)
                x = 0;

            if (y < 0)
                y = GameFieldSize.Height - 1;
            else if (y >= GameFieldSize.Height)
                y = 0;

            return new Point(x, y);
        }

        private bool OutsideGameFieldRange(Point position)
        {
            return position.X < 0 || position.X >= GameFieldSize.Width || position.Y < 0 || position.Y >= GameFieldSize.Height;
        }

        private bool SnakeBodyCollision(Point position)
        {
            foreach (var bodyPart in SnakeBodyParts)
            {
                if (bodyPart == position)
                    return true;
            }

            foreach (var obstacle in Obstacles)
            {
                if (obstacle == position)
                    return true;
            }

            return false;
        }

        internal void EscapeMenu()
        {
            CurrentState = GameState.StartScreen;
        }

        private void OnDifficultyChanged()
        {
            switch (difficulty)
            {
                case GameDifficulty.Easy:
                    UpdateInterval = 200;
                    break;
                case GameDifficulty.Medium:
                    UpdateInterval = 150;
                    break;
                case GameDifficulty.Hard:
                    UpdateInterval = 100;
                    break;
                case GameDifficulty.Nightmare:
                    UpdateInterval = 100;
                    break;
            }
        }

        protected virtual void OnUpdateIntervalChanged()
        {
            UpdateIntervalChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
