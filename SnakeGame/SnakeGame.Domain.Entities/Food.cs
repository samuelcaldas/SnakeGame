namespace SnakeGame.Domain.Entities
{
    /// <summary>
    /// Represents food that the snake can eat
    /// </summary>
    public class Food
    {
        public Position Position { get; }

        public Food(Position position)
        {
            Position = position;
        }

        public Food Clone()
        {
            return new Food(Position);
        }
    }
}