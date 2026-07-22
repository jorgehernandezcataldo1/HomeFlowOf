using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeFlow.Web.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        public IActionResult Index() => View();
    }
}