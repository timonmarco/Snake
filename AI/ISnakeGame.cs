namespace Snake.AI
{
    public interface ISnakeGame
    {
        SnakeGameState GameState { get; }
        int Height { get; }
        int Width { get; }

        void SetDirection(Direction direction);
    }
}