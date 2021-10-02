using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using GeometricProgramming.Services.Abstract;

namespace GeometricProgramming.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPrintService _printService;
        public HomeController(IPrintService printService)
        {
            this._printService = printService;
        }
        public IActionResult Index()
        {
            return View();
        }       
        
    }
}
