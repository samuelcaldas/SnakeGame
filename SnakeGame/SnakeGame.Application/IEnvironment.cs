using SnakeGame.Domain.Entities;
using System.Collections.Generic;

namespace SnakeGame.Application
{
    /// <summary>
    /// Interface for the game environment
    /// </summary>
    public interface IEnvironment
    {
        GameState Reset();
        (GameState nextState, float reward, bool done) Step(Direction action);
        GameState GetState();
        IReadOnlyList<Direction> GetValidActions();
    }
}