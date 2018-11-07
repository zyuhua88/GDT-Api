using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDT_API.Controllers.GDT.Controller
{
    public class YinHuanController : ApiController
    {
        private Lazy<Dal.yinhuan> y = new Lazy<Dal.yinhuan>();
        /// <summary>
        /// 添加隐患信息表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/yinhuan/gdt/addyinhuan")]
        public HttpResponseMessage AddYinHuan(dynamic data)
        {
            return y.Value.AddYinHuan(data);
        }

        /// <summary>
        /// 添加审批上报表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/yinhuan/gdt/addyinhuan_up")]
        public HttpResponseMessage AddYinHuan_up(dynamic data,int touser)
        {
            return y.Value.AddYinHuan_up(data,touser);
        }

        /// <summary>
        /// 上传整改表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/addyinhuan_down")]
        public HttpResponseMessage AddYinHuan_down(dynamic data)
        {
            return y.Value.AddYinHuan_down(data);
        }

        /// <summary>
        /// 修改隐患信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="yh_no"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/UpdateYinHuan")]
        public HttpResponseMessage UpdateYinHuan(dynamic data, string yh_no)
        {
            return y.Value.UpdateYinHuan(data,yh_no);
        }

        /// <summary>
        /// 修改审批信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="yh_upid"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/UpdateYinHuan_up")]
        public HttpResponseMessage UpdateYinHuan_up(dynamic data, int yh_upid)
        {
            return y.Value.UpdateYinHuan_up(data,yh_upid);
        }

        /// <summary>
        /// 通过并转发给相关实施负责人
        /// </summary>
        /// <param name="yh_no"></param>
        /// <param name="yh_to_userdown">要通知的整改负责人ID</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/UpdateYinHuan_up")]
        public HttpResponseMessage UpdateYinHuan_up(string yh_no, int yh_to_userdown)
        {
            return y.Value.UpdateYinHuan_up(yh_no,yh_to_userdown);
        }

        /// <summary>
        /// 修改整改信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="yh_downid"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/UpdateYinHuan_down")]
        public HttpResponseMessage UpdateYinHuan_down(dynamic data, int yh_downid)
        {
            return y.Value.UpdateYinHuan_down(data,yh_downid);
        }


        /// <summary>
        /// 修改鉴定信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="yh_no"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/UpdateJianDing")]
        public HttpResponseMessage UpdateJianDing(dynamic data, string yh_no)
        {
            return y.Value.UpdateJianDing(data,yh_no);
        }

        /// <summary>
        /// 删除我上报的隐患表单 同时删除已上报和已下发的表单
        /// </summary>
        /// <param name="yh_no"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/DelYinHuan")]
        public HttpResponseMessage DelYinHuan(string yh_no)
        {
            return y.Value.DelYinHuan(yh_no);
        }

        /// <summary>
        /// 撤消我上报的隐患表单 （删除已上报和已下发的表单）并修改表单状态数据为 2 已撤消
        /// </summary>
        /// <param name="yh_no"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/BackYinHuan")]
        public HttpResponseMessage BackYinHuan(string yh_no)
        {
            return y.Value.BackYinHuan(yh_no);
        }


        /// <summary>
        /// 查询隐患信息 只限于查询已发布的和已完成的 也就是说send_state==1 或者==3
        /// </summary>
        /// <param name="data">data里携带一个值value为用户检索的数据信息,userinfo用户的信息，是一个｛｝object类型</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/QuerySend")]
        public HttpResponseMessage QuerySend(dynamic data, int page, int count, int yh_send_state)
        {
            return y.Value.QuerySend(data,page,count,yh_send_state);
        }


        /// <summary>
        /// 查询隐患信息 只限于查询未发布的和已撤消的 也就是说send_state==0 或者==2
        /// </summary>
        /// <param name="data">data里携带一个值value为用户检索的数据信息,userinfo用户的信息，是一个｛｝object类型</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/QueryNoSend")]
        public HttpResponseMessage QueryNoSend(dynamic data, int page, int count, int yh_send_state)
        {
            return y.Value.QueryNoSend(data,page,count,yh_send_state);
        }

        /// <summary>
        /// 查询需要我整改的项目
        /// </summary>
        /// <param name="data">data.value,data.userInfo</param>
        /// <param name="state">处理状态</param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/QueryDown")]
        public HttpResponseMessage QueryDown(dynamic data, int state, int page, int count)
        {
            return y.Value.QueryDown(data,state,page,count);
        }


        /// <summary>
        /// 查询需要我整改的隐患单
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/QueryDown")]
        public HttpResponseMessage QueryDown(int us_id, int page, int count)
        {
            return y.Value.QueryDown(us_id,page,count);
        }


        /// <summary>
        /// 查询需要我审批的项目
        /// </summary>
        /// <param name="data">data.value,data.userInfo</param>
        /// <param name="state">审批状态</param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/QueryUp")]
        public HttpResponseMessage QueryUp(dynamic data, int state, int page, int count)
        {
            return y.Value.QueryUp(data,state,page,count);
        }


        /// <summary>
        /// 查询需要我审批的隐患单
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/QueryUp")]
        public HttpResponseMessage QueryUp(int us_id, int page, int count)
        {
            return y.Value.QueryUp(us_id,page,count);
        }


        /// <summary>
        /// 查询单条隐患信息
        /// </summary>
        /// <param name="yh_no"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yinhuan/gdt/Query")]
        public HttpResponseMessage Query(string yh_no)
        {
            return y.Value.Query(yh_no);
        }
    }
}
