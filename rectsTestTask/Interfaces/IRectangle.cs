using System.Drawing;

namespace rectsTestTask.Interfaces
{
    public interface IRectangle : IDeletable
    {
        Point Size { get; set; }
        Point Position { get; set; }

        bool DoesIntersect(IRectangle rectangle);
    }
}
