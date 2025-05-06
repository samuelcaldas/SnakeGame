using SnakeGame.Domain.Entities;
using System;
using System.Linq;

namespace SnakeGame.Application
{
    /// <summary>
    /// Interface for food placement strategy
    /// </summary>
    public interface IFoodPlacementStrategy
    {
        Food PlaceFood(GameState state);
    }
    
    /// <summary>
    /// Random food placement strategy
    /// </summary>
    public class RandomFoodPlacementStrategy : IFoodPlacementStrategy
    {
        private readonly Random _random;
        
        public RandomFoodPlacementStrategy(int? seed = null)
        {
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
        }
        
        public Food PlaceFood(GameState state)
        {
            // Get all snake body positions
            var snakeBody = state.Snake.GetBody();
            
            // Find an empty position for the food
            Position foodPosition;
            do
            {
                int x = _random.Next(0, state.BoardWidth);
                int y = _random.Next(0, state.BoardHeight);
                foodPosition = new Position(x, y);
            } while (snakeBody.Contains(foodPosition));
            
            return new Food(foodPosition);
        }
    }
}