using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDT_API.Controllers.GDT.Controller
{
    public class user_menuController : ApiController
    {
        private Lazy<Dal.user_menu> menu = new Lazy<Dal.user_menu>();

        /// <summary>
        /// 添加或都修改用户的模块权限信息  如果用户权限存在，则修改
        /// </summary>
        /// <param name="data">data.menu</param>
        /// <param name="us_id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user_menu/gdt/Add")]
        public HttpResponseMessage Add(dynamic data, int us_id)
        {
            return menu.Value.Add(data,us_id);
        }

        /// <summary>
        /// 查询单个用户的模块权限信息
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user_menu/gdt/Query")]
        public HttpResponseMessage Query(int us_id)
        {
            return menu.Value.Query(us_id);
        }
    }
}
