using SnakeGame.Application;
using SnakeGame.Domain.Entities;
using System;
using System.Threading;

namespace SnakeGame.Infrastructure
{
    /// <summary>
    /// Main game controller class
    /// </summary>
    public class Game
    {
        private readonly IEnvironment _environment;
        private readonly IAgent _agent;
        private readonly IRenderer _renderer;
        private readonly int _frameDelayMs;
        private bool _isRunning;

        public Game(IEnvironment environment, IAgent agent, IRenderer renderer, int frameDelayMs = 100)
        {
            _environment = environment;
            _agent = agent;
            _renderer = renderer;
            _frameDelayMs = frameDelayMs;
            _isRunning = false;
        }

        public void Run()
        {
            _isRunning = true;
            GameState state = _environment.Reset();
            _renderer.Render(state);
            
            // For training statistics when using machine agent
            int episode = 0;
            int totalSteps = 0;
            float totalReward = 0;
            
            while (_isRunning)
            {
                // Get action from agent
                Direction action = _agent.GetAction(state);
                
                // Step the environment
                var (nextState, reward, done) = _environment.Step(action);
                
                // Update statistics
                totalSteps++;
                totalReward += reward;
                
                // If using a machine agent, train it
                if (_agent is MachineAgent machineAgent)
                {
                    machineAgent.Train(state, action, reward, nextState, done);
                }
                
                // Render the new state
                _renderer.Render(nextState);
                
                // Update state
                state = nextState;
                
                // If game is over, reset
                if (done)
                {
                    episode++;
                    
                    // Display training statistics for machine agent
                    if (_agent is MachineAgent)
                    {
                        Console.WriteLine($"Episode {episode} - Steps: {totalSteps}, Total Reward: {totalReward:F2}, Score: {state.Score}");
                        
                        // Reset statistics
                        totalSteps = 0;
                        totalReward = 0;
                    }
                    
                    // Check if user wants to quit or continue
                    Console.WriteLine("Press 'Q' to quit, any other key to continue.");
                    if (Console.ReadKey(true).Key == ConsoleKey.Q)
                    {
                        _isRunning = false;
                    }
                    else
                    {
                        state = _environment.Reset();
                        _renderer.Render(state);
                    }
                }
                
                // Delay for frame rate control
                Thread.Sleep(_frameDelayMs);
            }
        }
    }
}