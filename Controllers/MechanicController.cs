using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Автомеханик")]
public class MechanicController : Controller
{
    public IActionResult Dashboard()
    {
        return View(); // MVC автоматично ще зареди /Views/Mechanic/Dashboard.cshtml
    }
}
