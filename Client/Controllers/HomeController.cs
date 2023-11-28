using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Common;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Result = TempData["Result"] as string ?? "";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult<string>> ValidateModel(DataModel model)
        {
            try
            {
                IValidator proxy = ServiceProxy.Create<IValidator>(new Uri("fabric:/CloudZadatak/Validator"));

                TempData["Result"] = await proxy.Validate(model);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Result"] = "Error in communication with service " + ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}
