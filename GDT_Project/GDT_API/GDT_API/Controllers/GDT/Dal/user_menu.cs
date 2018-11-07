using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Configuration;
using System.Data;


namespace GDT_API.Controllers.GDT.Dal
{
    public class user_menu
    {
        private string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private object obj = null;
        private Zh.Tool.SqlHelper help = null;
        private Lazy<Entity.user_menu> menu = new Lazy<Entity.user_menu>();

        public user_menu()
        {
            if (help==null) {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }

        /// <summary>
        /// 添加或都修改用户的模块权限信息  如果用户权限存在，则修改
        /// </summary>
        /// <param name="data"></param>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage Add(dynamic data,int us_id)
        {
            //查询是否为该用户添加了页面模块信息
            string sql = "select count(id) from user_menu where us_id="+us_id;
            if (Convert.ToInt32(help.FirstRow(sql)) > 0)
            {
                return Update(data, us_id);
            }
            else {
                string menu = data.menu;
                sql = "insert into user_menu(us_id,menu) values("+us_id+",'"+menu+"')";
                if (help.Count(sql) > 0)
                {
                    obj = new
                    {
                        code = 0,
                        msg = "ok"
                    };
                }
                else {
                    obj = new
                    {
                        code = 1,
                        msg = "error"
                    };
                }
                return Zh.Tool.Json.GetJson(obj);
            }
        }

        public HttpResponseMessage Update(dynamic data,int us_id)
        {
            string menu = data.menu;
            string sql = "update user_menu set menu='"+menu+"' where us_id="+us_id;
            if (help.Count(sql) > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "ok"
                };
            }
            else {
                obj = new
                {
                    obj=1,
                    msg="error"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询单个用户的模块权限信息
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage Query(int us_id)
        {
            string sql = "select * from user_menu where us_id="+us_id;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.user_menu>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    msg = "row count 0"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }
        
    }
}