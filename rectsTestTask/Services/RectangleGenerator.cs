using rectsTestTask.Interfaces;
using System.Drawing;
using Rectangle = rectsTestTask.Models.Rectangle;

namespace rectsTestTask.Services
{
    public class RectangleGenerator : IRectangleGenerator
    {
        public IList<IRectangle> Rectangles { get; private set; } = new List<IRectangle>();
        private Point WindowSize { get; set; }
        private int DefaultIterationsToDie { get; set; }

        public IEnumerable<IRectangle> RecentlyRemoved { get; private set; } = new List<IRectangle>();
        public IRectangle? RecentlyAdded { get; private set; }

        public void Reset(Point windowSize, int rectangleIterationsToDie)
        {
            Rectangles.Clear();
            RecentlyRemoved = new List<IRectangle>();
            RecentlyAdded = null;
            WindowSize = windowSize;
            DefaultIterationsToDie = rectangleIterationsToDie;
        }

        public void Iterate()
        {
            RecentlyAdded = GetRandomRectangleFitWindow(WindowSize, DefaultIterationsToDie);
            var recentlyRemoved = new List<IRectangle>();

            Rectangles.Where(r => !r.ToDelete && r.DoesIntersect(RecentlyAdded)).ToList().ForEach(r => r.ToDelete = true);
            Rectangles.Add(RecentlyAdded);

            Rectangles.Where(r => r.ToDelete).ToList().ForEach(r =>
            {
                if (r.IterationsLeft <= 0) 
                { 
                    Rectangles.Remove(r);
                    recentlyRemoved.Add(r);
                }
                else
                {
                    r.IterationsLeft--;
                }
            });

            RecentlyRemoved = recentlyRemoved;
        }

        private static IRectangle GetRandomRectangleFitWindow(Point windowSize, int iterationsToDie)
        {
            var xPosition = Random.Shared.Next(0, windowSize.X);
            var yPosition = Random.Shared.Next(0, windowSize.Y);

            var xSize = Random.Shared.Next(0, windowSize.X - xPosition);
            var ySize = Random.Shared.Next(0, windowSize.Y - yPosition);

            var rectangle = new Rectangle(new Point(xSize, ySize), new Point(xPosition, yPosition), iterationsToDie);

            return rectangle;
        }
    }
}
