using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;

namespace GDT_API.Controllers.GDT.Dal
{
    public class users
    {
        private Lazy<verify> verify = new Lazy<Dal.verify>();
        private Lazy<GDT.Entity.user_detail> detail = new Lazy<Entity.user_detail>();
        private string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private Zh.Tool.SqlHelper help = null;
        private object obj = null;
        public users()
        {
            if (help==null) {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }

        #region  =====================================================修改用户登录信息、详情信息、用户修改密码的方法

        /// <summary>
        /// 用户登录方法  成功 返回用户的ID  不成功 返回0
        /// </summary>
        /// <param name="login_name"></param>
        /// <param name="login_pwd"></param>
        /// <returns></returns>
        public HttpResponseMessage userLogin(string login_name,string login_pwd,out int usid)
        {
            object obj = null;
            string sql = "select us_id from users where login_name=@login_name and login_pwd=@login_pwd";
            SqlParameter[] sp = {
                new SqlParameter("login_name",login_name),
                new SqlParameter("login_pwd",login_pwd)
            };
            object userObj = help.FirstRow(sql,sp);
            if (userObj != null)
            {
                usid = Convert.ToInt32(userObj);
                List<Entity.user_detail> list = verify.Value.GetUserVal(usid);
                obj = new
                {
                    msg = "ok",
                    code = "A0000",
                    data = list
                };
                //添加用户的登录日志
                Thread th = new Thread(new ParameterizedThreadStart(Login_Log));
                th.Start(usid);

                string s = JsonConvert.SerializeObject(obj);
                HttpResponseMessage m = new HttpResponseMessage();
                m.Content = new StringContent(s, System.Text.Encoding.GetEncoding("UTF-8"), "application/json");
                return m;
            }
            else {
                obj = new
                {
                    msg = "该用户不存在！",
                    code = "A0001"
                };
                string s = JsonConvert.SerializeObject(obj);
                HttpResponseMessage m = new HttpResponseMessage();
                m.Content = new StringContent(s, System.Text.Encoding.GetEncoding("UTF-8"), "application/json");
                usid = 0;
                return m;
            }
            
        }

        public void Login_Log(object us_id)
        {
            int usid = 0;
            if (us_id == null)
            {
                return;
            }
            else {
                usid = Convert.ToInt32(us_id);
            }
            var login_time = Zh.Tool.Date_Tool.TimeToInt(DateTime.Now);
            string sql = "insert into login_log(us_id,login_time) values("+usid+","+login_time+")";
            help.Count(sql);
        }

        /// <summary>
        /// 添加用户的在线时长
        /// </summary>
        /// <param name="us_id"></param>
        public void on_line(int us_id)
        {
            ////查询这个用户的记录是不是存在
            string sqlc = "select count(id) from on_line where us_id="+us_id;
            if (Convert.ToInt32(help.FirstRow(sqlc)) == 0)
            {
                sqlc = "insert into on_line(us_id,count) values(" + us_id + ",1)";
                help.Count(sqlc);
            }
            else {
                sqlc = "update on_line set count=count+1 where us_id="+us_id;
                help.Count(sqlc);
            }
        }
        

        /// <summary>
        /// 通过用户名修改用户的密码
        /// </summary>
        /// <param name="login_name"></param>
        /// <param name="new_pwd"></param>
        /// <returns></returns>
        public int ChangePwd(string username,string tel,string pwd)
        {
            string sqls = "select count(a.us_id) from users a " +
                "left join user_detail b on a.us_id=b.us_id " +
                "where login_name='"+username+ "' and mobile='"+tel+"' ";
            if (Convert.ToInt32(help.FirstRow(sqls)) == 1)
            {
                //验证用户的登录名是否存在
                string sql = "update users set login_pwd=@login_pwd where login_name=@login_name";
                SqlParameter[] sp = {
                    new SqlParameter("@login_name",username),
                    new SqlParameter("@login_pwd",pwd)
                };
                return help.Count(sql, sp);
            }
            else {
                return 100;
            }
            
        }


        /// <summary>
        /// 修改登录信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage ChangeUsers(dynamic data,int us_id)
        {
            Entity.user_detail ud= detail.Value.SetData(data);
            string sql = "update users set login_name=@login_name,login_pwd=@login_pwd,real_name=@real_name where us_id=" + us_id + ";";
            
            SqlParameter[] sp = {
                new SqlParameter("@login_name",ud.login_name),
                new SqlParameter("@login_pwd",ud.login_pwd),
                new SqlParameter("@real_name",ud.real_name)
            };
            int i= help.Count(sql,sp);
            object obj = null;
            if (i > 0)
            {
                obj = new
                {
                    code = "0",
                    msg = "修改成功"
                };
            }
            else {
                obj = new
                {
                    code = "1",
                    msg = "修改失败"
                };
            }
            return help.ToJson(obj);
        }


        /// <summary>
        /// 修改用户的权限、详情等信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage ChangeUser_detail(dynamic data,int us_id)
        {
            Entity.user_detail ud = detail.Value.SetData(data);
            string sql = "update user_detail set gender=" + ud.gender + ",age=" + ud.age + ",us_no='" + ud.us_no + "',mobile='" + ud.mobile + "'," +
                "tel='" + ud.tel + "',work_state=" + ud.work_state + ",position='" + ud.position + "',head_id=" + ud.head_id + "," +
                "com_id=" + ud.com_id + ",b_id=" + ud.b_id + ",c_id=" + ud.c_id + ",verify=" + ud.verify + " where us_id=" + us_id;
            int i = help.Count(sql);
            object obj = null;
            if (i > 0)
            {
                obj = new
                {
                    code = "0",
                    msg = "修改成功"
                };
            }
            else
            {
                obj = new
                {
                    code = "1",
                    msg = "修改失败"
                };
            }
            return help.ToJson(obj);
        }


        /// <summary>
        /// 删除指定用户信息
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage DelUser_detail(int us_id)
        {
            string sql = "delete from user_detail where us_id="+us_id+";";
            sql += "delete from users where us_id="+us_id+";";
            int i= help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "删除成功"
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    msg = "删除失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 多删除  
        /// </summary>
        /// <param name="us_ids">用户的id数组  类型为1,2,3,4,5</param>
        /// <returns></returns>
        public HttpResponseMessage DelUserAll(string us_ids)
        {
            string sql = "delete from user_detail us_id in ("+us_ids+");";
            sql += "delete from users us_id in (" + us_ids + ");";
            int i= help.Count(sql);
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
            return Zh.Tool.Json.GetJson(sql);
        }

        #endregion


        #region  ============================================================添加用户登录信息、用户详情信息、根据用户检查用户名是否存在的方法
        /// <summary>
        /// 上传用户登录信息 返回刚添加用户的id 该方法将同时向用户详情信息表添加一条带有用户ID的空记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponseMessage AddUser(dynamic data)
        {
            object objs = null;
            Entity.user_detail ud = detail.Value.SetData(data);
            if (Count(ud.login_name)) {
                objs = new {
                    code = "1",
                    msg = "该用户已存在"
                };
                return Zh.Tool.Json.GetJson(objs);
            }
            //添加用户表
            string sql1 = "insert into users(login_name,login_pwd,real_name) values(@login_name,@login_pwd,@real_name) select @@IDENTITY";
            
            SqlParameter[] sp = {
                new SqlParameter("@login_name",ud.login_name),
                new SqlParameter("@login_pwd",ud.login_pwd),
                new SqlParameter("@real_name",ud.real_name)
            };
            object obj=help.FirstRow(sql1,sp);
            if (obj != null)
            {
                int u_id = Convert.ToInt32(obj);
                ///为用户添加三级教育卡
                int markcard = Convert.ToInt32(data.markcard);
                if (markcard==1) {
                    Card_Sign sign = new Card_Sign();
                    string end_time = data.end_time;
                    sign.Add(end_time,u_id);
                }
                objs = new
                {
                    usid = u_id,
                    code = "0",
                    msg = "添加管理员成功"
                };
            }
            else {
                objs = new
                {
                    usid = 0,
                    code = "2",
                    msg = "上传时出现错误"
                };
            }
            string s = JsonConvert.SerializeObject(objs);
            HttpResponseMessage m = new HttpResponseMessage();
            m.Content = new StringContent(s,System.Text.Encoding.GetEncoding("UTF-8"),"application/json");
            return m;
        }

        /// <summary>
        /// 上传用户权限及详情信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="usid"></param>
        /// <returns></returns>
        public HttpResponseMessage AddUser_detail(dynamic data,int usid)
        {
            Entity.user_detail ud = detail.Value.SetData(data);
            //添加用户详情表
            string sql2 = "insert into user_detail(us_id,gender,age,us_no,mobile,tel,work_state,position,head_id,com_id,b_id," +
            "c_id,verify) values(" + usid + "," + ud.gender + "," + ud.age + ",'" + ud.us_no + "','" + ud.mobile + "','" + ud.tel + "'," +
            "" + ud.work_state + ",'" + ud.position + "'," + ud.head_id + "," + ud.com_id + "," + ud.b_id + "," + ud.c_id + "," +
            "" + ud.verify + ")";
            int i = help.Count(sql2);
            object obj = null;
            if (i > 0)
            {
                obj = new {
                    code="0",
                    msg="添加成功"
                };
            }
            else
            {
                obj = new
                {
                    code = "1",
                    msg = "添加失败"
                };
            }
            return help.ToJson(obj);
        }

        

        /// <summary>
        /// 根据用户名查询用户是不是存在 存在返回true 不存在返回false
        /// </summary>
        /// <param name="login_name"></param>
        /// <returns></returns>
        public bool Count(string login_name)
        {
            string sql = "select count(us_id) from users where login_name=@login_name";
            SqlParameter[] sp = {
                new SqlParameter("@login_name",login_name)
            };
            object obj = help.FirstRow(sql,sp);
            if (obj != null)
            {
                int c = Convert.ToInt32(obj);
                if (c > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else {
                return false;
            }
        }

        #endregion


        #region  ================================================================查询相关
        /// <summary>
        /// 查询指定用户的详细信息
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage Query(int us_id)
        {
            List<Entity.user_detail> list = verify.Value.GetUserVal(us_id);
            object obj = new {
                code = "A0000",
                msg="ok",
                data=list
            };
            return help.ToJson(obj);
        }

        public HttpResponseMessage QueryUser(int us_id)
        {
            string sql = "select * from user_detail a " +
                "left join users b on a.us_id=b.us_id where a.us_id="+us_id;
            DataTable dt = help.Totable(sql);
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
                    code = 0,
                    msg = "成功",
                    data = ""
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 查询总部全部人员的数据量
        /// </summary>
        /// <param name="userTyle">查询用户的范围 总部所有用户 子公司所有用户 部门所有用户  班级所有用户</param>
        /// <param name="value">查询关键词</param>
        /// <param name="id">对应 userType的id</param>
        /// <returns></returns>
        public int GetListCount(string userTyle, string value, int id)
        {
            string sql = "";
            if (userTyle == "head")//如果参数为总部 查询总部人员的数量
            {
                sql = "select count(a.us_id) from user_detail a " +
                        "left join users b " +
                        "on a.us_id = b.us_id " +
                        "where head_id = " + id + " ";

            }
            else if (userTyle == "company")//如果参数为子公司 查询子公司人员的数量
            {
                sql = "select count(a.us_id) from user_detail a " +
                        "left join users b " +
                        "on a.us_id = b.us_id " +
                        "where com_id = " + id + " ";
            }
            else if (userTyle == "deparment")//如果参数为部门 查询部门人员的数量
            {
                sql = "select count(a.us_id) from user_detail a " +
                        "left join users b " +
                        "on a.us_id = b.us_id " +
                        "where b_id = " + id + " ";
            }
            else if (userTyle == "classes")//如果参数为班组 查询班组人员的数量
            {
                sql = "select count(a.us_id) from user_detail a " +
                        "left join users b " +
                        "on a.us_id = b.us_id " +
                        "where c_id = " + id + " ";
            }
            else
            {//返回1
                return 1;
            }
            if (value != null && value != "")
            {
                sql += " and ( a.mobile='" + value + "' or a.mobile like '%" + value + "%' or b.login_name like '%" + value + "%' or b.real_name like '%" + value + "%')";
            }
            object c = help.FirstRow(sql);
            if (c != null)
            {
                return Convert.ToInt32(c);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 查询员工信息 可根据范围和关键词进行筛选
        /// </summary>
        /// <param name="userType">用户的范围（类型）             不可为空</param>
        /// <param name="value">查询关键词                      可为空或""</param>
        /// <param name="id">部门/子公司/部门/班级的ID编号        不可为空</param>
        /// <param name="page">当前页                             不可为空</param>
        /// <param name="count">每页显示数据量                      不可空</param>
        /// <returns></returns>
        public HttpResponseMessage Query(string userType,string value, int id,int page,int count)
        {
            int p = (page - 1) * count + 1;
            int c = page * count;
            string sql = "";
            string where = "";
            //查询管理的范围 总部所有员工  子公司所有员工  部门所有员工  班级所有员工 
            if (userType == "head") {
                where = "where a.head_id = "+id+" ";
            } else if (userType == "company") {
                where = "where a.com_id = " + id + " ";
            } else if (userType == "deparment") {
                where = "where a.b_id = " + id + " ";
            } else if (userType=="classes") {
                where = "where a.c_id = " + id + " ";
            }
            /////如果用户使用了关键词进行查询
            if (value!=null && value!="") {
                where += " and (b.real_name like '%"+value+"%' or b.login_name like '%"+value+"%' or a.mobile like '%"+value+"%' ) ";
            }

            sql = "select * from ( " +
                    "select a.*,b.real_name,c.c_name,c.c_work_name,c.c_work_desc, " +
                    "d.b_name,d.b_work_name,d.b_work_desc,e.com_name,e.com_size, " +
                    "e.work_address, " +
                    "row_number() over(order by a.us_id) as row from user_detail a " +
                    "left join users b on a.us_id = b.us_id " +
                    "left join classes c on a.c_id = c.c_id " +
                    "left join deparment d on a.b_id = d.b_id " +
                    "left join company e on a.com_id = e.com_id " +
                    "" + where + ") temp " +
                    "where row between " + p + " and " + c + "";
            DataTable dt= help.Totable(sql);
            object obj = new {
                code = "A0000",
                msg = "ok",
                count = GetListCount(userType,value,id),
                data = ConvertToEntity<Entity.user_detail>.Convert(dt)
            };
            return help.ToJson(obj);
        }


        /// <summary>
        /// 筛选员工信息 查询数量
        /// </summary>
        /// <param name="head_id"></param>
        /// <param name="com_id"></param>
        /// <param name="b_id"></param>
        /// <param name="c_id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int QueryUserCount(int head_id,int com_id,int b_id,int c_id,string value)
        {
            string sql = "select count(a.us_id) from user_detail a "+
                        "left join users b on a.us_id = b.us_id " +
                        "where a.head_id="+head_id+" ";
            string where = "";
            if (com_id>=0) {
                where += " and a.com_id="+com_id+" ";
            }
            if (b_id>0) {
                where += " and a.b_id=" + b_id + " ";
            }
            if (c_id>0) {
                where += " and a.c_id=" + c_id + " ";
            }
            if (value!="" && value!=null) {
                where += " and (b.real_name like '%"+value+"%' or b.login_name like '%"+value+"%' or a.mobile like '%"+value+"%' )";
            }
            return Convert.ToInt32(help.FirstRow(sql+where));
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
        public HttpResponseMessage QueryUserList(int head_id, int com_id, int b_id, int c_id, string value,int page,int count,int verify,
            int mycom_id,int myb_id,int myc_id)
        {
            string where = "";
            if (verify==1) {
                where += " and a.com_id="+mycom_id+" ";
            }
            if (verify == 2)
            {
                if (myb_id == 0)
                {
                    where += " and a.b_id=1000000 ";
                }
                else
                {
                    where += " and a.b_id=" + myb_id + " ";
                }
            }
            if (verify == 3)
            {
                if (myc_id==0) {
                    where += " and a.c_id=1000000 ";
                }
                else
                {
                    where += " and a.c_id=" + myc_id + " ";
                }
                
            }
            //从和通过登录用户的权限查询相应员工信息

            if (com_id > 0)
            {
                where += " and a.com_id=" + com_id + " ";
            }
            if (b_id > 0)
            {
                where += " and a.b_id=" + b_id + " ";
            }
            if (c_id > 0)
            {
                where += " and a.c_id=" + c_id + " ";
            }
            if (value != "" && value != null)
            {
                where += " and (b.real_name like '%" + value + "%' or b.login_name like '%" + value + "%' or a.mobile like '%" + value + "%' )";
            }
            int p = (page - 1) * count + 1;
            int c = page * count;
            string sql = "select * from ( "+
                        "select a.position,a.mobile,a.gender,a.age,a.tel,a.head_id,a.com_id,a.b_id,a.c_id,a.us_id, " +
                        "b.login_name,b.real_name,c.c_name,d.b_name,e.com_name,a.verify, " +
                        "ROW_NUMBER() over(order by a.us_id) as row from user_detail a " +
                        "left join users b on a.us_id = b.us_id " +
                        "left join classes c on a.c_id = c.c_id " +
                        "left join deparment d on a.b_id = d.b_id " +
                        "left join company e on a.com_id = e.com_id " +
                        "where a.head_id = "+head_id+" "+where+" " +
                        ") temp " +
                        "where row between "+p+" and "+c+"";
            
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    count = QueryUserCount(head_id,com_id,b_id,c_id,value),
                    data = ConvertToEntity<Entity.user_detail>.Convert(dt)
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="还没有上传员工信息",
                    data=""
                };
            }
            return Zh.Tool.Json.GetJson(obj);
            
        }

        /// <summary>
        /// 查询用户的登录日志信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryLoginLog(int head_id, int page,int count)
        {
            int p = (page - 1) * count + 1;
            int c = page * count;
            string sql = "select * from ( "+
                        "select b.real_name,a.login_time, " +
                        "row_number() over(order by a.login_time desc) as row " +
                        "from login_log a " +
                        "left join users b " +
                        "on a.us_id = b.us_id " +
                        "left join user_detail c on a.us_id=c.us_id " +
                        "where c.head_id="+head_id+" )temp " +
                        "where row between "+p+" and "+c;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.login_log>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code = 0,
                    msg="无数据"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 查询用户的在线总时长
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public int Query_online(int us_id)
        {
            string sql = "select [count] from on_line where us_id="+us_id;
            object o= help.FirstRow(sql);
            if (o != null)
            {
                return Convert.ToInt32(o);
            }
            else {
                return 0;
            }
        }

        #endregion  =========================================================================查询结束



    }
}