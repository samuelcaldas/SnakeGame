using SnakeGame.Application;

namespace SnakeGame.Domain.Services
{
    /// <summary>
    /// Interface for game rules implementation
    /// </summary>
    public interface IGameRules
    {
        bool IsGameOver(GameState state);
        bool IsFoodEaten(GameState state);
    }
}