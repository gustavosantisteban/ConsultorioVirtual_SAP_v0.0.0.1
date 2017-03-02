﻿using OpenTokSDK;
using OpenTokSDK.Exception;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    public class ArchiveController : Controller
    {
        private const int archivesPerPage = 5;
        private OpenTok opentok = new OpenTok(Convert.ToInt32(ConfigurationManager.AppSettings["opentok_key"]),
                                    ConfigurationManager.AppSettings["opentok_secret"]);

        // POST Archive/Start
        public string Start()
        {
            HttpApplicationState Application = HttpContext.ApplicationInstance.Application;
            Archive archive;

            try
            {
                archive = opentok.StartArchive((string)Application["sessionId"], "Consultorio Virtual");
            }
            catch (OpenTokException)
            {
                return "Error: El archivo no podría haberse creado";
            }
            return archive.Id.ToString();
        }

        // POST Archive/Stop
        public string Stop(string id)
        {
            Archive archive;

            try
            {
                archive = opentok.StopArchive(id);
            }
            catch (OpenTokException)
            {
                return "Error: El archivo no podría haberse creado";
            }
            return archive.Id.ToString();
        }

        public ActionResult Delete(string id)
        {
            if (id != null)
            {
                try
                {
                    opentok.DeleteArchive(id);
                }
                catch (OpenTokException)
                {
                    Redirect("/");
                }
            }
            return Redirect("/Archive/List/");
        }

        // GET Archive/Stop
        public ActionResult List(string id)
        {
            int page = 0;

            try
            {
                page = Int32.Parse(id);
            }
            catch (Exception)
            {
                page = 0;
            }

            try
            {
                ViewBag.Archives = opentok.ListArchives(page * archivesPerPage, archivesPerPage);
            }
            catch (OpenTokException)
            {
                ViewBag.Error = "El archivo no podría haberse creado";
                return View();
            }

            if (page > 0)
            {
                ViewBag.ShowPrevious = string.Format("/Archive/List/{0}", page - 1);
            }
            if (ViewBag.Archives.TotalCount > page * archivesPerPage + archivesPerPage)
            {
                ViewBag.ShowNext = string.Format("/Archive/List/{0}", page + 1);
            }
            return View();
        }
    }
}