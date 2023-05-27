using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        [Route("/home")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("/action={act}")]
        public IActionResult Get(string act)
        {
            string res = "";

            switch (act)
            {
                case "^":
                    res = "forward";
                    break;
                case ">":
                    res = "right";
                    break;
                case "<":
                    res = "left";
                    break;
                case ".":
                    res = "backward";
                    break;
            }

            if (res != "")
                ControllerActions.Move(res);
            return Json("ok");
        }

        [Route("/check")]
        public IActionResult Check()
        {
            return Json(ControllerActions.IsOnline().Result);
        }

        [Route("/actionstrong={p1}&{p2}&{p3}&{p4}&{time}")]
        public IActionResult GetStrong(string p1, string p2, string p3, string p4, string time)
        {
            List<ActionObj> objs = new List<ActionObj>();

            objs.Add(new ActionObj()
            {
                Port = 17,
                R = (p1 == "1" ? true : false)
            });
            objs.Add(new ActionObj()
            {
                Port = 22,
                R = (p2 == "1" ? true : false)
            });
            objs.Add(new ActionObj()
            {
                Port = 23,
                R = (p3 == "1" ? true : false)
            });
            objs.Add(new ActionObj()
            {
                Port = 24,
                R = (p4 == "1" ? true : false)
            });
            if (objs.Count > 0)
            {
                ControllerActions.Move(objs, int.Parse(time));
            }
            return Json("ok");
        }
    }
}
