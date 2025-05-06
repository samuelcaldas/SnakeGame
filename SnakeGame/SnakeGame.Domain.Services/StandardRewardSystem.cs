using SnakeGame.Application;
using SnakeGame.Domain.Entities;

namespace SnakeGame.Domain.Services
{
    /// <summary>
    /// Standard implementation of reward system for reinforcement learning
    /// </summary>
    public class StandardRewardSystem : IRewardSystem
    {
        private readonly float _moveTowardsFoodReward;
        private readonly float _moveAwayFromFoodPenalty;
        private readonly float _eatFoodReward;
        private readonly float _deathPenalty;
        private readonly float _survivalReward;

        public StandardRewardSystem(
            float moveTowardsFoodReward = 0.1f,
            float moveAwayFromFoodPenalty = -0.1f,
            float eatFoodReward = 1.0f,
            float deathPenalty = -1.0f,
            float survivalReward = 0.01f)
        {
            _moveTowardsFoodReward = moveTowardsFoodReward;
            _moveAwayFromFoodPenalty = moveAwayFromFoodPenalty;
            _eatFoodReward = eatFoodReward;
            _deathPenalty = deathPenalty;
            _survivalReward = survivalReward;
        }

        public float CalculateReward(GameState oldState, GameState newState, Direction action)
        {
            // Check if the game is over (snake hit wall or itself)
            if (newState.IsGameOver)
            {
                return _deathPenalty;
            }

            // Check if food was eaten
            if (newState.Score > oldState.Score)
            {
                return _eatFoodReward;
            }

            // Calculate distance to food before and after the move
            double oldDistance = oldState.Snake.HeadPosition.DistanceTo(oldState.Food.Position);
            double newDistance = newState.Snake.HeadPosition.DistanceTo(newState.Food.Position);

            // Reward for moving closer to food, penalize for moving away
            if (newDistance < oldDistance)
            {
                return _moveTowardsFoodReward;
            }
            
            if (newDistance > oldDistance)
            {
                return _moveAwayFromFoodPenalty;
            }

            // Small reward for staying alive
            return _survivalReward;
        }
    }
}