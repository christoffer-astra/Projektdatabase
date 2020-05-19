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
        [HttpGet("submit")]
        public IActionResult Submit()
        {
            List<UddOmrModel> udds = _unitOfWork.Complete();
            //List<UddOmrModel> uddOmr = _repository.GetAll(_connection);
            //int id = _repository.GetId("EUD", _connection);
            //UddOmrModel udd = _repository.Get(id, _connection);
            //uddOmr.Add(udd);
            //Console.WriteLine(udd.UddOmrId.ToString()+udd.UddOmrName);
            //List<UddOmrModel> uddOmr = new List<UddOmrModel>();
            //using (IDbConnection db = new SqlConnection(_connectionString))
            //{
            //    uddOmr = db.Query<UddOmrModel>("Select uddOmrId, uddOmrName FROM UddOmr").ToList();
            //}
            return View(udds);
        }

        [HttpPost("submit")]
        public IActionResult Submit(Object model)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
