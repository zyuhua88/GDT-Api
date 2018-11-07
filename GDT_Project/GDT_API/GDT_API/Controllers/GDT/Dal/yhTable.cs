using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Http;

namespace GDT_API.Controllers.GDT.Dal
{
    public class yhTable
    {
        private string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private Lazy<Entity.yhtable> yh = new Lazy<Entity.yhtable>();
        private Zh.Tool.SqlHelper help = null;
        private object obj = null;
        public yhTable() {
            if (help==null) {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }

        /// <summary>
        /// 生成自动编号
        /// </summary>
        /// <param name="y_headid"></param>
        /// <returns></returns>
        public string SetNo(int y_headid)
        {
            string sql = "select top 1 y_id from yhtable where y_headid=" + y_headid+" order by y_id desc";
            int no = 0;
            if (sql != null) {
                no = Convert.ToInt32(help.FirstRow(sql)) + 1;
            }
            else {
                no = 1;
            }
            string hsql = "select head_no from head_office where head_id=" + y_headid;
            string nos = help.FirstRow(hsql).ToString();
            string newno = "";
            if (no < 10 && no>0)
            {
                newno = "000" + no;
            }
            if (no < 100 && no>9)
            {
                newno = "00" + no;
            }
            if (no < 1000 && no>99)
            {
                newno = "0" + no;
            }
            if (no > 999)
            {
                newno = no.ToString();
            }
            string y_no = nos + "-" + "HSEJC-" + newno;
            return y_no;
        }

        /// <summary>
        /// 创建检查表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int CreateYh(dynamic data)
        {
            Entity.yhtable y = yh.Value.SetData(data);
            ////获取编号
            string no = SetNo(y.y_headid);
            string sql = "insert into yhtable values('"+y.y_name+"',"+y.y_createtime+",'"+y.y_body+"','"+y.y_area+"'," +
                "'"+y.y_type+"',"+y.y_dengji+",'"+y.y_desc+"','"+y.y_img1+"',"+y.y_endtime+","+y.y_usid+","+y.y_zguser+",'"+y.y_yaoqiu+"', "+
                ""+y.y_headuser+","+y.y_headtype+",'"+y.y_zgdesc+"',0,'"+y.y_qrdesc+"'," +
                "'"+y.y_qrimg+"',"+y.y_status+","+y.y_qruser+",0,"+y.y_headid+",'"+no+"','')";

            string sql2 = "";
            if (y.y_headuser > 0)
            {//如果需要上级审批发信息给上级领导进行通知
                sql2 = "select mobile from user_detail where us_id="+y.y_headuser;
                object objs = help.FirstRow(sql2);
                if (objs!=null) {
                    if (objs.ToString()!="")
                    {
                        sendXinXi(objs.ToString(), "一个安全检查单需要您的审批，检查类型：" + y.y_type + "。详情请登录系统查看！");
                    }
                }
            }
            else {//需要上级审批 直接发信息给整改人
                sql2 = "select mobile from user_detail where us_id=" + y.y_zguser;
                object objs = help.FirstRow(sql2);
                if (objs != null)
                {
                    if (objs.ToString()!="") {
                        sendXinXi(objs.ToString(), "您收到一个检查整改通知单，检查类型：" + y.y_type + "。详情请登录系统查看并及时整改！");
                    }
                    
                }
            }
            return help.Count(sql);
        }

        //给上级发送信息
        public void sendXinXi(string tel,string body)
        {
            LoginPayAndShare.App_Start.XinXi x = new LoginPayAndShare.App_Start.XinXi();
            x.X(tel,body);
        }

        /// <summary>
        /// 删除指定的数据信息
        /// </summary>
        /// <param name="y_id"></param>
        /// <returns></returns>
        public int Del(int y_id)
        {
            string sql = "delete from yhtable where y_id="+y_id;
            return help.Count(sql);
        }

        /// <summary>
        /// 检查部门负责人签字
        /// </summary>
        /// <param name="y_headuser">检查部门负责人的id编号</param>
        /// <param name="y_headtype">检查部门负责人的意见 0 待批示 1 不同意 2 同意整改</param>
        /// <param name="y_id"></param>
        /// <returns></returns>
        public int HeadSign(int y_headuser,int y_headtype,int y_id)
        {
            if (ReStatus(y_id)==2) {//表单已闭环 不能修改
                return 100;
            }
            string sql = "update yhtable set y_headuser="+y_headuser+",y_headtype="+y_headtype+" where y_id="+y_id;
            int i= help.Count(sql);
            ///////发送信息
            string sql2 = "select b.mobile from yhtable a "+
                            "left join user_detail b " +
                            "on a.y_zguser = b.us_id where y_id = "+y_id;
            object objs = help.FirstRow(sql2);
            if (objs != null)
            {
                if (objs.ToString()!="") {
                    sendXinXi(objs.ToString(), "您收到一个检查整改通知单。详情请登录安全管理系统查看并急时整改！");
                }
            }
            return i;
        }

        /// <summary>
        /// 整改后 整改负责人的回复信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="y_id"></param>
        /// <returns></returns>
        public int ZhengGai(dynamic data,int y_id)
        {
            if (ReStatus(y_id) == 2)
            {//表单已闭环 不能修改
                return 100;
            }
            Entity.yhtable y = yh.Value.SetData(data);
            string sql = "update yhtable set y_zgdesc='"+y.y_zgdesc + "',y_zgtime="+y.y_zgtime + ",y_zgimg='"+y.y_zgimg+"', " +
                "y_status=3 where y_id=" + y_id+" ";
            int i = help.Count(sql);

            ///////发送信息
            string sql2 = "select b.mobile from yhtable a " +
                            "left join user_detail b " +
                            "on a.y_usid = b.us_id where y_id = " + y_id;
            object objs = help.FirstRow(sql2);
            if (objs != null)
            {
                if (objs.ToString()!="") {
                    sendXinXi(objs.ToString(), "您收到了来自整改负责人的整改回复信息，请前往系统查看并确认！");
                }
            }
            return i;
        }

        /// <summary>
        /// 整改确认
        /// </summary>
        /// <param name="data">确认说明，整改后图片，检查表单状态，确认人ID，确认时间</param>
        /// <param name="y_id"></param>
        /// <returns></returns>
        public int QueRen(dynamic data,int y_id)
        {
            if (ReStatus(y_id) == 2)
            {//表单已闭环 不能修改
                return 100;
            }
            Entity.yhtable y = yh.Value.SetData(data);
            string sql = "update yhtable set y_qrdesc='"+y.y_qrdesc + "',y_qrimg='"+y.y_qrimg + "',y_status="+y.y_status + "," +
                "y_qruser="+y.y_qruser + ",y_qrtime="+y.y_qrtime + " " +
                "where y_id="+y_id;
            return help.Count(sql);
        }


        /// <summary>
        /// 修改检查单  只限检查人修改
        /// </summary>
        /// <param name="data"></param>
        /// <param name="y_id"></param>
        /// <returns></returns>
        public int JianChaUpdate(dynamic data,int y_id)
        {
            if (ReSign(y_id) == 2)
            {//领导已确认 不能修改
                return 100;
            }
            Entity.yhtable y = yh.Value.SetData(data);
            string sql = "update yhtable set y_name='"+y.y_name+"',y_body='"+y.y_body+"',y_area='"+y.y_area+"'," +
                "y_type='"+y.y_type+"',y_dengji="+y.y_dengji+",y_desc='"+y.y_desc+"',y_endtime="+y.y_endtime+"," +
                "y_zguser="+y.y_zguser+",y_yaoqiu='"+y.y_yaoqiu+"',y_img1='"+y.y_img1+"' " +
                "where y_id="+y_id;
            return help.Count(sql);
        }

        /// <summary>
        /// 查询查询单
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="verify"></param>
        /// <param name="head_id"></param>
        /// <returns></returns>
        public HttpResponseMessage Query(int us_id,int verify,int head_id,int page,int count)
        {
            int p = (page - 1) * count + 1;
            int c = page * count;
            string where = "";
            ///通过不同的员工权限，查询不同的数据
            if (verify == 0 || verify==1)
            {
                where = "";
            }
            else {
                where = " and y_usid="+us_id+ "  ";
            }
            ///查询数据的总数量
            string sqlc = "select count(y_id) from yhtable where y_headid="+head_id+" "+where;

            string sql = "select * from ( "+
                        "select *,row_number() over(order by y_id desc) as row, " +
                        "(select real_name from users where us_id = y_usid) as jiancha_name, " +
                        "(select real_name from users where us_id = y_headuser) as head_username, " +
                        "(select real_name from users where us_id = y_zguser) as zhenggai_name, " +
                        "(select real_name from users where us_id = y_qruser) as queren_name " +
                        "from yhtable where y_headid="+head_id+" "+where+" ) temp " +
                        "where row between "+p+" and "+c;

            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    count = Convert.ToInt32(help.FirstRow(sqlc)),
                    data = ConvertToEntity<Entity.yhtable>.Convert(dt)
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="没有数据信息"
                };
            }

            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 通过隐患表的id查询隐患详细信息
        /// </summary>
        /// <param name="y_id"></param>
        /// <returns></returns>
        public HttpResponseMessage Query(int y_id)
        {
            string sql = "select *,row_number() over(order by y_id desc) as row, " +
                        "(select real_name from users where us_id = y_usid) as jiancha_name, " +
                        "(select real_name from users where us_id = y_headuser) as head_username, " +
                        "(select real_name from users where us_id = y_zguser) as zhenggai_name, " +
                        "(select real_name from users where us_id = y_qruser) as queren_name " +
                        "from yhtable where y_id="+y_id;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.yhtable>.Convert(dt)
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
        /// 数据统计
        /// </summary>
        /// <param name="us_id">用户的id</param>
        /// <param name="verify">用户的权限</param>
        /// <returns></returns>
        public HttpResponseMessage QueryGroup(int us_id,int verify,int head_id)
        {
            string where = "";
            if (verify!=0 && verify!=1) {
                where += " and y_usid="+us_id+"";
            }
            //and (y_status=1 or y_status=1) and (y_headuser=0 or y_headtype=2) 
            string mysql = "select count(y_usid) u_count," +
                            "(select count(y_headuser) from yhtable where y_headuser = "+us_id+ " and (y_headtype=0 or y_headtype=1 )) as h_count," +
                            "(select count(y_zguser) from yhtable where y_zguser = "+us_id+ " and (y_status=0 or y_status=1) and (y_headuser=0 or y_headtype=2)) as z_count," +
                            "(select count(y_qruser) q from yhtable where y_usid = "+us_id+ " and y_status=3) as q_count " +
                            "from yhtable where y_headid="+head_id+" " + where+" ";
            DataTable dt = help.Totable(mysql);
            obj = new {
                code = 0,
                data = ConvertToEntity<yhtj>.Convert(dt)
            };
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 统计用户的检查数量 按年月进行分组，并查询最近12个月的数拓信息
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="verify"></param>
        /// <param name="head_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryUnix(int us_id,int verify,int head_id)
        {
            string where = "";
            if (verify != 0 && verify != 1)
            {
                where += " and y_usid=" + us_id + " ";
            }
            string sql = "SELECT top 12 cast(year(DATEADD(s,y_createtime,'1970-01-01 00:00:00')) as varchar(10))+'年'+ "+
                        "cast(month(DATEADD(s, y_createtime, '1970-01-01 00:00:00')) as varchar(10)) + '月' as dates, " +
                        "count(y_id) as count from yhtable " +
                        "where y_headid="+head_id+" "+where+"" +
                        "group by year(DATEADD(s, y_createtime, '1970-01-01 00:00:00')), " +
                        "month(DATEADD(s, y_createtime, '1970-01-01 00:00:00')) " +
                        "order by " +
                        "year(DATEADD(s, y_createtime, '1970-01-01 00:00:00')), " +
                        "month(DATEADD(s, y_createtime, '1970-01-01 00:00:00')) asc";

            DataTable dt = help.Totable(sql);
            obj = new {
                code = 0,
                data = ConvertToEntity<Yh_group>.Convert(dt)
            };
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询出所有检查出来的隐患分类
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryType(int head_id)
        {
            string sql = "select y_type from yhtable where y_headid="+head_id;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.yhtable>.Convert(dt)
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
        /// 查询需要负责人签字的表单
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="y_headtype">上级领导签字状态 0 待确认 1 不同意 2 同意</param>
        /// <returns></returns>
        public HttpResponseMessage QueryHeads(int us_id,int y_headtype,int page,int count)
        {
            int p = (page - 1) * count + 1;
            int c = page * count;
            string type = "";
            if (y_headtype==0) {
                type = " (y_headtype=0 or y_headtype=1)";
            }

            string sqlc = "select count(y_id) from yhtable where y_headuser="+us_id+" and y_status!=2 and "+type+" ";
            string sql = "select * from ( " +
                        "select *,row_number() over(order by y_id desc) as row, " +
                        "(select real_name from users where us_id = y_usid) as jiancha_name, " +
                        "(select real_name from users where us_id = y_headuser) as head_username, " +
                        "(select real_name from users where us_id = y_zguser) as zhenggai_name, " +
                        "(select real_name from users where us_id = y_qruser) as queren_name " +
                        "from yhtable where y_headuser=" + us_id + " and y_status!=2 and "+type+"  ) temp " +
                        "where row between " + p + " and " + c;

            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    count = Convert.ToInt32(help.FirstRow(sqlc)),
                    data = ConvertToEntity<Entity.yhtable>.Convert(dt)
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
        /// 查询我要整改或已整改的
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="y_status">0待整改 1 重新整改 2 完成 3已整改待确认</param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryZhengGai(int us_id, int y_status, int page, int count)
        {
            int p = (page - 1) * count + 1;
            int c = page * count;
            var where = "";
            if (y_status==0) {
                where = " and (y_status=0 or y_status=1) and (y_headuser=0 or y_headtype=2) ";
            }

            string sqlc = "select count(y_id) from yhtable where y_zguser=" + us_id + " "+where+" ";
            string sql = "select * from ( " +
                        "select *,row_number() over(order by y_id desc) as row, " +
                        "(select real_name from users where us_id = y_usid) as jiancha_name, " +
                        "(select real_name from users where us_id = y_headuser) as head_username, " +
                        "(select real_name from users where us_id = y_zguser) as zhenggai_name, " +
                        "(select real_name from users where us_id = y_qruser) as queren_name " +
                        "from yhtable where y_zguser=" + us_id + " "+where+"  ) temp " +
                        "where row between " + p + " and " + c;

            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    count = Convert.ToInt32(help.FirstRow(sqlc)),
                    data = ConvertToEntity<Entity.yhtable>.Convert(dt)
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
        /// 查询我要整改或已整改的
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="y_status">0待整改 1 重新整改 2 完成 3已整改待确认</param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryQueRen(int us_id, int page, int count)
        {
            int p = (page - 1) * count + 1;
            int c = page * count;

            string sqlc = "select count(y_id) from yhtable where y_usid=" + us_id + " and y_status=3 ";
            string sql = "select * from ( " +
                        "select *,row_number() over(order by y_id desc) as row, " +
                        "(select real_name from users where us_id = y_usid) as jiancha_name, " +
                        "(select real_name from users where us_id = y_headuser) as head_username, " +
                        "(select real_name from users where us_id = y_zguser) as zhenggai_name, " +
                        "(select real_name from users where us_id = y_qruser) as queren_name " +
                        "from yhtable where y_usid=" + us_id + " and y_status=3  ) temp " +
                        "where row between " + p + " and " + c;

            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    count = Convert.ToInt32(help.FirstRow(sqlc)),
                    data = ConvertToEntity<Entity.yhtable>.Convert(dt)
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
        /// 返回检查表单的状态 如果 返回2 说明该检查已闭环
        /// </summary>
        /// <param name="data"></param>
        /// <param name="y_id"></param>
        /// <returns></returns>
        public int ReStatus(int y_id)
        {
            //查询该检查表的状态
            string sql = "select y_status from yhtable where y_id="+y_id;
            object o = help.FirstRow(sql);
            if (o != null)
            {
                return Convert.ToInt32(o);
            }
            else {
                return 100;
            }
        }

        /// <summary>
        /// 返回检查部门负责人签字状态 如果 返回2 说明已同意，检查人没将不能对检查表单进行修改
        /// </summary>
        /// <param name="data"></param>
        /// <param name="y_id"></param>
        /// <returns></returns>
        public int ReSign(int y_id)
        {
            //查询该检查表的状态
            string sql = "select y_headtype from yhtable where y_id=" + y_id;
            object o = help.FirstRow(sql);
            if (o != null)
            {
                return Convert.ToInt32(o);
            }
            else
            {
                return 100;
            }
        }

        /// <summary>
        /// 查询近十天的隐患类型
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage todayTj(int verify,int head_id,int com_id, int b_id,int c_id,int us_id)
        {
            string where = "";
            if (verify == 0)
            {
                where = " and y_usid in (select us_id from user_detail where head_id=" + head_id + ") ";
            }
            else if (verify == 1)
            {
                where = " and  y_usid in (select us_id from user_detail where com_id=" + com_id + ") ";
            }
            else if (verify == 2)
            {
                where = " and  y_usid in (select us_id from user_detail where b_id=" + b_id + ") ";
            }
            else {
                where = " and y_usid="+us_id+" ";
            }
            string sql = "select convert(varchar(10),getdate(), 120) as today, y_type from yhtable where convert(varchar(10),dateadd(s,y_createtime,'1970-01-01'),120)=CONVERT(varchar(10),getdate(),120) "+where+";" +
            "select convert(varchar(10),dateadd(day, -1, getdate()), 120) as today, y_type from yhtable where convert(varchar(10), dateadd(s, y_createtime, '1970-01-01'), 120) = convert(varchar(10), dateadd(day, -1, getdate()), 120) " + where + ";" +
            "select convert(varchar(10),dateadd(day, -2, getdate()), 120) as today, y_type from yhtable where convert(varchar(10), dateadd(s, y_createtime, '1970-01-01'), 120) = convert(varchar(10), dateadd(day, -2, getdate()), 120) " + where + ";" +
            "select convert(varchar(10),dateadd(day, -3, getdate()), 120) as today, y_type from yhtable where convert(varchar(10), dateadd(s, y_createtime, '1970-01-01'), 120) = convert(varchar(10), dateadd(day, -3, getdate()), 120) " + where + ";" +
            "select convert(varchar(10),dateadd(day, -4, getdate()), 120) as today, y_type from yhtable where convert(varchar(10), dateadd(s, y_createtime, '1970-01-01'), 120) = convert(varchar(10), dateadd(day, -4, getdate()), 120) " + where + ";" +
            "select convert(varchar(10),dateadd(day, -5, getdate()), 120) as today, y_type from yhtable where convert(varchar(10), dateadd(s, y_createtime, '1970-01-01'), 120) = convert(varchar(10), dateadd(day, -5, getdate()), 120) " + where + ";" +
            "select convert(varchar(10),dateadd(day, -6, getdate()), 120) as today, y_type from yhtable where convert(varchar(10), dateadd(s, y_createtime, '1970-01-01'), 120) = convert(varchar(10), dateadd(day, -6, getdate()), 120) " + where + ";" +
            "select convert(varchar(10),dateadd(day, -7, getdate()), 120) as today, y_type from yhtable where convert(varchar(10), dateadd(s, y_createtime, '1970-01-01'), 120) = convert(varchar(10), dateadd(day, -7, getdate()), 120) " + where + ";" +
            "select convert(varchar(10),dateadd(day, -8, getdate()), 120) as today, y_type from yhtable where convert(varchar(10), dateadd(s, y_createtime, '1970-01-01'), 120) = convert(varchar(10), dateadd(day, -8, getdate()), 120) " + where + ";" +
            "select convert(varchar(10),dateadd(day, -9, getdate()), 120) as today, y_type from yhtable where convert(varchar(10), dateadd(s, y_createtime, '1970-01-01'), 120) = convert(varchar(10), dateadd(day, -9, getdate()), 120) " + where + "; ";
            DataSet ds = help.ToDataSet(sql);
            obj = new {
                code=0,
                today=ConvertToEntity<Yh_SetData>.Convert(ds.Tables[0]),
                today_1 = ConvertToEntity<Yh_SetData>.Convert(ds.Tables[1]),
                today_2 = ConvertToEntity<Yh_SetData>.Convert(ds.Tables[2]),
                today_3 = ConvertToEntity<Yh_SetData>.Convert(ds.Tables[3]),
                today_4 = ConvertToEntity<Yh_SetData>.Convert(ds.Tables[4]),
                today_5 = ConvertToEntity<Yh_SetData>.Convert(ds.Tables[5]),
                today_6 = ConvertToEntity<Yh_SetData>.Convert(ds.Tables[6]),
                today_7 = ConvertToEntity<Yh_SetData>.Convert(ds.Tables[7]),
                today_8 = ConvertToEntity<Yh_SetData>.Convert(ds.Tables[8]),
                today_9 = ConvertToEntity<Yh_SetData>.Convert(ds.Tables[9])
            };
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询每年十二个月的数据
        /// </summary>
        /// <param name="verify"></param>
        /// <param name="head_id"></param>
        /// <param name="com_id"></param>
        /// <param name="b_id"></param>
        /// <param name="c_id"></param>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage MonthTj(int verify, int head_id, int com_id, int b_id, int c_id, int us_id,string year)
        {
            string where = "";
            if (year == "" && year == null)
            {
                where += " datepart(year, dateadd(s, y_createtime, '1970-01-01')) = datepart(year,getdate()) ";
            }
            else {
                where = " datepart(year, dateadd(s, y_createtime, '1970-01-01')) = '"+year+"' ";
            }

            if (verify == 0)
            {
                where += " and y_usid in (select us_id from user_detail where head_id=" + head_id + ") ";
            }
            else if (verify == 1)
            {
                where += " and  y_usid in (select us_id from user_detail where com_id=" + com_id + ") ";
            }
            else if (verify == 2)
            {
                where += " and  y_usid in (select us_id from user_detail where b_id=" + b_id + ") ";
            }
            else
            {
                where += " and y_usid=" + us_id + " ";
            }
            
            string sql = "select sum(case when  datepart(month,dateadd(s,y_createtime,'1970-01-01'))=1 then 1 else 0 end) as m1,"+
        "sum(case when  datepart(month, dateadd(s, y_createtime, '1970-01-01')) = 2 then 1 else 0 end) as m2," +
        "sum(case when  datepart(month, dateadd(s, y_createtime, '1970-01-01')) = 3 then 1 else 0 end) as m3," +
        "sum(case when  datepart(month, dateadd(s, y_createtime, '1970-01-01')) = 4 then 1 else 0 end) as m4," +
        "sum(case when  datepart(month, dateadd(s, y_createtime, '1970-01-01')) = 5 then 1 else 0 end) as m5," +
        "sum(case when  datepart(month, dateadd(s, y_createtime, '1970-01-01')) = 6 then 1 else 0 end) as m6," +
        "sum(case when  datepart(month, dateadd(s, y_createtime, '1970-01-01')) = 7 then 1 else 0 end) as m7," +
        "sum(case when  datepart(month, dateadd(s, y_createtime, '1970-01-01')) = 8 then 1 else 0 end) as m8," +
        "sum(case when  datepart(month, dateadd(s, y_createtime, '1970-01-01')) = 9 then 1 else 0 end) as m9," +
        "sum(case when  datepart(month, dateadd(s, y_createtime, '1970-01-01')) = 10 then 1 else 0 end) as m10," +
        "sum(case when  datepart(month, dateadd(s, y_createtime, '1970-01-01')) = 11 then 1 else 0 end) as m11," +
        "sum(case when  datepart(month, dateadd(s, y_createtime, '1970-01-01')) = 12 then 1 else 0 end) as m12 " +
        "from yhtable " +
        "where  "+where+" ";
            obj = new {
                code=0,
                data=ConvertToEntity<Yh_month>.Convert(help.Totable(sql))
            };
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询近十天的每天的隐患数据量
        /// </summary>
        /// <param name="verify"></param>
        /// <param name="head_id"></param>
        /// <param name="com_id"></param>
        /// <param name="b_id"></param>
        /// <param name="c_id"></param>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage YhCountTen(int verify, int head_id, int com_id, int b_id, int c_id, int us_id)
        {
            string where = "";
            if (verify == 0)
            {
                where = " and y_usid in (select us_id from user_detail where head_id=" + head_id + ") ";
            }
            else if (verify == 1)
            {
                where = " and  y_usid in (select us_id from user_detail where com_id=" + com_id + ") ";
            }
            else if (verify == 2)
            {
                where = " and  y_usid in (select us_id from user_detail where b_id=" + b_id + ") ";
            }
            else
            {
                where = " and y_usid=" + us_id + " ";
            }
            string sql = "select CONVERT(varchar(10),dateadd(s,y_createtime,'1970-01-01'),120) as today,count(y_id) as count from yhtable "+
"where datediff(day, dateadd(s, y_createtime,'1970-01-01'),getdate())<= 10 and  " +
"datediff(day, dateadd(s, y_createtime, '1970-01-01'), getdate()) >= 0 "+where+" group by CONVERT(varchar(10), dateadd(s, y_createtime, '1970-01-01'), 120)";
            obj = new {
                code=0,
                data=ConvertToEntity<Yh_SetData>.Convert(help.Totable(sql))
            };
            return Zh.Tool.Json.GetJson(obj);
        }



    }


    public class yhtj
    {
        public int u_count { get; set; }
        public int h_count { get; set; }
        public int z_count { get; set; }
        public int q_count { get; set; }
    }

    public class Yh_group
    {
        public string dates { get; set; }
        public int count { get; set; }
    }

    public class Yh_SetData
    {
        public string today { get; set; }
        public string y_type { get; set; }
        public int count { get; set; }
    }

    public class Yh_month
    {
        public int m1 { get; set; }
        public int m2 { get; set; }
        public int m3 { get; set; }
        public int m4 { get; set; }
        public int m5 { get; set; }
        public int m6 { get; set; }
        public int m7 { get; set; }
        public int m8 { get; set; }
        public int m9 { get; set; }
        public int m10 { get; set; }
        public int m11 { get; set; }
        public int m12 { get; set; }
    }
}