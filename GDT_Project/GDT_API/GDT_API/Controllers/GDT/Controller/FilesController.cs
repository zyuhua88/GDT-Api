using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GDT_API.Controllers.GDT.Controller
{
    public class FilesController : ApiController
    {
        private Lazy<Dal.Files> file = new Lazy<Dal.Files>();


        [HttpPost]
        [Route("api/files/gdt/uploadfile")]
        public string UploadFile()
        {
            HttpPostedFile context = HttpContext.Current.Request.Files[0];
            return file.Value.UploadFile(context);
        }
    }
}
