using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Projektdatabase.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Projektdatabase.Persistence;

namespace Projektdatabase.Controllers
{
    public class HomeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("projektdatabasen")]
        public IActionResult ProjektDatabasen()
        {
            IEnumerable<ProjektModel> projekt = _unitOfWork.RetrieveAllProjekts();
            List<ProjektModel> test = new List<ProjektModel>();
            test = projekt.ToList();
            var check = test[0].KlassifikationModels.ToList();
            return View(projekt);
        }
        [HttpGet("create")]
        public IActionResult Submit()
        {
            ProjektModel projekt = _unitOfWork.GetProjektModel();
            return View(projekt);
        }

        [HttpPost("create")]
        public IActionResult Submit(ProjektModel projektModel)
        {
            if (!ModelState.IsValid) return View(projektModel);
            
            for (var i = 0; i < projektModel.KlassifikationModels.Count; i++) {
                if (projektModel.KlassifikationModels[i].IsChecked) continue;
                projektModel.KlassifikationModels.RemoveAt(i);
                i--;
            }
            for (var i = 0; i < projektModel.UddOmrModels.Count; i++)
            {
                if (projektModel.UddOmrModels[i].IsChecked) continue;
                projektModel.UddOmrModels.RemoveAt(i);
                i--;
            }

            _unitOfWork.SubmitProjekt(projektModel);
            return RedirectToAction("index");

            //ADD 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
