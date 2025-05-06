using SnakeGame.Domain.Entities;
using System;

namespace SnakeGame.Application
{
    /// <summary>
    /// Implementation of a human-controlled agent
    /// </summary>
    public class HumanAgent : IAgent
    {
        public Direction GetAction(GameState state)
        {
            var currentDirection = state.Snake.CurrentDirection;
            
            while (true)
            {
                if (!Console.KeyAvailable)
                {
                    // Return the current direction if no key is pressed
                    return currentDirection;
                }
                
                var key = Console.ReadKey(true).Key;
                
                var direction = key switch
                {
                    ConsoleKey.UpArrow => Direction.Up,
                    ConsoleKey.DownArrow => Direction.Down,
                    ConsoleKey.LeftArrow => Direction.Left,
                    ConsoleKey.RightArrow => Direction.Right,
                    _ => currentDirection
                };
                
                // Don't allow moving in opposite direction
                if (direction.IsOpposite(currentDirection))
                {
                    continue;
                }
                
                return direction;
            }
        }
    }
}