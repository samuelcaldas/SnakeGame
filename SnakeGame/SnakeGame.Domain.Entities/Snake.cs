using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Domain.Entities
{
    /// <summary>
    /// Represents a snake segment in the snake's body
    /// </summary>
    public class SnakeSegment
    {
        public Position Position { get; set; }
        public SnakeSegment? Next { get; set; }
        public SnakeSegment? Previous { get; set; }

        public SnakeSegment(Position position)
        {
            Position = position;
        }

        public SnakeSegment Clone()
        {
            return new SnakeSegment(Position);
        }
    }

    /// <summary>
    /// Represents the snake entity in the game
    /// </summary>
    public class Snake
    {
        private SnakeSegment _head;
        private SnakeSegment _tail;
        private Direction _currentDirection;

        public Direction CurrentDirection => _currentDirection;
        public Position HeadPosition => _head.Position;

        public Snake(Position initialPosition, Direction initialDirection)
        {
            _head = new SnakeSegment(initialPosition);
            _tail = _head;
            _currentDirection = initialDirection;
        }

        public IReadOnlyList<Position> GetBody()
        {
            var body = new List<Position>();
            var currentSegment = _head;
            
            while (currentSegment != null)
            {
                body.Add(currentSegment.Position);
                currentSegment = currentSegment.Next;
            }
            
            return body;
        }

        public void Move(Direction direction)
        {
            if (direction.IsOpposite(_currentDirection) && HasMoreThanOneSegment())
            {
                // Cannot move in the opposite direction
                direction = _currentDirection;
            }

            _currentDirection = direction;
            Position newHeadPosition = _head.Position.Translate(direction);
            
            // Move the tail to become the new head
            if (_head != _tail)
            {
                var oldTail = _tail;
                _tail = _tail.Previous!;
                _tail.Next = null;
                
                oldTail.Position = newHeadPosition;
                oldTail.Next = _head;
                oldTail.Previous = null;
                
                _head.Previous = oldTail;
                _head = oldTail;
            }
            else
            {
                _head.Position = newHeadPosition;
            }
        }

        public void Grow()
        {
            var newSegment = new SnakeSegment(_head.Position.Translate(_currentDirection));
            newSegment.Next = _head;
            _head.Previous = newSegment;
            _head = newSegment;
        }

        public bool CollidesWithSelf()
        {
            var headPosition = _head.Position;
            var currentSegment = _head.Next;
            
            while (currentSegment != null)
            {
                if (currentSegment.Position.Equals(headPosition))
                {
                    return true;
                }
                currentSegment = currentSegment.Next;
            }
            
            return false;
        }

        public bool CollidesWithWall(int width, int height)
        {
            var position = _head.Position;
            return position.X < 0 || position.X >= width || 
                   position.Y < 0 || position.Y >= height;
        }

        public Snake Clone()
        {
            var clone = new Snake(_head.Position, _currentDirection);
            
            // If snake has more than one segment, need to clone all segments
            if (HasMoreThanOneSegment())
            {
                var originalCurrent = _head.Next;
                var cloneCurrent = clone._head;
                
                while (originalCurrent != null)
                {
                    var newSegment = new SnakeSegment(originalCurrent.Position);
                    cloneCurrent.Next = newSegment;
                    newSegment.Previous = cloneCurrent;
                    
                    cloneCurrent = newSegment;
                    originalCurrent = originalCurrent.Next;
                }
                
                clone._tail = cloneCurrent;
            }
            
            return clone;
        }

        private bool HasMoreThanOneSegment()
        {
            return _head != _tail;
        }
    }
}