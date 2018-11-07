using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDT_API.Controllers.GDT.Controller
{
    public class CompanyController : ApiController
    {
        private Lazy<Dal.company> com = new Lazy<Dal.company>();


        /// <summary>
        /// 查询子公司下的部门列表
        /// </summary>
        /// <param name="com_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/company/gdt/querydeparment")]
        public HttpResponseMessage QueryDeparment(int com_id)
        {
            return com.Value.QueryDeparment(com_id);
        }

        /// <summary>
        /// 查询集团或企业下的所有部门信息 如果是系统管理员进入，则查询集团下的 否则查询子公司下的所有部门
        /// </summary>
        /// <param name="head_id"></param>
        /// <param name="com_id"></param>
        /// <param name="verify"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/company/gdt/querydeparment")]
        public HttpResponseMessage QueryDeparment(int head_id, int com_id, int verify)
        {
            return com.Value.QueryDeparment(head_id,com_id,verify);
        }

        /// <summary>
        /// 添加我所在子公司中的部门信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/company/gdt/add_deparment")]
        public HttpResponseMessage Add_deparment(dynamic data)
        {
            return com.Value.Add_deparment(data);
        }

        /// <summary>
        /// 根据总部ID修改部门信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="b_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/company/gdt/update_deparment")]
        public HttpResponseMessage Update_deparment(dynamic data, int b_id)
        {
            return com.Value.Update_deparment(data,b_id);
        }

        /// <summary>
        /// 根据部门ID删除部门信息，同时删除该部门下的所有班组
        /// </summary>
        /// <param name="b_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/company/gdt/deldeparment")]
        public HttpResponseMessage DelDeparment(int b_id)
        {
            return com.Value.DelDeparment(b_id);
        }

        /// <summary>
        /// 查询指定的部门详情
        /// </summary>
        /// <param name="b_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/company/gdt/query_deparment")]
        public HttpResponseMessage Query_deparment(int b_id)
        {
            return com.Value.Query_deparment(b_id);
        }
    }
}
