using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDT_API.Controllers.GDT.Controller
{
    public class Train_ProjectController : ApiController
    {
        private Lazy<Dal.Train_Project> p = new Lazy<Dal.Train_Project>();


        /// <summary>
        /// 添加培训计划
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Train_project/gdt/Add")]
        public int Add(dynamic data)
        {
            return p.Value.Add(data);
        }


        /// <summary>
        /// 添加计划执行记录表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Train_project/gdt/AddProJectList")]
        public int AddProJectList(dynamic data)
        {
            return p.Value.AddProJectList(data);
        }


        /// <summary>
        /// 修改培训计划信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="t_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Train_project/gdt/Update")]
        public int Update(dynamic data, int t_id)
        {
            return p.Value.Update(data,t_id);
        }


        /// <summary>
        /// 删除培训计划信息
        /// </summary>
        /// <param name="t_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Train_project/gdt/Del")]
        public int Del(int t_id) {
            return p.Value.Del(t_id);
        }


        /// <summary>
        /// 查询培训计划列表
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Train_project/gdt/Query")]
        public HttpResponseMessage Query(int head_id, string value)
        {
            return p.Value.Query(head_id,value);
        }


        /// <summary>
        /// 查询单条数据信息
        /// </summary>
        /// <param name="t_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Train_project/gdt/Query")]
        public HttpResponseMessage Query(int t_id)
        {
            return p.Value.Query(t_id);
        }


        /// <summary>
        /// 查询执行计划的列表
        /// </summary>
        /// <param name="t_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Train_project/gdt/QueryProjectlist")]
        public HttpResponseMessage QueryProjectlist(int t_id)
        {
            return p.Value.QueryProjectlist(t_id);
        }


        /// <summary>
        /// 查询集团下所有公司和部门的分组信息
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Train_project/gdt/QueryDeparment")]
        public HttpResponseMessage QueryDeparment(int head_id)
        {
            return p.Value.QueryDeparment(head_id);
        }

        /// <summary>
        /// 根据集团ID查询集团下所有部门id  及部门名称 name
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Train_project/gdt/QueryAllDeparment")]
        public HttpResponseMessage QueryAllDeparment(int head_id)
        {
            return p.Value.QueryAllDeparment(head_id);
        }


        /// <summary>
        /// 查询集团下所有管理员包括班组长
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Train_project/gdt/QueryAllHead")]
        public HttpResponseMessage QueryAllHead(int head_id, int com_id)
        {
            return p.Value.QueryAllHead(head_id,com_id);
        }
    }
}
