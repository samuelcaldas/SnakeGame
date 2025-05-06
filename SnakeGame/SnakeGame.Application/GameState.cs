using SnakeGame.Domain.Entities;

namespace SnakeGame.Application
{
    /// <summary>
    /// Represents the current state of the game
    /// </summary>
    public class GameState
    {
        public Snake Snake { get; }
        public Food Food { get; }
        public int BoardWidth { get; }
        public int BoardHeight { get; }
        public int Score { get; private set; }
        public bool IsGameOver { get; set; }

        public GameState(Snake snake, Food food, int boardWidth, int boardHeight, int score = 0, bool isGameOver = false)
        {
            Snake = snake;
            Food = food;
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            Score = score;
            IsGameOver = isGameOver;
        }

        public void IncrementScore()
        {
            Score++;
        }

        public GameState Clone()
        {
            return new GameState(
                Snake.Clone(),
                Food.Clone(),
                BoardWidth,
                BoardHeight,
                Score,
                IsGameOver
            );
        }
    }
}