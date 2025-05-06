using System;

namespace SnakeGame.Domain.Entities
{
    /// <summary>
    /// Represents a position in the 2D game grid
    /// </summary>
    public readonly struct Position
    {
        public readonly int X { get; }
        public readonly int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position Translate(Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Position(X, Y - 1),
                Direction.Down => new Position(X, Y + 1),
                Direction.Left => new Position(X - 1, Y),
                Direction.Right => new Position(X + 1, Y),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        public double DistanceTo(Position other)
        {
            int dx = other.X - X;
            int dy = other.Y - Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override bool Equals(object? obj)
        {
            return obj is Position position &&
                   X == position.X &&
                   Y == position.Y;
        }

        public override int GetHashCode()
        {
            // Alternative implementation without using HashCode.Combine
            return (X * 397) ^ Y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}