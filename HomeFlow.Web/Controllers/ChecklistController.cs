using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeFlow.Web.Controllers
{
    [Authorize]
    public class ChecklistController : Controller
    {
        public IActionResult Configuracion() => View();
    }
}