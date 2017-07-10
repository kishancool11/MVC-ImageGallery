using ImageGallery.Models.DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ImageGallery.Controllers
{
    public class DownloadFileController : Controller
    {  
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Any, VaryByParam = "Id;t")]
        public ActionResult GetImage(int Id = 0, int t = 0)
        {
            if (Id == 0)
            {
                return File("~/images/NoImage.png", "image/png");
            }

            ImageGalleryEntities db = new ImageGalleryEntities();

            var model = db.WebFiles.Find(Id);

            if (model != null)
            {
                if (t != 0)
                {
                    byte[] img = getThumbNail(model.Data, t);
                    return File(img, model.ContentType, "thumb_" + model.FileName);
                }

                return File(model.Data, model.ContentType, model.FileName);
            }
            else
            {
                return HttpNotFound();
            }
        }

        private byte[] getThumbNail(byte[] data, int multi = 1)
        {
            using (var file = new MemoryStream(data))
            {
                int width = 200 * multi; 
                using (var image = Image.FromStream(file, true, true)) /* Creates Image from specified data stream */
                {
                    int X = image.Width;
                    int Y = image.Height;
                    int height = (int)((width * Y) / X);

                    using (var thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero))
                    {
                        var jpgInfo = ImageCodecInfo.GetImageEncoders().Where(codecInfo => codecInfo.MimeType == "image/png").First(); /* Returns array of image encoder objects built into GDI+ */
                        using (var encParams = new EncoderParameters(1))
                        {
                            using (var samllfile = new MemoryStream())
                            {
                                long quality = 100;
                                encParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                                thumb.Save(samllfile, jpgInfo, encParams);
                                return samllfile.ToArray();
                            }
                        };
                    };
                };
            };
        }
    }
}