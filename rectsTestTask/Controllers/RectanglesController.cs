using Microsoft.AspNetCore.Mvc;
using rectsTestTask.Interfaces;
using rectsTestTask.Services;
using System.Drawing;

namespace rectsTestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RectanglesController : ControllerBase
    {
        private readonly RectangleGenerator _rectsStore;

        public RectanglesController(RectangleGenerator rectsStore)
        {
            _rectsStore = rectsStore;
        }

        [HttpGet]
        [Route("get_all_rectangles")]
        public IEnumerable<IRectangle> GetAllRectangles()
        {
            return _rectsStore.Rectangles;
        }

        [HttpPost]
        [Route("reset")]
        public void Reset(int xSize, int ySize, int iterationsToDie)
        {
            _rectsStore.Reset(new Point(xSize, ySize), iterationsToDie);
        }

        [HttpGet]
        [Route("iterate")]
        public ActionResult Iterate()
        {
            _rectsStore.Iterate();

            var iterateResult = new
            {
                added = _rectsStore.RecentlyAdded,
                removed = _rectsStore.RecentlyRemoved
            };

            return new JsonResult(iterateResult);
        }
    }
}
