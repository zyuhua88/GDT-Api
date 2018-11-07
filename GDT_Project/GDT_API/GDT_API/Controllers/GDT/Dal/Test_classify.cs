using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace GDT_API.Controllers.GDT.Dal
{
    public class Test_classify
    {
        private string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private Zh.Tool.SqlHelper help = null;
        private Lazy<Entity.test> test = new Lazy<Entity.test>();
        private object obj = null;

        public Test_classify()
        {
            if (help == null)
            {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }

        /// <summary>
        /// 上传试题分类名称
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponseMessage Add(dynamic data)
        {
            Entity.test t = test.Value.SetData(data);
            string sql = "insert into test_classify(t_name,create_time,us_id,head_id,type_a,type_b,type_c,type_d,is_send,defen_a,defen_b," +
                "defen_c,defen_d,t_types) values(" +
                "@t_name,"+t.create_time+","+t.us_id+","+t.head_id+","+t.type_a+ "," + t.type_b + "," + t.type_c + "," + t.type_d + "," +
                "0,"+t.defen_a+","+t.defen_b+","+t.defen_c+","+t.defen_d+","+t.t_types+" )";
            SqlParameter[] sp = {
                new SqlParameter("@t_name",t.t_name)
            };
            int i = help.Count(sql,sp);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "上传成功"
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    msg = "上传失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 修改题库名称信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id">对应题库ID编号</param>
        /// <returns></returns>
        public HttpResponseMessage Update(dynamic data,int id)
        {
            Entity.test t = test.Value.SetData(data);
            string sql = "update test_classify set t_name=@t_name,us_id=" + t.us_id + "," +
                "head_id=" + t.head_id + ",type_a=" + t.type_a + ",type_b=" + t.type_b + ",type_c=" + t.type_c + "," +
                "type_d=" + t.type_d + ",defen_a="+t.defen_a+",defen_b="+t.defen_b+",defen_c="+t.defen_c+",defen_d="+t.defen_d+",t_types="+t.t_types+" where id="+id;
            SqlParameter[] sp = {
                new SqlParameter("@t_name",t.t_name)
            };
            int i = help.Count(sql,sp);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "修改成功"
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "修改失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 更变试题的发布状态 
        /// </summary>
        /// <param name="id">题库ID编号</param>
        /// <param name="is_send">0 为收回发布 1 发布给学员</param>
        /// <returns></returns>
        public HttpResponseMessage SendTest(int id, int is_send)
        {
            string sql = "update test_classify set is_send="+is_send+" where id="+id;
            int i = help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "发布状态已改变"
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "在更尽管发布状态时出现问题"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 删除指定的题库信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Del(int id)
        {
            string sql = "delete from test_classify where id=" + id + ";";
            sql += "delete from test where classify_id=" + id;
            int i = help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "删除成功"
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "删除失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 删除指定的题库信息  多选择删除
        /// </summary>
        /// <param name="ids">格式为1,2,3,4,5</param>
        /// <returns></returns>
        public HttpResponseMessage Del(string ids)
        {
            string sql = "delete from test_classify where id in (" + ids+")";
            int i = help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "删除成功"
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "删除失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询指定的题库信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Query(int id)
        {
            string sql = "select * from test_classify where id="+id;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.test>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    msg="没有查询到数据信息"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询集团内部发部的题库总数量
        /// </summary>
        /// <param name="head_id"></param>
        /// <param name="count"></param>
        /// <returns>返回list<int>类型</returns>
        public List<int> QueryCount(int head_id,int count)
        {
            List<int> list = new List<int>();
            string sql = "select count(*) from test_classify where head_id="+head_id;
            object objs = help.FirstRow(sql);
            int testCount = 0;//预定义查询到的题总数
            int page = 0;//预定义查询到了总页数
            if (objs != null)
            {
                testCount = Convert.ToInt32(objs);
                if (testCount % count > 0)
                {
                    page = Convert.ToInt32(testCount / count) + 1;
                }
                else
                {
                    page = testCount / count;
                }
            }
            else {
                testCount = 0;
                page = 0;
            }
            list.Add(page);//总页数据
            list.Add(testCount);//总数据量
            return list;
        }

        /// <summary>
        /// 查询集团内部发布的题库列表
        /// </summary>
        /// <param name="head_id"></param>
        /// <param name="pages"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public HttpResponseMessage Query(int head_id,int pages,int count)
        {
            int p = (pages - 1) * count + 1;
            int c = pages * count;
            List<int> list = QueryCount(head_id,count);
            int page = list[0];//总页数
            int testCount = list[1];//总数量
            string sql = "select * from( "+
                        "select a.*,(select count(b.id) from test b where b.classify_id=a.id) as tcount, " +
                        "row_number() over(order by a.id desc) as row from test_classify a " +
                        "where a.head_id = "+head_id+") temp " +
                        "where row between "+p+" and "+c+" ";
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    page = page,
                    testCount = testCount,
                    data = ConvertToEntity<Entity.test>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    page = page,
                    testCount = testCount,
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }
    }
}