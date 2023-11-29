using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestingBranchesWebsite.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Branches()
    {
      ViewBag.Message = "DRGT Malta Branches.";

      return View();
    }
  }
}