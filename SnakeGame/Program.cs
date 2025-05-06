using SnakeGame.Application;
using SnakeGame.Domain.Entities;
using SnakeGame.Domain.Services;
using SnakeGame.Infrastructure;
using System;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Snake Game with Reinforcement Learning");
            Console.WriteLine("1. Human Player");
            Console.WriteLine("2. Machine Learning Agent");
            Console.Write("Select mode (1-2): ");
            string input = Console.ReadLine() ?? "1";
            
            // Configure game
            int boardWidth = 20;
            int boardHeight = 15;
            int seed = new Random().Next(); // Random seed
            
            // Create environment components
            var gameRules = new StandardGameRules();
            var rewardSystem = new StandardRewardSystem();
            var foodPlacementStrategy = new RandomFoodPlacementStrategy(seed);
            
            // Create environment
            var environment = new SnakeEnvironment(
                gameRules,
                rewardSystem,
                foodPlacementStrategy,
                boardWidth,
                boardHeight,
                seed);
            
            // Create renderer
            var renderer = new ConsoleRenderer();
            
            // Create agent based on user selection
            IAgent agent;
            int frameDelay;
            
            if (input == "2")
            {
                Console.WriteLine("Running in Machine Learning mode");
                agent = new MachineAgent(epsilon: 0.2f, seed: seed);
                frameDelay = 50; // Faster for machine agent
            }
            else
            {
                Console.WriteLine("Running in Human Player mode");
                agent = new HumanAgent();
                frameDelay = 150; // Slower for human player
            }
            
            // Create and run game
            var game = new Game(environment, agent, renderer, frameDelay);
            game.Run();
        }
    }
}