using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.WebMVC.Controllers
{
    public class CategoryController : Controller
    {
       
        [Authorize]
        // GET: Category
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CategoryService(userId);
            var model = service.GetCategories();
            return View(model);
        }

        //GET: Category

        public ActionResult Create()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryCreate model)
        {
            if (!ModelState.IsValid) return View(model);
            var svc = CreateCategoryService();
            if (svc.CreateCategory(model))
            {
                TempData["SaveResult"] = "Your note was created!";
                return RedirectToAction("Index");
            };
            ModelState.AddModelError("", "Category could not be created.");
            return View(model);
        }

        //GET Category/{id}
        public ActionResult Details(int id)
        {
            var svc = CreateCategoryService();
            var model = svc.GetCategoryById(id);

            return View(model);
        }

        //GET Edit

        public ActionResult Edit(int id)
        {
            var svc = CreateCategoryService();
            var detail = svc.GetCategoryById(id);
            var model = new CategoryEdit
            {
                CategoryId = detail.CategoryId,
                Name = detail.Name
            };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryEdit model, int id)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.CategoryId != id)
            {
                ModelState.AddModelError("", "ID Mismatch");
                return View(model);
            }

            var svc = CreateCategoryService();

            if (svc.UpdateCategory(model))
            {
                TempData["Save Result"] = "Category updated!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Category could not be updated");
            return View(model);
            
        }

        public ActionResult Delete(int? id)
        {
            var svc = CreateCategoryService();
            CategoryDetail category = svc.GetCategoryById((int)id);
            if (category == null)
                return View(HttpNotFound());
            return View(category);
        }


        [ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(int id)
        {
            var catSvc = CreateCategoryService();
            var noteSvc = CreateNoteService();
            var noteList = GetCategoryNotes(id);
            foreach(var item in noteList)
            {
                var editModel = new NoteEdit();
                var detailModel = noteSvc.GetNoteById(item.NoteId);
                editModel.NoteId = detailModel.NoteId;
                editModel.Title = detailModel.Title;
                editModel.Content = detailModel.Content;
                editModel.CategoryId = null;
                noteSvc.UpdateNote(editModel);
                
            }
            catSvc.DeleteCategory(id);
            TempData["SaveResult"] = "Your category was deleted, and all notes with that category will be updated.";
            return RedirectToAction("Index");
        }


        //Helper

        public IEnumerable<NoteListItem> GetCategoryNotes(int? catId)
        {
            var svc = CreateNoteService();
            return svc.GetNotesByCatId(catId);
        }
        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        } 

        private CategoryService CreateCategoryService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CategoryService(userId);
            return service;
        }
    }
}