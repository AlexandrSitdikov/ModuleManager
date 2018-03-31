namespace SampleMVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class SampleController : Controller
    {
        public ActionResult ModuleList()
        {
            return this.Json((Global.Container.GetInstance(typeof(ModuleManager.Manager)) as ModuleManager.Manager).GetModules().Select(x => x.Name), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logs()
        {
            return this.Json((Global.Container.GetInstance(typeof(ILogger)) as ILogger).GetAll(), JsonRequestBehavior.AllowGet);
        }
    }
}