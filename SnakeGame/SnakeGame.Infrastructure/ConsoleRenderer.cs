using SnakeGame.Application;
using SnakeGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Infrastructure
{
    /// <summary>
    /// Interface for renderers
    /// </summary>
    public interface IRenderer
    {
        void Render(GameState state);
    }

    /// <summary>
    /// Console-based renderer for the snake game
    /// </summary>
    public class ConsoleRenderer : IRenderer
    {
        private readonly char _wallChar;
        private readonly char _snakeHeadChar;
        private readonly char _snakeBodyChar;
        private readonly char _foodChar;
        private readonly char _emptyChar;
        
        public ConsoleRenderer(
            char wallChar = '#',
            char snakeHeadChar = 'O',
            char snakeBodyChar = 'o',
            char foodChar = 'F',
            char emptyChar = ' ')
        {
            _wallChar = wallChar;
            _snakeHeadChar = snakeHeadChar;
            _snakeBodyChar = snakeBodyChar;
            _foodChar = foodChar;
            _emptyChar = emptyChar;
        }

        public void Render(GameState state)
        {
            Console.Clear();
            
            // Render top border
            Console.WriteLine(new string(_wallChar, state.BoardWidth + 2));
            
            // Get snake body positions for quick lookup
            var snakeBody = state.Snake.GetBody().ToList();
            var headPosition = snakeBody.First();
            
            // Render game board
            for (int y = 0; y < state.BoardHeight; y++)
            {
                Console.Write(_wallChar); // Left border
                
                for (int x = 0; x < state.BoardWidth; x++)
                {
                    var position = new Position(x, y);
                    
                    if (position.Equals(headPosition))
                    {
                        Console.Write(_snakeHeadChar);
                    }
                    else if (snakeBody.Contains(position))
                    {
                        Console.Write(_snakeBodyChar);
                    }
                    else if (position.Equals(state.Food.Position))
                    {
                        Console.Write(_foodChar);
                    }
                    else
                    {
                        Console.Write(_emptyChar);
                    }
                }
                
                Console.WriteLine(_wallChar); // Right border
            }
            
            // Render bottom border
            Console.WriteLine(new string(_wallChar, state.BoardWidth + 2));
            
            // Render score and game status
            Console.WriteLine($"Score: {state.Score}");
            
            if (state.IsGameOver)
            {
                Console.WriteLine("Game Over!");
            }
        }
    }
}