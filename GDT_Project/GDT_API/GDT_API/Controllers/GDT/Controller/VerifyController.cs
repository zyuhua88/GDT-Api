using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDT_API.Controllers.GDT.Controller
{
    public class VerifyController : ApiController
    {
        private Lazy<Dal.verify> ver = new Lazy<Dal.verify>();
        private Lazy<Dal.users> user = new Lazy<Dal.users>();

        /// <summary>
        /// 获取登录用户的权限
        /// </summary>
        /// <param name="usid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/verify/gdt/getusers")]
        public HttpResponseMessage GetUsers(int usid)
        {
            List<Entity.user_detail> list = ver.Value.GetUserVal(usid);
            object obj = new {
                code = "A0000",
                msg = "成功",
                data = list
            };
            return Zh.Tool.Json.GetJson(obj);
        }

        [HttpPost]
        [Route("api/verify/gdt/getusid")]
        public int GetUsid(dynamic data)
        {
            string usname = data.usname;
            string pwds = data.pwds;
            int usid = 0;
            user.Value.userLogin(usname, pwds, out usid);
            return usid;
        }
    }
}
