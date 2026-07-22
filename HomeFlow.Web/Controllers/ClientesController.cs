// HomeFlow.Web/Controllers/ClientesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeFlow.Web.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        public IActionResult Propietarios() => View();
    }
}