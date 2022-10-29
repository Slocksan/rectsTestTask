using System.Drawing;

namespace rectsTestTask.Interfaces
{
    public interface IRectangleGenerator
    {
        IList<IRectangle> Rectangles { get; }
        IEnumerable<IRectangle> RecentlyRemoved { get; }
        IRectangle? RecentlyAdded { get; }

        public void Reset(Point windowSize, int rectangleIterationsToDie);
        public void Iterate();
    }
}
