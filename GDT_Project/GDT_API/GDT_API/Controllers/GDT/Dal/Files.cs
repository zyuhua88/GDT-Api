using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Http;
using System.Data;

namespace GDT_API.Controllers.GDT.Dal
{
    public class Files
    {

        public string UploadFile(HttpPostedFile file)
        {
            string obj = "{\"code\": 0,\"msg\": \"\",\"data\": {\"src\": \"http://cdn.layui.com/123.jpg\"}}";
            Stream st = file.InputStream;
            string Ft = file.FileName.Substring(file.FileName.LastIndexOf("."), file.FileName.Length - file.FileName.LastIndexOf("."));
            Random ran = new Random();
            string Fn = ran.Next(100000, 999999) + DateTime.Now.ToFileTime() + Ft;
            string path = AppDomain.CurrentDomain.BaseDirectory + "/Files/" + Fn;
            string msg = "";
            Zh.Tool.File_Tool.File_Upload(st,path,out msg);
            if (msg == "A0000")
            {
                obj = "{\"code\": 0,\"msg\": \"文件上传成功\",\"data\": {\"src\": \"" + Fn + "\"}}";
                return obj;
            }
            else {
                obj = "{\"code\": 1,\"msg\": \"文件上传失败\",\"data\": {\"src\": \"\"}}";
                return obj;
            }
        }
        
    }
}