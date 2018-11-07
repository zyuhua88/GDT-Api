using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDT_API.Controllers.GDT.Controller
{
    public class UsersController : ApiController
    {
        private Lazy<Dal.users> users = new Lazy<Dal.users>();


        /// <summary>
        /// 添加用户的登录信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/users/gdt/addusers")]
        public HttpResponseMessage AddUsers(dynamic data)
        {
            return users.Value.AddUser(data);
        }


        /// <summary>
        /// 添加用户的的详细信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="usid"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/users/gdt/adduser_detail")]
        public HttpResponseMessage AddUser_detail(dynamic data,int usid)
        {
            return users.Value.AddUser_detail(data,usid);
        }

        /// <summary>
        /// 筛选员工信息 查询列表
        /// </summary>
        /// <param name="head_id"></param>
        /// <param name="com_id"></param>
        /// <param name="b_id"></param>
        /// <param name="c_id"></param>
        /// <param name="value"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/users/gdt/queryuserlist")]
        public HttpResponseMessage QuerUserList(int head_id, int com_id, int b_id, int c_id, string value,int page,int count,int verify,
            int mycom_id, int myb_id, int myc_id)
        {
            return users.Value.QueryUserList(head_id,com_id,b_id,c_id,value,page,count,verify,mycom_id,myb_id,myc_id);
        }

        /// <summary>
        /// 查询单个用户的详细信息
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/users/gdt/queryuser")]
        public HttpResponseMessage QuerUser(int us_id)
        {
            return users.Value.QueryUser(us_id);
        }

        /// <summary>
        /// 删除指定用户
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/users/gdt/deluser_detail")]
        public HttpResponseMessage DelUser_detail(int us_id)
        {
            return users.Value.DelUser_detail(us_id);
        }

        /// <summary>
        /// 删除指定用户  多用户删除
        /// </summary>
        /// <param name="us_id">格式为1,2,3,4,5</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/users/gdt/deluserall")]
        public HttpResponseMessage DelUserAll(string us_ids)
        {
            return users.Value.DelUserAll(us_ids);
        }

        /// <summary>
        /// 修改登录信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/users/gdt/changeusers")]
        public HttpResponseMessage ChangeUsers(dynamic data, int us_id)
        {
            return users.Value.ChangeUsers(data,us_id);
        }

        /// <summary>
        /// 修改用户的权限、详情等信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/users/gdt/changeuser_detail")]
        public HttpResponseMessage ChangeUser_detail(dynamic data, int us_id)
        {
            return users.Value.ChangeUser_detail(data,us_id);
        }


        /// <summary>
        /// 查询用户的登录日志信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/users/gdt/QueryLoginLog")]
        public HttpResponseMessage QueryLoginLog(int head_id, int page, int count)
        {
            return users.Value.QueryLoginLog(head_id, page,count);
        }


        /// <summary>
        /// 添加用户的在线时长
        /// </summary>
        /// <param name="us_id"></param>
        /// 
        [HttpPost]
        [Route("api/users/gdt/on_line")]
        public void on_line(int us_id)
        {
            users.Value.on_line(us_id);
        }

        /// <summary>
        /// 通过用户名修改用户的密码
        /// </summary>
        /// <param name="login_name"></param>
        /// <param name="new_pwd"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/users/gdt/ChangePwd")]
        public int ChangePwd(string username, string tel, string pwd)
        {
            return users.Value.ChangePwd(username,tel,pwd);
        }


        /// <summary>
        /// 查询用户的在线总时长
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/users/gdt/Query_online")]
        public int Query_online(int us_id)
        {
            return users.Value.Query_online(us_id);
        }
    }
}
