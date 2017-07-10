using ImageGallery.Models.DAL;
using ImageGallery.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageGallery.Controllers
{
    public class GalleryAdminController : Controller
    {
        // GET: GalleryAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ImageEditorViewModel vm = new ImageEditorViewModel();
            ViewBag.Action = "Create";
            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult Create(ImageEditorViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ImageGalleryEntities db = new ImageGalleryEntities(); 

                    var fileModel = WebFileViewModel.getEntityModel(model.FileImage);
                    db.WebFiles.Add(fileModel);
                    db.SaveChanges();

                    var entity = ImageEditorViewModel.getEnityModel(model);                  
                    entity.WebImageId = fileModel.Id;
                    entity.OrderNo = db.Galleries.Count() > 0 ? db.Galleries.Max(x=> x.OrderNo) + 1 : 1;
                    db.Galleries.Add(entity);
                    db.SaveChanges();

                    return Json(new { success = true, Caption = model.Caption });
                }

                return Json(new { success = false, ValidationMessage = "Please check validation messages" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, ExceptionMessage = "Some error here" });
            }
        }

        public ActionResult _List()
        {
            ImageGalleryEntities db = new ImageGalleryEntities();
            var list = db.Galleries.OrderBy(x => x.OrderNo)
                        .Select(x => new ImageList
                        {
                            Id = x.Id,
                            IsActive = x.IsActive,
                            OrderNo = x.OrderNo,
                            WebImageId = x.WebImageId,
                            Title = x.Title
                        }).ToList();

            return PartialView(list);
        }
        

        [HttpPost]
        public JsonResult ChangeActive(int Id, bool status)
        {
            ImageGalleryEntities db = new ImageGalleryEntities();
            var entity = db.Galleries.Find(Id);
            entity.IsActive = status;
            db.SaveChanges();

            return Json(entity.Title);
        }
    }
}