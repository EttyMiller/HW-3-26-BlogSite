using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Class_3_12.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Class_3_12.Controllers;

public class HomeController : Controller
{
    private readonly string _connectionString = @"Data Source =.\sqlexpress;Initial Catalog = Northwnd; Integrated Security = true; TrustServerCertificate=yes;";

    public IActionResult Index()
    {
        var db = new NorthwndDb(_connectionString);
        var vm = new NorthwndViewModel()
        {
            orders = db.GetOrders()
        };
        return View(vm);
    }
}
