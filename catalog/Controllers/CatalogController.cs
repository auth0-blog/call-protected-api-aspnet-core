using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
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
    public async Task<IActionResult> Redeem(string prodid)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        RedeemResponse response;

        using(var httpClient = new HttpClient())
        {
            string ApiUrl = "https://localhost:7151/redeem";
            httpClient.DefaultRequestHeaders.Authorization 
                         = new AuthenticationHeaderValue("Bearer", accessToken);
            response = await httpClient.GetFromJsonAsync<RedeemResponse>(ApiUrl);
        }

        response.ProdId = prodid;

        return View(response);
    }
}