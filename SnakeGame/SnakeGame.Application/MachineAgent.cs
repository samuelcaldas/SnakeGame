using SnakeGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Application
{
    /// <summary>
    /// Implementation of a machine-controlled agent for reinforcement learning
    /// This is a simplified version - in a real implementation, you would have a neural network model
    /// </summary>
    public class MachineAgent : IAgent
    {
        private readonly Random _random;
        private float _epsilon; // Exploration rate
        
        // In a real implementation, this would be a neural network model
        private Dictionary<string, float[]> _qTable; // Simple Q-table for demonstration
        private readonly float _learningRate;
        private readonly float _discountFactor;
        private readonly float _epsilonDecay;
        
        // Training memory for experience replay
        private readonly List<(string state, Direction action, float reward, string nextState, bool done)> _memory;
        private readonly int _batchSize;
        
        public MachineAgent(float epsilon = 0.1f, float learningRate = 0.1f, float discountFactor = 0.99f, 
                           float epsilonDecay = 0.999f, int batchSize = 32, int? seed = null)
        {
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
            _epsilon = epsilon;
            _learningRate = learningRate;
            _discountFactor = discountFactor;
            _epsilonDecay = epsilonDecay;
            _batchSize = batchSize;
            
            _qTable = new Dictionary<string, float[]>();
            _memory = new List<(string, Direction, float, string, bool)>();
        }

        public Direction GetAction(GameState state)
        {
            // With probability epsilon, choose a random action (exploration)
            if (_random.NextDouble() < _epsilon)
            {
                int actionIndex = _random.Next(0, 4);
                return (Direction)actionIndex;
            }
            
            // Otherwise, choose the action with the highest Q-value (exploitation)
            var stateKey = SerializeState(state);
            
            if (!_qTable.ContainsKey(stateKey))
            {
                // Initialize Q-values for new state
                _qTable[stateKey] = new float[4]; // One value for each direction
            }
            
            // Find the action with the highest Q-value
            float[] qValues = _qTable[stateKey];
            int bestActionIndex = Array.IndexOf(qValues, qValues.Max());
            
            return (Direction)bestActionIndex;
        }

        public void Train(GameState state, Direction action, float reward, GameState nextState, bool done)
        {
            // Store experience in memory
            _memory.Add((SerializeState(state), action, reward, SerializeState(nextState), done));
            
            // Experience replay - train on a batch of past experiences
            if (_memory.Count >= _batchSize)
            {
                for (int i = 0; i < _batchSize; i++)
                {
                    int index = _random.Next(0, _memory.Count);
                    var (s, a, r, ns, d) = _memory[index];
                    
                    UpdateQValue(s, a, r, ns, d);
                }
                
                // Decay epsilon for less exploration over time
                _epsilon *= _epsilonDecay;
            }
        }
        
        private void UpdateQValue(string stateKey, Direction action, float reward, string nextStateKey, bool done)
        {
            // Initialize Q-values if not existing
            if (!_qTable.ContainsKey(stateKey))
            {
                _qTable[stateKey] = new float[4];
            }
            
            if (!_qTable.ContainsKey(nextStateKey))
            {
                _qTable[nextStateKey] = new float[4];
            }
            
            int actionIndex = (int)action;
            
            // Q-learning update rule
            float currentQ = _qTable[stateKey][actionIndex];
            float maxNextQ = done ? 0 : _qTable[nextStateKey].Max();
            
            // Q(s,a) = Q(s,a) + α * [r + γ * max Q(s',a') - Q(s,a)]
            _qTable[stateKey][actionIndex] = currentQ + _learningRate * (reward + _discountFactor * maxNextQ - currentQ);
        }
        
        private string SerializeState(GameState state)
        {
            // Simplified state representation - in a real implementation, this would be more sophisticated
            var headPos = state.Snake.HeadPosition;
            var foodPos = state.Food.Position;
            var direction = state.Snake.CurrentDirection;
            
            return $"{headPos.X},{headPos.Y},{foodPos.X},{foodPos.Y},{(int)direction}";
        }
    }
}