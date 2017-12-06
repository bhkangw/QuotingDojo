using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DbConnection;

namespace QuotingDojo.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if(TempData["Error"] != null){
                ViewBag.Error = TempData["Error"];
            }
            return View();
        }

        [HttpPost]
        [Route("/quotes")]
        public IActionResult AddQuotes(string author, string content)
        {
            if(author == "" || content == ""){
                TempData["Error"] = "Neither field should be empty!";
                return RedirectToAction("Index");
            }

            //Add the quote to the database
            string query = $"INSERT INTO quotes (author, content, created_at) VALUES ('{author}', '{content}', NOW())";
            DbConnector.Execute(query);
            return RedirectToAction("Quotes");
        }

        [HttpGet]
        [Route("/quotes")]
        public IActionResult Quotes()
        {
            //Get all quotes
            string query = "SELECT * FROM quotes ORDER BY created_at desc";
            var quotes = DbConnector.Query(query);

            ViewBag.quotes = quotes;
            return View();
        }
    }
}
