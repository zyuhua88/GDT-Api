using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Http;
using System.Data;
using System.Data.SqlClient;

namespace GDT_API.Controllers.GDT.Dal
{
    public class Train_Project
    {
        private Lazy<Entity.projectlist> p = new Lazy<Entity.projectlist>();
        private Zh.Tool.SqlHelper help = null;

        private string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private object obj = null;

        public Train_Project()
        {
            if (help==null) {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }


        


        /// <summary>
        /// 添加培训计划
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(dynamic data)
        {
            Entity.projectlist t = p.Value.SetData(data);
            string sql = "insert into train_project values('"+t.t_name + "',"+t.start_time+","+t.end_time+",'"+ t.score_name + "'," +
                "'" + t.b_id + "'," + t.c_id + "," + t.head_id+","+t.com_id+","+t.time_length + ",'"+ t.score_type + "','"+t.descs + "',"+t.create_time + "," +
                "" + t.us_id + "," + t.work_usid + ")";
            return help.Count(sql);
        }

        /// <summary>
        /// 添加计划执行记录表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int AddProJectList(dynamic data)
        {
            Entity.projectlist t = p.Value.SetData(data);
            string sql = "insert into projectlist values("+t.t_id+","+ t.create_time + ","+t.work_time + ",'"+t.project_body + "'," +
                ""+t.work_usid + ",'"+t.l_desc + "',"+t.l_us_id + ",'"+t.work_length+"')";
            return help.Count(sql);
        }


        /// <summary>
        /// 修改培训计划信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="t_id"></param>
        /// <returns></returns>
        public int Update(dynamic data,int t_id)
        {
            Entity.projectlist t = p.Value.SetData(data);
            string sql = "update train_project set t_name='" + t.t_name + "',start_time=" + t.start_time + ",end_time=" + t.end_time + "," +
                "score_name='" + t.score_name + "'," +
                "head_id=" + t.head_id + ",com_id=" + t.com_id + ",b_id='" + t.b_id + "',c_id=" + t.c_id + ",time_length=" + t.time_length + "," +
                "score_type='" + t.score_type + "',descs='" + t.descs + "',create_time=" + t.create_time + "," +
                "work_usid=" + t.work_usid + ",us_id=" + t.us_id + " where t_id="+t_id;
            return help.Count(sql);
        }

        

        /// <summary>
        /// 删除培训计划信息
        /// </summary>
        /// <param name="t_id"></param>
        /// <returns></returns>
        public int Del(int t_id)
        {
            string sql = "delete from train_project where t_id="+t_id+";";
            sql += "delete from projectlist where t_id="+t_id;
            return help.Count(sql);
        }

        /// <summary>
        /// 删除指定的培训列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelProJectList(int id)
        {
            string sql = "delete from projectlist where id="+id;
            return help.Count(sql);
        }


        /// <summary>
        /// 查询培训计划列表
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        public HttpResponseMessage Query(int head_id,string value)
        {
            string where = "";
            if (value!=null && value!="") {
                where += " and (a.t_name like '%"+value+"%' or a.descs like '%"+value+"%') ";
            }
            string sql = "select a.*,(select count(d.id) from projectlist d where d.t_id=a.t_id) as count, "+
                        "b.real_name as real_name,c.real_name as work_name " +
                        "from train_project a " +
                        "left join users b on a.us_id = b.us_id " +
                        "left join users c on a.work_usid = c.us_id " +
                        "where a.head_id = "+head_id+" "+where;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.projectlist>.Convert(dt)
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="nob"
                };
            }
            return Zh.Tool.Json.GetJson(obj);

        }

        /// <summary>
        /// 查询单条数据信息
        /// </summary>
        /// <param name="t_id"></param>
        /// <returns></returns>
        public HttpResponseMessage Query(int t_id)
        {
            string sql = "select * from train_project where t_id="+t_id;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.train_project>.Convert(dt)
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="nob"
                };
            }

            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询执行计划的列表
        /// </summary>
        /// <param name="t_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryProjectlist(int t_id)
        {
            string sql = "select a.*,b.t_name,c.real_name as work_name,d.real_name as real_name "+
                        "from projectlist a left " +
                        "join train_project b on a.t_id = b.t_id left " +
                        "join users c on a.work_usid = c.us_id " +
                        "left join users d on a.l_us_id = d.us_id " +
                        "where a.t_id="+t_id;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.projectlist>.Convert(dt)
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="nob"
                };
            }
            
            return Zh.Tool.Json.GetJson(obj);
        }




        /// <summary>
        /// 查询集团下所有公司和部门的分组信息
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryDeparment(int head_id)
        {
            string sql1 = "select com_id,com_name from company where head_id="+head_id+";select com_id,b_id,b_name from deparment";
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(sql1, conn);
            da.Fill(ds);
            //这里是对datarelation的应用 分组管理
            DataRelation dr = new DataRelation("table", ds.Tables[0].Columns["com_id"], ds.Tables[1].Columns["com_id"]);
            ds.Relations.Add(dr);

            List<ComAnddeparment> list = new List<ComAnddeparment>();
            
            foreach (DataRow item in ds.Tables["table"].Rows) {
                List<Dars> delist = new List<Dars>();
                foreach (var item1 in item.GetChildRows(dr)) {
                    delist.Add(new Dars() {
                        com_id = Convert.ToInt32(item1["com_id"]),
                        b_id = Convert.ToInt32(item1["b_id"]),
                        b_name = item1["b_name"].ToString()
                    });
                }
                list.Add(new ComAnddeparment()
                {
                    com_id = Convert.ToInt32(item["com_id"]),
                    com_name = item["com_name"].ToString(),
                    list=delist
                });
            }

            obj = new {
                code = 0,
                data = list
            };
            return Zh.Tool.Json.GetJson(obj);

        }

        /// <summary>
        /// 根据集团ID查询集团下所有部门id  及部门名称 name
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryAllDeparment(int head_id)
        {
            string sql = "select a.b_id,a.b_name from  deparment a "+
                        "left join company b " +
                        "on a.com_id = b.com_id " +
                        "where b.head_id = "+head_id;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Dars>.Convert(dt)
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="nob"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询集团下所有管理员包括班组长
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryAllHead(int head_id,int com_id)
        {
            string sql = "select a.us_id,b.real_name,a.verify,a.com_id,a.b_id from user_detail a " +
                "left join users b on a.us_id=b.us_id where a.verify<4 and a.head_id="+head_id+" and a.com_id="+com_id;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.user_detail>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code = 0,
                    msg="null"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }
    }

    public class ComAnddeparment
    {
        public int com_id { get; set; }
        public string com_name { get; set; }
        public List<Dars> list { get; set; }
    }

    public class Dars
    {
        public int com_id { get; set; }
        public int b_id { get; set; }
        public string b_name { get; set; }
    }
}