using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        if (User.IsInRole("Admin"))
            return RedirectToAction("Admin");

        if (User.IsInRole("Doctor"))
            return RedirectToAction("Doctor");

        if (User.IsInRole("Patient"))
            return RedirectToAction("Patient");

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Admin()
    {
        return View();
    }

    public IActionResult Doctor()
    {
        return View();
    }

    public IActionResult Patient()
    {
        return View();
    }
}