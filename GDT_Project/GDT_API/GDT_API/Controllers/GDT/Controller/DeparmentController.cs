using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;

namespace GDT_API.Controllers.GDT.Controller
{
    public class DeparmentController : ApiController
    {
        private Lazy<Dal.Deparment> depar = new Lazy<Dal.Deparment>();

        /// <summary>
        /// 根据部门id查询班组列表
        /// </summary>
        /// <param name="b_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/deparment/gdt/query")]
        public HttpResponseMessage Query(int b_id)
        {
            return depar.Value.Query(b_id);
        }

        /// <summary>
        /// 根据班组ID获取班组详细信息
        /// </summary>
        /// <param name="c_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/deparment/gdt/queryclass")]
        public HttpResponseMessage QueryClass(int c_id)
        {
            return depar.Value.QueryClass(c_id);
        }


        /// <summary>
        /// 根据部门ID查询部门下所有的员工信息
        /// </summary>
        /// <param name="b_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/deparment/gdt/QueryUser")]
        public HttpResponseMessage QueryUser(int c_id)
        {
            return depar.Value.QueryUser(c_id);
        }


        /// <summary>
        /// 添加班组信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/deparment/gdt/addclass")]
        public HttpResponseMessage AddClass(dynamic data)
        {
            return depar.Value.AddClass(data);
        }

        /// <summary>
        /// 修改班组信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="c_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/deparment/gdt/updateclass")]
        public HttpResponseMessage UpdateClass(dynamic data, int c_id)
        {
            return depar.Value.UpdateClass(data,c_id);
        }

        /// <summary>
        /// 删除班组信息
        /// </summary>
        /// <param name="c_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/deparment/gdt/delclass")]
        public HttpResponseMessage DelClass(int c_id)
        {
            return depar.Value.DelClass(c_id);
        }
    }
}
