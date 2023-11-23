using Microsoft.AspNetCore.Mvc;
using System;

public class HomeController : Controller
{
    public IActionResult Hello()
    {
        try
        {
           
            return View();
        }
        catch (Exception ex)
        {
           
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Error()
    {
       
        return View("Error");
    }
}
