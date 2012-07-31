using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Samples.HvMvc.Areas.api.Controllers
{
    public class Default1Controller : Controller
    {
        //
        // GET: /api/Default1/

        public ActionResult Index()
        {
            return View();
        }

    }
}
