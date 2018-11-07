using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GDT_API.Controllers.GDT.Dal
{
    public class Head_office
    {
        private string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private Lazy<Entity.user_detail> user = new Lazy<Entity.user_detail>();
        private Lazy<Entity.company> company = new Lazy<Entity.company>();
        private Zh.Tool.SqlHelper help = null;
        private object obj = null;
        public Head_office()
        {
            if (help==null) {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }

        /// <summary>
        /// 添加集团信息 成功返回受影响的行数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int AddHeadOffice(dynamic data)
        {
            Entity.user_detail d= user.Value.SetData(data);
            string sql = "insert into head_office(head_name,head_size,head_address,head_logo) values(" +
                "@head_name,@head_size,@head_address,@head_logo)";
            SqlParameter[] sp = {
                new SqlParameter("@head_name",d.head_name)
                ,new SqlParameter("@head_size",d.head_size)
                ,new SqlParameter("@head_address",d.head_address)
                ,new SqlParameter("@head_logo",d.head_logo)
            };
            return help.Count(sql,sp);
        }

        /// <summary>
        /// 添加集团信息  成功返回最新的集团ID
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int AddHead(dynamic data)
        {
            Entity.user_detail d = user.Value.SetData(data);
            string sql = "insert into head_office(head_name,head_size,head_address,head_logo) values(" +
                "@head_name,@head_size,@head_address,@head_logo) select @@IDENTITY";
            SqlParameter[] sp = {
                new SqlParameter("@head_name",d.head_name)
                ,new SqlParameter("@head_size",d.head_size)
                ,new SqlParameter("@head_address",d.head_address)
                ,new SqlParameter("@head_logo",d.head_logo)
            };
            return Convert.ToInt32(help.FirstRow(sql, sp));
        }




        /// <summary>
        /// 添加子公司的方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponseMessage AddCompany(dynamic data)
        {
            Entity.user_detail com = user.Value.SetData(data);
            string sql = "insert into company(com_name,com_size,work_address,head_id) values(" +
                "'"+com.com_name+"','"+com.com_size+"','"+com.work_address+"',"+com.head_id+")";
            int i= help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = "A0000",
                    msg = "添加成功"
                };
            }
            else {
                obj = new
                {
                    code = "A0001",
                    msg = "添加失败"
                };
            }
            return help.ToJson(obj);
        }


        /// <summary>
        /// 查询集团下的子公司列表
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryCompany(int head_id,int com_id,int verify)
        {
            string sql = "select c.*,(select count(us_id) from user_detail where com_id=c.com_id) as u_count," +
                "(select count(b_id) from deparment where com_id=c.com_id) as b_count " +
                "from company c where c.head_id="+head_id+" ";
            if (verify<=3) {
                if (verify == 1 || verify == 2)
                {
                    sql += " and c.com_id=" + com_id + " ";
                }
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "查看数据的权限不足",
                };
                return Zh.Tool.Json.GetJson(obj);
            }

            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "ok",
                    count = 200,
                    data = ConvertToEntity<Entity.user_detail>.Convert(help.Totable(sql))
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    msg = "没有上传公司列表信息",
                    count = 200,
                    data = ConvertToEntity<Entity.user_detail>.Convert(help.Totable(sql))
                };
            }
            
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询单个子公司的详情信息
        /// </summary>
        /// <param name="com_id">子公司唯一ID编号</param>
        /// <returns></returns>
        public HttpResponseMessage QueryChildCompany(int com_id)
        {
            string sql = "select * from company where com_id="+com_id;
            DataTable dt= help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = "0",
                    msg = "读取数据成功",
                    data = ConvertToEntity<Entity.company>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code = "1",
                    msg = "没有查询出相关数据信息"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 修改子公司信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="com_id"></param>
        /// <returns></returns>
        public HttpResponseMessage UpdateCompany(dynamic data,int com_id)
        {
            Entity.user_detail com = user.Value.SetData(data);
            string sql = "update company set com_name='"+com.com_name+ "',com_size='"+com.com_size+ "'," +
                "work_address='"+com.work_address+"' where com_id="+com_id;
            int i= help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = "A0000",
                    msg = "修改成功"
                };
            }
            else {
                obj = new
                {
                    code = "A0001",
                    msg="修改失败"
                };
            }
            return help.ToJson(obj);
        }

        /// <summary>
        /// 删除子公司信息 同时删除其下部门及班级，并且调整用户的角色
        /// </summary>
        /// <param name="com_id"></param>
        /// <returns></returns>
        public HttpResponseMessage DelCompany(int com_id)
        {
            string classwhere = "select c_id from user_detail where com_id=" + com_id;
            string sql = "delete from company where com_id="+com_id+";";
            sql += "delete from deparment where com_id =" + com_id + ";";
            sql += "delete from classes where c_id in (" + classwhere + ");";

            if (help.Count(sql) > 0)
            {
                string newsql = "update user_detail set com_id=0,b_id=0,c_id=0 where com_id="+com_id;
                int i = help.Count(newsql);
                if (i > 0)
                {
                    obj = new
                    {
                        code = "A0000",
                        msg = "删除数据成功！"
                    };
                }
                else {
                    obj = new
                    {
                        code = "A0002",
                        msg = "删除数据成功！但在用户角色调整时未完成"
                    };
                }
            }
            else {
                obj = new {
                    code="A0001",
                    msg="删除数据时出错误！"
                };
            }

            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 修改总部信息  包括公司logo
        /// </summary>
        /// <param name="data"></param>
        /// <param name="head_id"></param>
        /// <returns></returns>
        public HttpResponseMessage UpdateHead_office(dynamic data,int head_id)
        {
            Entity.user_detail d = user.Value.SetData(data);
            string sql = "update head_office set head_name=@head_name,head_size=@head_size," +
                "head_address=@head_address,head_logo=@head_logo where head_id="+head_id;
            SqlParameter[] sp = {
                new SqlParameter("@head_name",d.head_name),
                new SqlParameter("@head_size",d.head_size),
                new SqlParameter("@head_address",d.head_address),
                new SqlParameter("@head_logo",d.head_logo),
            };
            int i= help.Count(sql,sp);
            if (i > 0)
            {
                obj = new
                {
                    code = "A0000",
                    msg = "修改成功"
                };
            }
            else {
                obj = new
                {
                    code = "A0001",
                    msg = "修改失败"
                };
            }
            return help.ToJson(obj);
        }

        /// <summary>
        /// 根据集团Id查询集团信息
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryHeadOffice(int head_id)
        {
            string sql = "select * from head_office where head_id="+head_id;
            obj = new {
                code="A0000",
                msg="ok",
                count=1,
                data=ConvertToEntity<Entity.head_office>.Convert(help.Totable(sql))
            };
            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 总部上传公司LOGO及上传文件
        /// </summary>
        /// <returns></returns>
        public string Upload_file()
        {
            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            string type = file.FileName.Substring(file.FileName.LastIndexOf("."), file.FileName.Length - file.FileName.LastIndexOf("."));
            Random ran = new Random();
            int r = ran.Next(1000000,9999999);
            string newName = DateTime.Now.ToFileTime() + r + type;
            string path = AppDomain.CurrentDomain.BaseDirectory + "/file/" + newName;
            string msg = "";
            string objs = "";

            string outpath = "/logo/" + newName;
            if (Zh.Tool.File_Tool.File_Upload(file.InputStream, path, out msg))
            {
                objs = "{\"code\": 0 ,\"msg\": \"上传成功\",\"data\": {\"src\": \""+outpath+"\"}}";
            }
            else {
                objs = "{\"code\": 1 ,\"msg\": \"上传失败\",\"data\": {\"src\": \"\"}}";
            }
            return objs;
            
        }

        /// <summary>
        /// 查询集团下所有的班级信息，并按照部门进行分组
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        public HttpResponseMessage GetAllClass(int head_id)
        {
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            DataSet ds = new DataSet();
            string sql = "select a.b_name,a.b_id from deparment a "+
                            "left join company b " +
                            "on a.com_id = b.com_id " +
                            "where b.head_id = "+head_id+"; ";
            sql += "select a.c_name,a.c_id,b.b_id from classes a "+
                    "left join deparment b " +
                    "left join company c " +
                    "on b.com_id = c.com_id " +
                    "on a.b_id = b.b_id " +
                    "where c.head_id = "+head_id+"; ";

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(ds);
            //这里是对datarelation的应用 分组管理
            DataRelation dr = new DataRelation("table", ds.Tables[0].Columns["b_id"], ds.Tables[1].Columns["b_id"]);
            ds.Relations.Add(dr);//---------------这一句千万不能忘记写--------------------------
            List<bname> blist = new List<bname>();
            foreach (DataRow item in ds.Tables["table"].Rows)
            {
                
                string str = item["b_name"].ToString();
                List<Entity.classes> list = new List<Entity.classes>();
                foreach (var item1 in item.GetChildRows(dr)) {
                    string c_name = item1["c_name"].ToString();
                    list.Add(new Entity.classes() {
                        c_id = Convert.ToInt32(item1["c_id"]),
                        c_name = item1["c_name"].ToString()
                    });
                }
                blist.Add(new bname()
                {
                    b_id = Convert.ToInt32(item["b_id"]),
                    b_name = item["b_name"].ToString(),
                    clist=list
                });
            }
            
            
            return Zh.Tool.Json.GetJson(blist);
        }

        /// <summary>
        /// 查询集团下所有的信息信息  可根据用户姓名进行筛选
        /// </summary>
        /// <param name="data"></param>
        /// <param name="head_id"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public HttpResponseMessage GetAllUser(dynamic data,int head_id)
        {
            string value = data.value;
            string usql = "select count(a.us_id) from user_detail a "+
                            "left join users b " +
                            "on a.us_id = b.us_id " +
                            "where head_id = "+head_id+" ";
            string where = "";
            if (value!="" && value!=null) {
                usql += " and (b.real_name like '%" + value + "%' or a.tel like '%"+value+ "%' or a.mobile like '%"+value+"%' or a.us_no like '%"+value+"%') ";
                where = " and (a.real_name like '%" + value + "%' or b.tel like '%" + value + "%' or b.mobile like '%" + value + "%' or b.us_no like '%" + value + "%') ";
            }
            int counts =Convert.ToInt32( help.FirstRow(usql));

            string sql = "select b.*,a.login_name,a.real_name, row_number() " +
                        "over(order by a.us_id desc) as row from users a " +
                        "left join user_detail b " +
                        "on a.us_id = b.us_id " +
                        "where b.head_id = " + head_id + " " + where + " ";
            DataTable dt= help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    count=counts,
                    data = ConvertToEntity<Entity.user_detail>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    count=0,
                    msg="没有查询到相关员工信息"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 已班级的形式进行分配试题
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponseMessage AddTestFoClass(dynamic data)
        {
            JArray ja = data.classId;
            int testid = data.testid;
            string cid = "";
            for (int i=0;i<ja.Count;i++) {
                if (i == 0)
                {
                    cid = "" + ja[i].ToString();
                }
                else {
                    cid += "," + ja[i].ToString();
                }
            }

            //通过ja查询出所有员工的id编号
            string usql = "select us_id from user_detail where c_id in ("+cid+")";
            DataTable dt= help.Totable(usql);

            string sql = "";
            for (int i=0;i<dt.Rows.Count;i++) {
                int uid = Convert.ToInt32(dt.Rows[i][0]);
                int tiems = Zh.Tool.Date_Tool.TimeToInt(DateTime.Now);
                sql+= "insert mytest(testid,usid,times) select "+testid+","+uid+","+tiems+" " +
"where not EXISTS(select * from mytest where testid = "+testid+" and usid = "+uid+"); ";
            }
            int count= help.Count(sql);

            if (count > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "试题分配成功"
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="试题发布失败"
                };
            }

            return Zh.Tool.Json.GetJson(obj);
            
        }

        /// <summary>
        /// 已个人的形式进行分配试题
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponseMessage AddTestFoUser(dynamic data)
        {
            // 这里接收的是用户的id 形式为数组
            JArray ja = data.us_ids;
            int testid = data.testid;
            string sql = "";
            for (int i=0;i<ja.Count;i++) {
                int uid =Convert.ToInt32(ja[i]);
                int tiems = Zh.Tool.Date_Tool.TimeToInt(DateTime.Now);
                sql += "insert mytest(testid,usid,times) select " + testid + "," + uid + "," + tiems + " " +
"where not EXISTS(select * from mytest where testid = " + testid + " and usid = " + uid + "); ";
            }

            int count= help.Count(sql);
            if (count > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "试题分配成功"
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "试题发布失败"
                };
            }

            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 一键为集团所有人分配试题
        /// </summary>
        /// <param name="head_id"></param>
        /// <param name="testid"></param>
        /// <returns></returns>
        public HttpResponseMessage AddTestFoHead(int head_id,int testid)
        {
            //通过ja查询出所有员工的id编号
            string usql = "select us_id from user_detail where head_id="+head_id+" ";
            DataTable dt = help.Totable(usql);

            string sql = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int uid = Convert.ToInt32(dt.Rows[i][0]);
                int tiems = Zh.Tool.Date_Tool.TimeToInt(DateTime.Now);
                sql += "insert mytest(testid,usid,times) select " + testid + "," + uid + "," + tiems + " " +
"where not EXISTS(select * from mytest where testid = " + testid + " and usid = " + uid + "); ";
            }
            int count = help.Count(sql);

            if (count > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "试题分配成功"
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "试题发布失败"
                };
            }

            return Zh.Tool.Json.GetJson(obj);
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
        public HttpResponseMessage DataList(int head_id,int verify,int com_id,int b_id,int c_id)
        {
            string where_1 = "";//定义厂级需要的筛选条件
            string where_2 = "";//定义部门级需要的筛选条件
            string where_3 = "";//定义班组级需要的筛选条件

            if (verify==1) {//厂级管理权限
                where_1 += " and com_id="+com_id+" ";
            }
            if (verify==2) {//部门权限
                where_1 += " and com_id=" + com_id + " ";
                where_2 += " and b_id="+b_id+" ";
            }
            if (verify==3) {//班级权限
                where_1 += " and com_id=" + com_id + " ";
                where_2 += " and b_id=" + b_id + " ";
                where_3 += " and c_id="+c_id+" ";
            }
            //string sql = "select * from company where head_id="+head_id+" "+where_1+";"+
            //            "select * from deparment where com_id in " +
            //            "(select com_id from company where head_id = " + head_id + ") " + where_2+";" +
            //            "select * from classes where b_id in  " +
            //            "(select b_id from deparment where com_id in " +
            //            "(select com_id from company where head_id = " + head_id + ")) " + where_3+"; ";
            string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

            DataSet comDs = new DataSet();
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            SqlDataAdapter comAda = new SqlDataAdapter("select * from company where head_id=" + head_id + " " + where_1 + ";", conn);
            comAda.Fill(comDs,"company");

            SqlDataAdapter depAda = new SqlDataAdapter("select * from deparment where com_id in " +
                        "(select com_id from company where head_id = " + head_id + ") " + where_2 + ";",conn);
            depAda.Fill(comDs,"deparment");

            SqlDataAdapter classAda = new SqlDataAdapter("select * from classes where b_id in  " +
                        "(select b_id from deparment where com_id in " +
                        "(select com_id from company where head_id = " + head_id + ")) " + where_3 + "; ",conn);
            classAda.Fill(comDs,"classes");

            DataRelation comRel = comDs.Relations.Add("company",comDs.Tables["company"].Columns["com_id"],comDs.Tables["deparment"].Columns["com_id"]);
            DataRelation depRel = comDs.Relations.Add("deparment",comDs.Tables["deparment"].Columns["b_id"],comDs.Tables["classes"].Columns["b_id"]);

            List<Ccompany> clist = new List<Ccompany>();
            foreach (DataRow comRow in comDs.Tables["company"].Rows) {
                
                List<Cdeparment> dlist = new List<Cdeparment>();
                foreach (DataRow depRow in comRow.GetChildRows(comRel)) {
                    
                    List<Cclasses> classlist = new List<Cclasses>();
                    foreach (DataRow classesRow in depRow.GetChildRows(depRel)) {
                        classlist.Add(new Cclasses {
                            c_id=(int)classesRow["c_id"],
                            c_name=classesRow["c_name"].ToString()
                        });
                        
                    }
                    dlist.Add(new Cdeparment
                    {
                        b_id = (int)depRow["b_id"],
                        b_name = depRow["b_name"].ToString(),
                        claList=classlist
                    });
                }
                clist.Add(new Ccompany
                {
                    com_id = (int)comRow["com_id"],
                    com_name = comRow["com_name"].ToString(),
                    depList=dlist
                });
            }

            conn.Close();

            obj = new
            {
                code = 0,
                list = clist
            };

            return Zh.Tool.Json.GetJson(obj);
            
        }

    }

    public class bname {
        public int b_id { get; set; }
        public string b_name { get; set; }
        public List<Entity.classes> clist { get; set; }
    }

    public class Ccompany
    {
        public int com_id { get; set; }
        public string com_name { get; set; }
        public List<Cdeparment> depList { get; set; }

        
    }

    public class Cdeparment
    {
        public int b_id { get; set; }
        public string b_name { get; set; }
        public List<Cclasses> claList { get; set; }
    }

    public class Cclasses
    {
        public int c_id { get; set; }
        public string c_name { get; set; }
    }


   
}