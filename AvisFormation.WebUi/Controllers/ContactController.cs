﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AvisFormation.Logic;

namespace AvisFormation.WebUi.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnvoyerEmail(string nom, string email, string message)
        {
            try
            {
                var mger = new EmailManager();
                mger.SendEmail(nom, message, email);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return View("ErreurEnvoi");
            }
            return View("Merci");
        }
    }
}