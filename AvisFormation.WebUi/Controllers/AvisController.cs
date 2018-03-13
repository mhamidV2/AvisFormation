using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AvisFormation.Data;
using AvisFormation.Logic;
using AvisFormation.WebUi.Models;
using Microsoft.AspNet.Identity;

namespace AvisFormation.WebUi.Controllers
{
    public class AvisController : Controller
    {
        // GET: Avis
        [Authorize]
        public ActionResult LaisserUnAvis(string nomSeo)
        {
            var vm = new LaisserUnAvisViewModel();
            vm.NomSeo = nomSeo;
            using (var context = new AvisEntities())
            {
                var formationEntity = context.Formation.FirstOrDefault(f => f.NomSeo == nomSeo);

                if (formationEntity == null)
                    return RedirectToAction("Acceuil", "Home");

                vm.FormationName = formationEntity.Nom;
            }
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        //public ActionResult SaveComment(string commentaire, string nom, string note, string nomSeo)
        public ActionResult SaveComment(SaveCommentViewModel comment)
        {
            using (var context = new AvisEntities())
            {
                var formationEntity = context.Formation.FirstOrDefault(f => f.NomSeo == comment.nomSeo);

                if (formationEntity == null)
                    return RedirectToAction("Acceuil", "Home");

                Avis nouvelAvis = new Avis();

                nouvelAvis.DateAvis = DateTime.Now;
                nouvelAvis.Description = comment.commentaire;

                nouvelAvis.UserId = User.Identity.GetUserId();

                var userId = User.Identity.GetUserId();

                var mgerUnicite = new UniqueAvisVerification();
                if(!mgerUnicite.EstAuthoriseACommenter(userId,formationEntity.Id))
                {
                    TempData["Message"] = "Désolé, vous ne pouvez poster qu'un seul avis par formation";
                    return RedirectToAction("DetailsFormation", "Formation", new { nomSeo = comment.nomSeo });
                }

                var mger = new PersonneManager();
                nouvelAvis.Nom = mger.GetNomFromUserId(userId);



                double dNote = 0;

                if(!double.TryParse(comment.note, NumberStyles.Any, CultureInfo.InvariantCulture, out dNote))
                {
                    throw new Exception("impossible de parser la note "+ comment.note);
                }
            
                nouvelAvis.Note = dNote;

                nouvelAvis.IdFormation = formationEntity.Id;
                context.Avis.Add(nouvelAvis);
                context.SaveChanges();
            }

            return RedirectToAction("DetailsFormation","Formation",new { nomSeo = comment.nomSeo });
        }
    }
}