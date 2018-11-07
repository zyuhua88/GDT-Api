using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;

namespace GDT_API.Controllers.GDT.Dal
{
    public class Deparment
    {
        private string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private Zh.Tool.SqlHelper help = null;
        private Lazy<Entity.user_detail> user = new Lazy<Entity.user_detail>();
        private object obj = null;

        public Deparment()
        {
            if (help==null) {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }
        /// <summary>
        /// 根据部门id查询班组列表
        /// </summary>
        /// <param name="b_id"></param>
        /// <returns></returns>
        public HttpResponseMessage Query(int b_id)
        {
            string sql = "select b.*,(select count(a.us_id) from user_detail a where a.c_id=b.c_id ) as u_count from classes b " +
                "where b.b_id="+b_id+"";
            DataTable dt= help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "读取列表成功",
                    data = ConvertToEntity<Entity.user_detail>.Convert(dt)
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="没有数据",
                    data=""
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 根据班组ID获取班组详细信息
        /// </summary>
        /// <param name="c_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryClass(int c_id)
        {
            string sql = "select * from classes where c_id=" + c_id;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "读取列表成功",
                    data = ConvertToEntity<Entity.user_detail>.Convert(dt)
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "没有数据",
                    data = ""
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 根据部门ID查询部门下所有的员工信息
        /// </summary>
        /// <param name="b_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryUser(int c_id)
        {
            string sql = "select a.us_id, real_name from user_detail a "+
                        "left join users b " +
                        "on a.us_id = b.us_id " +
                        "where a.b_id in ( " +
                        "select b_id from classes where c_id = "+c_id+") " +
                        "and a.verify < 4 ";

            DataTable dt = help.Totable(sql);
            obj = new {
                code = 0,
                msg = "",
                data = ConvertToEntity<Entity.user_detail>.Convert(dt)
            };
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 向上查询领导的US_ID信息 直到最高级别用户
        /// </summary>
        /// <param name="b_id"></param>
        /// <returns></returns>
        public int QueryAdminId(int b_id)
        {
            //查询出申报人员所在的部门领导
            string sql = "select * from user_detail "+
                        "where b_id = "+b_id+" and verify = 2";
            DataTable dt = help.Totable(sql);
            int adminid = 0;
            if (dt.Rows.Count > 0) {//如果部门的管理员存在
                adminid = Convert.ToInt32(dt.Rows[0]["us_id"]);
            }
            else {
                sql = "select us_id from user_detail where com_id in( "+
                     "select com_id from deparment where b_id = "+b_id+") and verify = 1";
                DataTable dtt = help.Totable(sql);
                if (dtt.Rows.Count > 0)//如果子公司管理员存在
                {
                    adminid = Convert.ToInt32(dtt.Rows[0]["us_id"]);
                }
                else {//部门及子公司都没有设置管理员。查询最高级别领导
                    sql = "select us_id from user_detail where head_id in ( "+
                            "select b.head_id from deparment a " +
                            "left join company b " +
                            "on a.com_id = b.com_id " +
                            "where a.b_id = "+b_id+") and verify = 0";
                    DataTable dttt = help.Totable(sql);
                    adminid = Convert.ToInt32(dttt.Rows[0]["us_id"]);
                }
            }
            return adminid;
        }

        /// <summary>
        /// 添加班组信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponseMessage AddClass(dynamic data)
        {
            Entity.user_detail us = user.Value.SetData(data);
            string sql = "insert into classes(c_name,c_work_name,c_work_desc,b_id) " +
                "values('"+us.c_name+"','"+us.c_work_name+"','"+us.c_work_desc+"',"+us.b_id+")";
            int i= help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "添加班组成功"
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    msg = "添加班组失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 修改班组信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="c_id"></param>
        /// <returns></returns>
        public HttpResponseMessage UpdateClass(dynamic data,int c_id)
        {
            Entity.user_detail us = user.Value.SetData(data);
            string sql = "update classes set c_name='" + us.c_name + "',c_work_name='" + us.c_work_name + "'," +
                "c_work_desc='" + us.c_work_desc + "',b_id=" + us.b_id + " where c_id="+c_id;

            int i = help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "修改班组信息成功"
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "修改班组信息失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 删除班组信息
        /// </summary>
        /// <param name="c_id"></param>
        /// <returns></returns>
        public HttpResponseMessage DelClass(int c_id)
        {
            string sql = "delete from classes where c_id="+c_id;
            int i = help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "删除班组信息成功"
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "删除班组信息失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

    }
}