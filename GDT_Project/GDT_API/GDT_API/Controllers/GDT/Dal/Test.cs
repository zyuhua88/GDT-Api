using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Configuration;
using System.Data;
using Newtonsoft.Json.Linq;

namespace GDT_API.Controllers.GDT.Dal
{

    public class Test
    {
        private string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private Zh.Tool.SqlHelper help = null;
        private Lazy<Entity.test> test = new Lazy<Entity.test>();
        private object obj = null;

        public Test()
        {
            if (help==null) {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }
        public HttpResponseMessage Add(dynamic data)
        {
            return Zh.Tool.Json.GetJson(obj);
        }

        //客户文件上传 读取excel文件并写入到数据库中
        public string Upload_file(HttpPostedFile file,int classify_id)
        {
            string obj = "{\"code\": 0,\"msg\": \"\",\"data\": {\"src\": \"http://cdn.layui.com/123.jpg\"}} ";
            string msg = "";
            try
            {
                string type = file.FileName.Substring(file.FileName.LastIndexOf("."), file.FileName.Length - file.FileName.LastIndexOf("."));
                string nname = "";
                if (type == ".xls" || type == ".xlsx")
                {
                    nname = DateTime.Now.ToFileTime() + type;
                }
                else
                {
                    return obj = "{\"code\": 1,\"msg\": \"文件格式不正确\",\"data\": {\"src\": \"\"}} ";
                    
                }

                if (Zh.Tool.File_Tool.File_Upload(file.InputStream, AppDomain.CurrentDomain.BaseDirectory + "/Files/" + nname, out msg))
                {
                    DataTable dt = Zh.Tool.Excel_Tool.ReadExcel(AppDomain.CurrentDomain.BaseDirectory + "/Files/" + nname);
                    string sql = "insert into test(classify_id,t_type,t_desc,t_title,t_a,t_b,t_c,t_d,t_e," +
                        "t_f,daan)";
                    if (dt.Columns.Count < 10)
                    {
                        msg = "表格模版不正确";
                        return obj = "{\"code\": 1,\"msg\": \"表格模版不正确\",\"data\": {\"src\": \"\"}} ";

                    }
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (i == 0)
                        {
                            sql += " values("+classify_id+",'"+dt.Rows[i][0]+ "','" + dt.Rows[i][1] + "','" + dt.Rows[i][2] + "'," +
                                "'" + dt.Rows[i][3] + "','" + dt.Rows[i][4] + "','" + dt.Rows[i][5] + "','" + dt.Rows[i][6] + "'," +
                                "'" + dt.Rows[i][7] + "','" + dt.Rows[i][8] + "','" + dt.Rows[i][9] + "') ";
                        }
                        else
                        {
                            sql += " ,(" + classify_id + ",'" + dt.Rows[i][0] + "','" + dt.Rows[i][1] + "','" + dt.Rows[i][2] + "'," +
                                "'" + dt.Rows[i][3] + "','" + dt.Rows[i][4] + "','" + dt.Rows[i][5] + "','" + dt.Rows[i][6] + "'," +
                                "'" + dt.Rows[i][7] + "','" + dt.Rows[i][8] + "','" + dt.Rows[i][9] + "') ";
                        }
                    }
                    int ii = help.Count(sql);
                    if (ii > 0)
                    {
                        return obj = "{\"code\": 0,\"msg\": \"数据导入成功，成功上传"+dt.Rows.Count+"条试题信息！\",\"data\": {\"src\": \"\"}} ";
                    }
                    else {
                        return obj = "{\"code\": 1,\"msg\": \"数据导入失败\",\"data\": {\"src\": \"\"}} ";
                    }
                    
                    
                }
                else
                {
                    return obj = "{\"code\": 1,\"msg\": \"在上传文件时出错\",\"data\": {\"src\": \"\"}} ";
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return obj = "{\"code\": 1,\"msg\": \""+msg+"\",\"data\": {\"src\": \"\"}} ";
            }


        }

        /// <summary>
        /// 查询我的试题库信息
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="t_types">三级教育的类型 0 普通 1 厂级 2 车间  3班组</param>
        /// <returns></returns>
        public HttpResponseMessage Mytest(int us_id,int? t_types)
        {
            string where = "";
            if (t_types!=null) {
                where = " and b.t_types="+t_types+" ";
            }
            string sql = "select *,(select count(id) from test where classify_id=a.testid) as tcount "+
                            "from mytest a " +
                            "left join test_classify b " +
                            "on a.testid = b.id " +
                            "where a.usid = "+us_id+" "+where+"";
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.mytest>.Convert(dt)
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="您还没有试题数据信息"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 查询我的试题库信息
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage Mytest(int us_id)
        {
            string sql = "select *,(select count(id) from test where classify_id=a.testid) as tcount " +
                            "from mytest a " +
                            "left join test_classify b " +
                            "on a.testid = b.id " +
                            "where a.usid = " + us_id + " ";
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.mytest>.Convert(dt)
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "您还没有试题数据信息"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询试题的配置信息
        /// </summary>
        /// <param name="testid">试题的分类ID</param>
        /// <returns></returns>
        public HttpResponseMessage GetTestConfig(int testid)
        {
            string sql = "select type_a,type_b,type_c,type_d,defen_a,defen_b,defen_c,defen_d from test_classify where id=" + testid;
            DataTable dt = help.Totable(sql);
            int typeA = Convert.ToInt32(dt.Rows[0][0]);
            int typeB = Convert.ToInt32(dt.Rows[0][1]);
            int typeC = Convert.ToInt32(dt.Rows[0][2]);
            int typeD = Convert.ToInt32(dt.Rows[0][3]);

            int defen_a = Convert.ToInt32(dt.Rows[0][4]);
            int defen_b = Convert.ToInt32(dt.Rows[0][5]);
            int defen_c = Convert.ToInt32(dt.Rows[0][6]);
            int defen_d = Convert.ToInt32(dt.Rows[0][7]);

            string sqlA = "select top "+typeA+" * from test where t_type like '%是非题%' and classify_id="+testid+" order by newid()";
            string sqlB = "select top " + typeB + " * from test where t_type like '%单选题%' and classify_id="+testid+" order by newid()";
            string sqlC = "select top " + typeC + " * from test where t_type like '%多选题%' and classify_id="+testid+" order by newid()";
            string sqlD = "select top " + typeD + " * from test where t_type like '%案例%' and classify_id="+testid+" order by newid()";

            DataTable dtA = help.Totable(sqlA);
            DataTable dtB = help.Totable(sqlB);
            DataTable dtC = help.Totable(sqlC);
            DataTable dtD = help.Totable(sqlD);

            dtA.Merge(dtB);
            dtA.Merge(dtC);
            dtA.Merge(dtD);

            int count = dtA.Rows.Count + dtB.Rows.Count + dtC.Rows.Count + dtD.Rows.Count;
            if (count > 0)
            {
                obj = new
                {
                    code = 0,
                    defen_a=defen_a,
                    defen_b=defen_b,
                    defen_c=defen_c,
                    defen_d=defen_d,
                    data = ConvertToEntity<Entity.test>.Convert(dtA)
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="您要查看的试题不存在，或都已经管理员删除"
                };
            }

            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询本套试题的所有试题编号
        /// </summary>
        /// <param name="testid">试题的分类ID</param>
        /// <returns></returns>
        public HttpResponseMessage GetAllTest(int testid)
        {
            string sql1 = "select id as testid,t_type,daan from test where (t_type='是非题' or t_type='判断题') and classify_id=" + testid;
            string sql2 = "select id as testid,t_type,daan from test where t_type like '%单%' and classify_id=" + testid;
            string sql3 = "select id as testid,t_type,daan from test where t_type like '%多%' and classify_id=" + testid;
            string sql4 = "select id as testid,t_type,daan from test where t_type like '%案例%' and classify_id=" + testid;

            DataTable dt1= help.Totable(sql1);
            DataTable dt2 = help.Totable(sql2);
            DataTable dt3 = help.Totable(sql3);
            DataTable dt4 = help.Totable(sql4);

            dt1.Merge(dt2);
            dt1.Merge(dt3);
            dt1.Merge(dt4);

            if (dt1.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<TestReturn>.Convert(dt1)
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="本套试题不存在或已被管理员删除"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
            
        }


        /// <summary>
        /// 根据试题的ID编号获取详细的试题内容
        /// </summary>
        /// <param name="testid"></param>
        /// <returns></returns>
        public HttpResponseMessage GetTest(int testid)
        {
            string sql = "select * from test where id="+testid;
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
                    msg = "本试题不存在或已被管理员删除"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 用户胶卷 并将错题加入到错题库中
        /// </summary>
        /// <param name="testid">试题的分类ID</param>
        /// <param name="score">成绩</param>
        /// <param name="us_id">用户编号</param>
        /// <param name="data">data.testid每题的ID编号</param>
        /// <returns></returns>
        public HttpResponseMessage JiaoJuan(int testid, int score,int us_id,dynamic data)
        {
            int times = Zh.Tool.Date_Tool.TimeToInt(DateTime.Now);
            ////记录用户的得分
            string scoreSql = "insert into score(us_id,us_score,test_time,testclassify_id) values(" +
                ""+us_id+","+score+","+times+","+testid+")";
            int i= help.Count(scoreSql);
            if (i > 0)
            {
                //把用户的错题加入到错题库中
                JArray ja = data.testid;
                string sql = "insert into test_error(us_id,test_id,classify_id) ";

                for (int ii = 0; ii < ja.Count; ii++)
                {
                    if (ii == 0)
                    {
                        sql += " values(" + us_id + "," + ja[ii] + "," + testid + ") ";
                    }
                    else
                    {
                        sql += ",(" + us_id + "," + ja[ii] + "," + testid + ") ";
                    }
                }
                int err = help.Count(sql);
                if (err > 0)
                {
                    obj = new
                    {
                        code = 0,
                        msg = "交卷成功，并成功将错题加入到了您的错库"
                    };
                }
                else
                {
                    obj = new
                    {
                        code = 0,
                        msg = "交卷成功，但在生成错题库时出现问题"
                    };
                }
            }
            else {
                obj = new
                {
                    code = 1,
                    msg = "胶卷失败！请稍候重试"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 获取我出错的试题列表
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage TestErrorList(int us_id)
        {
            string sql = "select a.id,a.t_name,a.create_time,"+
                        "(select count(id) from test_error where classify_id = a.id and us_id="+us_id+") as TestCount, " +
                        "(select count(id) from mytest where testid=a.id) as MyTestCount "+
                        "from test_classify a where id in ( " +
                        "select classify_id from test_error " +
                        "group by classify_id) and us_id="+us_id;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.test_error>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    msg="没有查询到错题库列表"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询本题库我的所有错误试题
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="classify_id"></param>
        /// <returns></returns>
        public HttpResponseMessage TestError(int us_id,int classify_id)
        {
            string sql = "select b.*,a.id as eid,a.us_id,a.test_id," +
                "(select count(id) from test_error where test_id=a.test_id and us_id="+us_id+" ) as MyTestCount,"+
                        "(select count(id) from test_error where test_id = a.test_id) as TestCount " +
                        "from test_error a " +
                        "left join test b " +
                        "on a.test_id = b.id " +
                        "where a.us_id = "+us_id+" and a.classify_id = "+classify_id;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.test_error>.Convert(dt)
                };
            }
            else
            {
                obj = new
                {
                    code = 0,
                    msg = "没有查询到错题库列表"
                };
            }
            return Zh.Tool.Json.GetJson(obj);

        }


        /// <summary>
        /// 删除我的错题
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public HttpResponseMessage DelError(int eid)
        {
            string sql = "delete from test_error where id="+eid;
            int i= help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg="错题删除成功"
                };
            }
            else
            {
                obj = new
                {
                    code = 0,
                    msg = "错库删除失败，请重试！"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 统计用户的考试信息
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryTestCount(int us_id)
        {
            string sql = "select avg(us_score) as avg,max(us_score) as max," +
                "min(us_score) as min,count(id) as count from score where us_id="+us_id;
            DataTable dt= help.Totable(sql);
            obj = new {
                data=ConvertToEntity<TestTJ>.Convert(dt)
            };

            return Zh.Tool.Json.GetJson(obj);
            
        }

        /// <summary>
        /// 统计
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryTj(int us_id)
        {
            string sql = "select top 12 cast(year(dateadd(s,test_time,'1970-01-01')) as varchar(10))+'年' " +
"+ cast(month(dateadd(s, test_time, '1970-01-01')) as varchar(10)) + '月' as date, " +
"count(id) as count,max(us_score) as max, " +
"min(us_score) as min,avg(us_score) as avg " +
"from score where us_id =" + us_id + " " +
"group by year(dateadd(s, test_time, '1970-01-01')),month(dateadd(s, test_time, '1970-01-01'))";
            DataTable dt = help.Totable(sql);
            obj = new {
                code=0,
                data=ConvertToEntity<TestTJ>.Convert(dt)
            };
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询所有企业员工的最高成绩 并按从大到小的顺序排序
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryAllUs_MaxScore(int head_id)
        {
            string sql = "select max(us_score) as max,us_id from score " +
                "where us_id in (select us_id from user_detail where head_id="+head_id+") "+
                "group by us_id order by max(us_score) desc";
            DataTable dt= help.Totable(sql);
            obj = new {
                code=0,
                data=ConvertToEntity<TestTJ>.Convert(dt)
            };
            return Zh.Tool.Json.GetJson(obj);
        }
    }



    public class TestReturn
    {
        public int testid { get; set; }
        public string t_type { get; set; }
        public string daan { get; set; }
    }

    public class TestTJ
    {
        public int us_id { get; set; }
        public int count { get; set; }
        public int max { get; set; }
        public int min { get; set; }
        public int avg { get; set; }
        public string date { get; set; }
    }

    
}