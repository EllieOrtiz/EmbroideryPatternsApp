using EmbroideryPatternsApp.Services;
using EmbroideryPatternsApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmbroideryPatternsApp.Models;

namespace EmbroideryPatternsApp.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private WorldContext _context;

        public AppController(IMailService mailService, IConfigurationRoot config, Models.WorldContext context)
        {
            _mailService = mailService;
            _config = config;
            _context = context;
        }

     public IActionResult Index()
        {
            var data = _context.Trips.ToList();

            return View(data);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (model.Email.Contains("hotmail.com"))
            {
                ModelState.AddModelError("", "We dont support hotmail addresses");
            }

            if (ModelState.IsValid)
            {

                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "From StitchAPattern", model.Message);

                ModelState.Clear();

                ViewBag.UserMessage = "Message Sent";
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
