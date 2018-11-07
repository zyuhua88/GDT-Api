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
    public class company
    {
        private string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private Zh.Tool.SqlHelper help = null;
        private Lazy<Entity.user_detail> user = new Lazy<Entity.user_detail>();
        private object obj = null;

        public company()
        {
            if (help==null) {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }

        /// <summary>
        /// 修改所在子公司的公司信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="com_id"></param>
        /// <returns></returns>
        public HttpResponseMessage Update_company(dynamic data,int com_id)
        {
            Entity.user_detail d= user.Value.SetData(data);
            string sql = "update company set com_name=@com_name,com_size=@com_size,work_address=@work_address " +
                "where com_id="+com_id+"";
            SqlParameter[] sp = {
                new SqlParameter("@com_name",d.com_name),
                new SqlParameter("@com_size",d.com_size),
                new SqlParameter("@work_address",d.work_address)
            };
            int i = help.Count(sql,sp);
            if (i > 0)
            {
                obj = new
                {
                    code = "A0000",
                    msg = "修改成功"
                };
            }
            else {
                obj = new {
                    code="A0001",
                    msg="修改失败"
                };
            }
            return help.ToJson(obj);
        }

        /// <summary>
        /// 添加我所在子公司中的部门信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponseMessage Add_deparment(dynamic data)
        {
            Entity.user_detail d = user.Value.SetData(data);
            string sql = "insert into deparment(b_name,b_work_name,b_work_desc,com_id) " +
                "values(@b_name,@b_work_name,@b_work_desc,"+d.com_id+")";
            SqlParameter[] sp = {
                new SqlParameter("@b_name",d.b_name),
                new SqlParameter("@b_work_name",d.b_work_name),
                new SqlParameter("@b_work_desc",d.b_work_desc)
            };
            int i = help.Count(sql,sp);
            if (i > 0)
            {
                obj = new
                {
                    code = "A0000",
                    msg = "部门添加成功"
                };
            }
            else {
                obj = new
                {
                    code = "A0001",
                    msg = "部门添加失败"
                };
            }
            return help.ToJson(obj);
        }

        /// <summary>
        /// 根据总部ID修改部门信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="b_id"></param>
        /// <returns></returns>
        public HttpResponseMessage Update_deparment(dynamic data,int b_id)
        {
            Entity.user_detail d = user.Value.SetData(data);
            string sql = "update deparment set b_name=@b_name,b_work_name=@b_work_name,b_work_desc=@b_work_desc " +
                "where b_id="+b_id;
            SqlParameter[] sp = {
                new SqlParameter("@b_name",d.b_name),
                new SqlParameter("@b_work_name",d.b_work_name),
                new SqlParameter("@b_work_desc",d.b_work_desc)
            };
            int i = help.Count(sql, sp);
            if (i > 0)
            {
                obj = new
                {
                    code = "A0000",
                    msg = "部门修改成功"
                };
            }
            else
            {
                obj = new
                {
                    code = "A0001",
                    msg = "部门修改失败"
                };
            }
            return help.ToJson(obj);
        }

        /// <summary>
        /// 查询部门下所有班级的信息及班组人员数量
        /// </summary>
        /// <param name="b_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryClasses(int b_id)
        {
            string csql = "select count(c_id) from classes where b_id="+b_id;
            int i =Convert.ToInt32(help.FirstRow(csql));
            string sql = "select a.*,(select count(c_id) from user_detail where b_id=a.b_id) as count from classes a where b_id="+b_id;
            DataTable dt = help.Totable(sql);
            List<Entity.user_detail> list= ConvertToEntity<Entity.user_detail>.Convert(dt);
            obj = new {
                code = "A0000",
                count=i,
                data=list
            };
            return help.ToJson(obj);
        }

        /// <summary>
        /// 查询公司下部门的信息及部门下的班级数量
        /// </summary>
        /// <param name="com_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryDeparment(int com_id)
        {
            string bc = "select count(b_id) from deparment where com_id="+com_id;
            int c =Convert.ToInt32(help.FirstRow(bc));

            string sql = "select *,(select count(c_id) from classes where b_id=a.b_id) as c_count,"+
                        "(select count(us_id) from user_detail where b_id = a.b_id ) as u_count "+
                        "from deparment a " +
                        "left join company b on a.com_id=b.com_id " +
                        "where a.com_id = "+com_id;
            DataTable dt = help.Totable(sql);
            obj = new {
                code="0",
                count=c,
                data=ConvertToEntity<Entity.user_detail>.Convert(dt)
            };
            return help.ToJson(obj);
        }

        /// <summary>
        /// 查询集团或企业下的所有部门信息 如果是系统管理员进入，则查询集团下的 否则查询子公司下的所有部门
        /// </summary>
        /// <param name="head_id"></param>
        /// <param name="com_id"></param>
        /// <param name="verify"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryDeparment(int head_id,int com_id, int verify)
        {
            string sql = "select * from deparment a "+
                        "left join company b " +
                        "left join head_office c " +
                        "on b.head_id = c.head_id " +
                        "on a.com_id = b.com_id " +
                        "where c.head_id = "+head_id+" ";
            if (verify>0 && com_id>0) {
                
                sql += " and b.com_id="+com_id+" ";
            }
            DataTable dt= help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "",
                    data = ConvertToEntity<Entity.classes>.Convert(dt)
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="没有查询到部门信息"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        




        /// <summary>
        /// 根据部门ID删除部门信息，同时删除该部门下的所有班组
        /// </summary>
        /// <param name="b_id"></param>
        /// <returns></returns>
        public HttpResponseMessage DelDeparment(int b_id)
        {
            string sql = "delete from deparment where b_id="+b_id+";";
            sql += "delete from classes where b_id=" + b_id;
            int i= help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = "0",
                    msg = "删除部门配置成功！"
                };
            }
            else {
                obj = new
                {
                    code = "0",
                    msg = "删除部门配置失败！"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 查询指定的部门详情
        /// </summary>
        /// <param name="b_id"></param>
        /// <returns></returns>
        public HttpResponseMessage Query_deparment(int b_id)
        {
            string sql = "select * from deparment where b_id="+b_id;
            DataTable dt= help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "成功",
                    data = ConvertToEntity<Entity.user_detail>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    msg = "没有查询到数据信息",
                    data = ""
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

    }
}