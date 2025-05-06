using SnakeGame.Domain.Entities;

namespace SnakeGame.Application
{
    /// <summary>
    /// Interface for game agents (human or machine)
    /// </summary>
    public interface IAgent
    {
        Direction GetAction(GameState state);
    }
}