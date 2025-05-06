namespace SnakeGame.Domain.Entities
{
    /// <summary>
    /// Represents the possible directions of movement
    /// </summary>
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public static class DirectionExtensions
    {
        public static bool IsOpposite(this Direction current, Direction other)
        {
            return (current == Direction.Up && other == Direction.Down) ||
                   (current == Direction.Down && other == Direction.Up) ||
                   (current == Direction.Left && other == Direction.Right) ||
                   (current == Direction.Right && other == Direction.Left);
        }
    }
}