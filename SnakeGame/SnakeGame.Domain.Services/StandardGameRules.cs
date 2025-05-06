using SnakeGame.Application;

namespace SnakeGame.Domain.Services
{
    /// <summary>
    /// Standard implementation of game rules
    /// </summary>
    public class StandardGameRules : IGameRules
    {
        public bool IsGameOver(GameState state)
        {
            return state.Snake.CollidesWithSelf() || 
                   state.Snake.CollidesWithWall(state.BoardWidth, state.BoardHeight);
        }

        public bool IsFoodEaten(GameState state)
        {
            return state.Snake.HeadPosition.Equals(state.Food.Position);
        }
    }
}