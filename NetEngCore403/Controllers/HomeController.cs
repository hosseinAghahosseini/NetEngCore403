using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NetEngCore403.Entities;
using NetEngCore403.Models;
using NetEngCore403.Services;

namespace NetEngCore403.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        DateTime date = DateTime.Now;
        ViewBag.Now = date;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult ListMovies(string Message = "none")
    {
        MovieService ms = new MovieService(_context);
        var res = ms.ReadMovieList();

        ViewBag.Message = Message;

        return View(res);
    }

    public IActionResult CreateMovie()
    {
        DirectorService ds = new DirectorService(_context);
        ViewBag.DirectorList = ds.ReadDirectorList();
        return View();
    }

    [HttpPost]
    public IActionResult CreateMovie(Movie Model)
    {
        MovieService ms = new MovieService(_context);
        var res = ms.CreateMovie(Model);
        if (res > 0)
        {
            return RedirectToAction("ListMovies", "Home", new { Message = "Insert Was Successful" });
        }
        return RedirectToAction("ListMovies", "Home", new { Message = "Error" });

    }
}
