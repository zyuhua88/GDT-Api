using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Http;
using System.Data;

namespace GDT_API.Controllers.GDT.Dal
{
    public class Tj
    {
        private string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private Zh.Tool.SqlHelper help = null;
        private object obj = null;

        public Tj()
        {
            if (help == null)
            {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }

        /// <summary>
        /// 查询用户每题的最高得分、最低得分、平均得分、在线时长（分钟）,累计参考次数、试题名称
        /// 用户id编号
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryUserTest(int us_id)
        {
            string sql = "select max(a.us_score) as max,min(a.us_score) as min, " +
                        "avg(a.us_score) as avg,count(a.us_id) as count,a.us_id,b.t_name, " +
                        "(select count from on_line where us_id = 13)/ 60 as online " +
                        "from score a " +
                        "left join test_classify b on a.testclassify_id = b.id " +
                        "where a.us_id = " + us_id + " " +
                        "group by b.t_name,a.us_id";
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<userTest>.Convert(dt)
                };
            }
            else {
                obj = new {
                    code = 1,
                    msg = "没有数据信息"
                };
            }
            return Zh.Tool.Json.GetJson(obj);

        }

        /// <summary>
        /// 根据用户设定的成绩查询用户的合格率
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryUserTestAVG(int us_id,int score)
        {
            string sql = "select sum(case when us_score>="+score+" then 1 else 0 end)*100/count(*) as yes, "+
                        "sum(case when us_score < "+score+" then 1 else 0 end) * 100 / count(*) as no, " +
                        "(select t_name from test_classify where id = testclassify_id) as testname " +
                        "from score " +
                        "where us_id = "+us_id+" " +
                        "group by testclassify_id";
            obj = new
            {
                code=0,
                data=ConvertToEntity<HeGe>.Convert(help.Totable(sql))
            };
            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 查询学习资料的学习情况 结果为文件名称、学习百分比、已学习时长、要求的总学习时长
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryFile(int us_id)
        {
            string sql = "select title,a.timelenght*100/b.studenttime as times," +
                        "a.timelenght,b.studenttime " +
                        " from studenttime a " +
                        "left join filelist b " +
                        "on a.fileid = b.id " +
                        "where a.usid = " + us_id + "";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<userFile>.Convert(dt)
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "没有数据信息"
                };
            }
            return Zh.Tool.Json.GetJson(obj);

        }

        /// <summary>
        /// 查询我发布的检查信息及我发布的培训计划信息  我发布的
        /// yhtable=检查信息  project=培训计划
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage TjmySend(int us_id)
        {
            string sql = "SELECT sum(列名1) yhtable,sum(列名2) project,sum(列名3) 列名3 FROM ( " +
                         "SELECT count(*) 列名1,0 列名2,0 列名3 FROM yhtable " +
                         "WHERE y_usid = " + us_id + " " +
                         "UNION ALL " +
                         "SELECT 0 列名1,count(*) 列名2,0 列名3 FROM train_project " +
                         "WHERE us_id = " + us_id + " " +
                         ") t; ";
            obj = new {
                code=0,
                data=ConvertToEntity<TjmySend>.Convert(help.Totable(sql))
            };
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询我等待处理信息及我负责的培训计划  等处理处理项
        /// yhtable=检查信息  project=培训计划
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage waitWork(int us_id)
        {
            string sql = "SELECT sum(列名1) yhtable,sum(列名2) project,sum(列名3) qrcount FROM ( "+
                         "SELECT count(*) 列名1,0 列名2,0 列名3 FROM yhtable " +
                         "WHERE y_headuser = "+us_id+ " and y_headtype!=2 " +
                         "UNION ALL " +
                         "SELECT 0 列名1,count(*) 列名2,0 列名3 FROM yhtable " +
                         "WHERE y_zguser = " + us_id + " and (y_status=0 or y_status=1) and (y_headuser=0 or y_headtype=2) " +
                         "UNION ALL "+
                        "SELECT 0 列名1,0 列名2,count(y_id) 列名3 FROM yhtable " +
                        "WHERE y_qruser = "+us_id+" and y_status = 3 " +
                         ") t; ";
            obj = new
            {
                code = 0,
                data = ConvertToEntity<TjmySend>.Convert(help.Totable(sql))
            };
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询已审核的检查数量
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public object MyVerifyOver(int us_id)
        {
            string sql = "select count(y_id) from yhtable " +
                        "where y_headuser = 2 and y_headtype = "+us_id+"";
            return  help.FirstRow(sql);
            
        }

        /// <summary>
        /// 查询整改完成的
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public object MyZGover(int us_id)
        {
            string sql = "select count(y_id) from yhtable "+
"where y_zguser = 2 and y_status = 3";
            return help.FirstRow(sql);
        }


    }

    

    public class TjmySend
    {
        public int yhtable { get; set; }
        public int project { get; set; }
        public int qrcount { get; set; }
    }
        


    public class userTest
    {
        public int max { get; set; }
        public int min { get; set; }
        public int count { get; set; }
        public int us_id { get; set; }
        public string t_name { get; set; }
        public int avg { get; set; }
        public int online { get; set; }
    }

    public class userFile
    {
        public string title { get; set; }
        public int times { get; set; }
        public int timelenght { get; set; }
        public int studenttime { get; set; }
    }

    public class HeGe
    {
        public int yes { get; set; }//合格率
        public int no { get; set; }//不合格率
        public string testname { get; set; }
    }
}