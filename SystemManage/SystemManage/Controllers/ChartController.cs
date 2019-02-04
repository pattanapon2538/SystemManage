using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Models;
using SystemManage.Database;

namespace SystemManage.Controllers
{
    public class ChartController : Controller
    {
        Entities db = new Entities();
        // GET: Chart
        public ActionResult Index()
        {
            ProjectModel model = new ProjectModel();
            int ProjectID = 2007;
            var t = db.Tasks.Where(m => m.ProjectID == ProjectID).ToList();
            List<Chart> dataPoints = new List<Chart>();
            double total = 100 / t.Count;
            foreach (var item in t)
            {
                double TaskPercent = (item.TotalPercent / 100) * total;
                dataPoints.Add(new Chart(item.TaskName, TaskPercent));
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }
    }
}