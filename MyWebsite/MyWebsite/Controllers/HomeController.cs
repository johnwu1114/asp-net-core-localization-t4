using Microsoft.AspNetCore.Mvc;
using MyWebsite.Filters;
using Resources;
using System.Globalization;

namespace MyWebsite.Controllers
{
    [TypeFilter(typeof(CultureFilter))]
    public class HomeController : Controller
    {
        private readonly ILocalizer _localizer;

        public HomeController(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Content()
        {
            return Content($"CurrentCulture: {CultureInfo.CurrentCulture.Name}\r\n"
                         + $"CurrentUICulture: {CultureInfo.CurrentUICulture.Name}\r\n"
                         + $"{_localizer.Text.Hello}");
        }
    }
}