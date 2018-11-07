using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDT_API.Controllers.GDT.Controller
{
    public class HeadOfficeController : ApiController
    {

        private Lazy<Dal.Head_office> head = new Lazy<Dal.Head_office>();

        #region ===========================================集团信息操作相关方法

        /// <summary>
        /// 添加集团信息 成功返回受影响的行数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/addheadoffice")]
        public int AddHeadoffice(dynamic data)
        {
            return head.Value.AddHeadOffice(data);
        }
        /// <summary>
        /// 添加集团信息 成功返回新添加的集团ID
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/addhead")]
        public int AddHead(dynamic data)
        {
            return head.Value.AddHead(data);
        }

        /// <summary>
        /// 根据集团Id查询集团信息
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/queryheadoffice")]
        public HttpResponseMessage QueryHeadOffice(int head_id)
        {
            return head.Value.QueryHeadOffice(head_id);
        }


        /// <summary>
        /// 修改总部信息  包括公司logo
        /// </summary>
        /// <param name="data"></param>
        /// <param name="head_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/updatehead_office")]
        public HttpResponseMessage UpdateHead_office(dynamic data, int head_id)
        {
            return head.Value.UpdateHead_office(data,head_id);
        }

        #endregion

        #region  ====================================子公司信息操作方法相关
        /// <summary>
        /// 查询集团下的子公司列表
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/QueryCompany")]
        public HttpResponseMessage QueryCompany(int head_id,int com_id,int verify)
        {
            return head.Value.QueryCompany(head_id,com_id,verify);
        }

        /// <summary>
        /// 查询单个子公司的详情信息
        /// </summary>
        /// <param name="com_id">子公司唯一ID编号</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/querychildcompany")]
        public HttpResponseMessage QueryChildCompany(int com_id)
        {
            return head.Value.QueryChildCompany(com_id);
        }

        /// <summary>
        /// 添加子公司的方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/AddCompany")]
        public HttpResponseMessage AddCompany(dynamic data)
        {
            return head.Value.AddCompany(data);
        }

        /// <summary>
        /// 修改子公司信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="com_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/UpdateCompany")]
        public HttpResponseMessage UpdateCompany(dynamic data, int com_id)
        {
            return head.Value.UpdateCompany(data,com_id);
        }

        /// <summary>
        /// 删除子公司信息 同时删除其下部门及班级，并且调整用户的角色
        /// </summary>
        /// <param name="com_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/DelCompany")]
        public HttpResponseMessage DelCompany(int com_id)
        {
            return head.Value.DelCompany(com_id);
        }

        #endregion


        /// <summary>
        /// 查询集团下所有的班级信息，并按照部门进行分组
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/GetAllClass")]
        public HttpResponseMessage GetAllClass(int head_id)
        {
            return head.Value.GetAllClass(head_id);
        }

        /// <summary>
        /// 查询集团下所有的信息信息  可根据用户姓名进行筛选
        /// </summary>
        /// <param name="data"></param>
        /// <param name="head_id"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/GetAllUser")]
        public HttpResponseMessage GetAllUser(dynamic data, int head_id)
        {
            return head.Value.GetAllUser(data,head_id);
        }



        #region ==========================分配试题 =====================

        /// <summary>
        /// 已班级的形式进行分配试题
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/AddTestFoClass")]
        public HttpResponseMessage AddTestFoClass(dynamic data)
        {
            return head.Value.AddTestFoClass(data);
        }


        /// <summary>
        /// 已个人的形式进行分配试题
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/AddTestFoUser")]
        public HttpResponseMessage AddTestFoUser(dynamic data)
        {
            return head.Value.AddTestFoUser(data) ;
        }

        /// <summary>
        /// 一键为集团所有人分配试题
        /// </summary>
        /// <param name="head_id"></param>
        /// <param name="testid"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/AddTestFoHead")]
        public HttpResponseMessage AddTestFoHead(int head_id, int testid)
        {
            return head.Value.AddTestFoHead(head_id,testid);
        }




        /// <summary>
        /// 查询厂级、部门、班组分组数据列表
        /// </summary>
        /// <param name="head_id"></param>
        /// <param name="verify"></param>
        /// <param name="com_id"></param>
        /// <param name="b_id"></param>
        /// <param name="c_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/headoffice/gdt/DataList")]
        public HttpResponseMessage DataList(int head_id, int verify, int com_id, int b_id, int c_id)
        {
            return head.Value.DataList(head_id,verify,com_id,b_id,c_id);
        }

        #endregion
    }
}
