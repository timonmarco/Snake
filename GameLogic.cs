using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Snake
{
    public partial class GameLogic
    {
        public event EventHandler UpdateIntervalChanged;
        public event EventHandler UpdateSnakeLivescore;

        private const string HighscoreFilePath = "highscore.json";
        private static Random random = new Random();
        private GameDifficulty difficulty;
        private int updateInterval;
        private SoundManager eatSoundManager;
        private SoundManager gameOverSoundManager;
        private SoundManager menuSoundManager;
        private SoundManager MenuButtonSoundManager;
        private SoundManager GameStartSoundManager;
        private SoundManager GameRunningSoundManager;
        public bool ObstaclesEnabled { get; set; } = false;
        public bool SoundEnabled { get; set; } = false;

        public Size GameFieldSize { get; set; } = new Size(20, 20);

        public Size SingleFieldSize { get; set; } = new Size(1, 1);

        public Queue<Point> SnakeBodyParts { get; set; } = new Queue<Point>();

        public List<Point> Obstacles { get; set; } = new List<Point>();

        public int Highscore { get; set; }

        public int Livescore { get; set; }

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

        public bool WallCollisionEnabled { get; set; } = false;

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
            LoadHighscore();
            ResetGame();
            CurrentState = GameState.StartScreen;
            eatSoundManager = new SoundManager("C:\\Users\\paetkau\\source\\repos\\Snake\\Resources\\AppleCrunch.mp3");
            gameOverSoundManager = new SoundManager("C:\\Users\\paetkau\\source\\repos\\Snake\\Resources\\GameOver.mp3");
            menuSoundManager = new SoundManager("C:\\Users\\paetkau\\source\\repos\\Snake\\Resources\\MenuTheme.mp3");
            MenuButtonSoundManager = new SoundManager("C:\\Users\\paetkau\\source\\repos\\Snake\\Resources\\MenuButton.mp3");
            GameStartSoundManager = new SoundManager("C:\\Users\\paetkau\\source\\repos\\Snake\\Resources\\GameStartEnter.mp3");
            GameRunningSoundManager = new SoundManager("C:\\Users\\paetkau\\source\\repos\\Snake\\Resources\\GameRunning.mp3");
        }


        public void PlayEatSound()
        {
            if (SoundEnabled)
                eatSoundManager.Play();
        }

        public void PlayGameOverSound()
        {
            if (SoundEnabled)
                gameOverSoundManager.Play();
        }

        public void PlayMenuSound()
        {
            if (SoundEnabled)
                menuSoundManager.Play();
        }

        public void PlayMenuButtonSound()
        {
            if (SoundEnabled)
                MenuButtonSoundManager.Play();
        }

        public void SnakeHighscore()
        {
            if (CurrentState == GameState.Gameover)
            {
                if (SnakeBodyParts.Count > Highscore)
                {
                    Highscore = SnakeBodyParts.Count - 3;
                    SaveHighscore();
                }
            }
        }

        public void SnakeLivescore()
        {
            if (CurrentState == GameState.Running)
            {
                Livescore = SnakeBodyParts.Count - 3;
                OnUpdateSnakeLivescore();
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
                validPosition = !SnakeBodyParts.Contains(Food) && !Obstacles.Contains(Food);
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

            if (CurrentState == GameState.Running)
            {
                SnakeLivescore();
            }

            if (SnakeBodyCollision(newHead))
            {
                CurrentState = GameState.Gameover;
                SnakeHighscore();
                PlayGameOverSound();
                return;
            }

            if (OutsideGameFieldRange(newHead))
            {
                if (!WallCollisionEnabled)
                {
                    newHead = WallsOffPosition(newHead);
                }
                else
                {
                    CurrentState = GameState.Gameover;
                    PlayGameOverSound();
                    SnakeHighscore();
                    return;
                }
            }

            SnakeBodyParts.Enqueue(newHead);
            if (SnakeBodyParts.Contains(Food))
            {
                GenerateFood();
                PlayEatSound();
                if (Difficulty == GameDifficulty.Nightmare)
                {
                    UpdateInterval -= 2;
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

        private void LoadHighscore()
        {
            if (File.Exists(HighscoreFilePath))
            {
                var json = File.ReadAllText(HighscoreFilePath);
                var highscoreData = JsonConvert.DeserializeObject<HighscoreData>(json);
                Highscore = highscoreData.Highscore;
            }
        }

        private void SaveHighscore()
        {
            var highscoreData = new HighscoreData() { Highscore = Highscore };
            var json = JsonConvert.SerializeObject(highscoreData);
            File.WriteAllText(HighscoreFilePath, json);
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

        protected virtual void OnUpdateSnakeLivescore()
        {
            UpdateSnakeLivescore?.Invoke(this, EventArgs.Empty);
        }
    }
}
