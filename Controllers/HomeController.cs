using HackerNewsWeb.Models;
using HackerNewsWeb.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace HackerNewsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILog _logger;
        // private readonly ILogger<HomeController> _logger;
        //
        private readonly HackerNewsService _hackerNewsService;

        public HomeController(ILog logger, HackerNewsService hackerNewsService)
        {
            _logger = logger;
            _hackerNewsService = hackerNewsService;
        }

        public IActionResult Index(int page = 1)
        {
            _logger.Info($"page:{page}");

            try
            {
                var (ids, pageCount) = _hackerNewsService.GetTopStories(page).Result;
                var items = _hackerNewsService.GetItems(ids).Result;

                ViewBag.PageCount = pageCount;
                ViewBag.CurrentPage = page;

                return View(items);
            }
            catch(Exception ex)
            {
                _logger.Error($"Exception,page:{page},message:{ex.Message}");

                return RedirectToAction("Error");
            }
        }

        [Route("Home/Detail/{id}")]
        public IActionResult Detail(int id, int page = 1)
        {
            _logger.Info($"id:{id}");

            if (id <= 0)
            {
                return RedirectToAction("Index");
            }

            try
            {
                var item = _hackerNewsService.GetItemOrNull(id).Result;

                if (item == null)
                {
                    _logger.Warn($"item not found,id:{id}");

                    return RedirectToAction("Index");
                }

                if (item.Type != "story")
                {
                    _logger.Warn($"type not story,id:{id},type:{item.Type}");

                    return RedirectToAction("Index");
                }

                ViewBag.CurrentPage = page;

                return View(item);
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception,id:{id},page:{page},message:{ex.Message}");

                return RedirectToAction("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
