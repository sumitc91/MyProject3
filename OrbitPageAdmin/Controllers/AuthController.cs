using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urNotice.Common.Infrastructure.Model.Person;
using urNotice.Common.Infrastructure.Model.urNoticeModel.EmailModel;
using urNotice.Services.Person;

namespace OrbitPageAdmin.Controllers
{
    public class AuthController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult SendEmail(CreateOrbitPageEmailRequest req)
        {
            IPerson adminModel = new Admin();
            var response = adminModel.SendEmail(req, Request);
            return Json(response);
        }
    }
}
