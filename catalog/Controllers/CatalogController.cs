using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using acme.Models;

namespace acme.Controllers;

public class CatalogController : Controller {
    private readonly ILogger<HomeController> _logger;

    public CatalogController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Redeem(string prodid)
    {        
        return View(new RedeemResponse() {Code = 0});
    }
}