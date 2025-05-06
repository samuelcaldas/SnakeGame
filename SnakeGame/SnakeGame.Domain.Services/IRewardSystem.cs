using SnakeGame.Application;
using SnakeGame.Domain.Entities;

namespace SnakeGame.Domain.Services
{
    /// <summary>
    /// Interface for reward system implementation
    /// </summary>
    public interface IRewardSystem
    {
        float CalculateReward(GameState oldState, GameState newState, Direction action);
    }
}