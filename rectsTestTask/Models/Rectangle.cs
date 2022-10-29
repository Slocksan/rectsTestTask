using rectsTestTask.Interfaces;
using System.Drawing;

namespace rectsTestTask.Models
{
    public class Rectangle : IRectangle
    {
        public Point Size { get; set; }
        public Point Position { get; set; }
        public bool ToDelete { get; set; }
        public int IterationsLeft { get; set; }

        public Rectangle(Point size, Point position, int iterationsLeft, bool toDelete = false)
        {
            Size = size;
            Position = position;
            ToDelete = toDelete;
            IterationsLeft = iterationsLeft;
        }

        public bool DoesIntersect(IRectangle rectangle) =>
                Position.X <= rectangle.Position.X + rectangle.Size.X &&
                Position.X + Size.X >= rectangle.Position.X &&
                Position.Y <= rectangle.Position.Y + rectangle.Size.Y &&
                Position.Y + Size.Y >= rectangle.Position.Y;
    }
}
