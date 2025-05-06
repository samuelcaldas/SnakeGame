using SnakeGame.Domain.Entities;
using SnakeGame.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Application
{
    /// <summary>
    /// The main game environment, similar to OpenAI Gym environments
    /// </summary>
    public class SnakeEnvironment : IEnvironment
    {
        private GameState _state;
        private readonly IGameRules _gameRules;
        private readonly IRewardSystem _rewardSystem;
        private readonly IFoodPlacementStrategy _foodPlacementStrategy;
        private readonly int _boardWidth;
        private readonly int _boardHeight;
        private readonly Random _random;

        public SnakeEnvironment(
            IGameRules gameRules,
            IRewardSystem rewardSystem,
            IFoodPlacementStrategy foodPlacementStrategy,
            int boardWidth = 20,
            int boardHeight = 15,
            int? seed = null)
        {
            _gameRules = gameRules;
            _rewardSystem = rewardSystem;
            _foodPlacementStrategy = foodPlacementStrategy;
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
            
            // Initialize with a dummy state that will be replaced in Reset()
            _state = CreateInitialState();
        }

        public GameState Reset()
        {
            _state = CreateInitialState();
            return _state.Clone();
        }

        public (GameState nextState, float reward, bool done) Step(Direction action)
        {
            var oldState = _state.Clone();
            
            // Move the snake
            _state.Snake.Move(action);
            
            // Check if the game is over
            if (_gameRules.IsGameOver(_state))
            {
                _state.IsGameOver = true;
                float reward = _rewardSystem.CalculateReward(oldState, _state, action);
                return (_state.Clone(), reward, true);
            }
            
            // Check if food is eaten
            if (_gameRules.IsFoodEaten(_state))
            {
                // Grow the snake
                _state.Snake.Grow();
                
                // Increment score
                _state.IncrementScore();
                
                // Place new food
                var newFood = _foodPlacementStrategy.PlaceFood(_state);
                _state = new GameState(
                    _state.Snake,
                    newFood,
                    _state.BoardWidth,
                    _state.BoardHeight,
                    _state.Score,
                    _state.IsGameOver
                );
            }
            
            // Calculate reward
            float stepReward = _rewardSystem.CalculateReward(oldState, _state, action);
            
            return (_state.Clone(), stepReward, _state.IsGameOver);
        }

        public GameState GetState()
        {
            return _state.Clone();
        }

        public IReadOnlyList<Direction> GetValidActions()
        {
            var currentDirection = _state.Snake.CurrentDirection;
            var allDirections = Enum.GetValues(typeof(Direction))
                                   .Cast<Direction>()
                                   .Where(d => !d.IsOpposite(currentDirection))
                                   .ToList();
            
            return allDirections;
        }

        private GameState CreateInitialState()
        {
            // Create snake in the middle of the board
            int middleX = _boardWidth / 2;
            int middleY = _boardHeight / 2;
            var initialPosition = new Position(middleX, middleY);
            var initialDirection = (Direction)_random.Next(0, 4); // Random direction
            
            var snake = new Snake(initialPosition, initialDirection);
            
            // Create initial game state with food
            var initialState = new GameState(snake, new Food(initialPosition), _boardWidth, _boardHeight);
            
            // Place food
            var food = _foodPlacementStrategy.PlaceFood(initialState);
            
            return new GameState(snake, food, _boardWidth, _boardHeight);
        }
    }
}