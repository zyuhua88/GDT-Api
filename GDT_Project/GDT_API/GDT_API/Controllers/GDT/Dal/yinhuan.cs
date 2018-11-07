using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace GDT_API.Controllers.GDT.Dal
{
    public class yinhuan
    {
        private Lazy<Entity.yinhuan_down> down = new Lazy<Entity.yinhuan_down>();
        private Zh.Tool.SqlHelper help=null;
        private string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private object obj = null;

        public yinhuan()
        {
            if (help==null) {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }

        #region  ===========================================添加数据相关==================
        /// <summary>
        /// 添加隐患信息表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponseMessage AddYinHuan(dynamic data)
        {
            Deparment de = new Deparment();
            
            Entity.yinhuan y = down.Value.SetData(data);
            
            string sql = "insert into yinhuan(yh_no,yh_applicationName,yh_body,yh_create_time,yh_area,yh_type,yh_dengji,yh_desc,yh_img," +
                "yh_imgodd,yh_timestart,yh_timeend,yh_user_from,yh_deparment,yh_to_deparment,yh_yaoqiu,yh_to_user,yh_shishi_time," +
                "yh_zhenggaihou,yh_queren_user,yh_queren_time,yh_send_state) values('"+data.yh_no+"',@yh_applicationName,@yh_body,"+y.yh_create_time+"," +
                "@yh_area,@yh_type,"+y.yh_dengji+",@yh_desc,@yh_img," +
                "@yh_imgodd,"+y.yh_timestart+","+y.yh_timeend+","+y.yh_user_from+","+y.yh_deparment+","+y.yh_to_deparment+"," +
                "@yh_yaoqiu,"+y.yh_to_user+"," +
                ""+y.yh_shishi_time+"," +
                "@yh_zhenggaihou,"+y.yh_queren_user+","+y.yh_queren_time+","+y.yh_send_state+")";
            SqlParameter[] sp = {
                new SqlParameter("@yh_applicationName",y.yh_applicationName),
                new SqlParameter("@yh_body",y.yh_body),
                new SqlParameter("@yh_area",y.yh_area),
                new SqlParameter("@yh_type",y.yh_type),
                new SqlParameter("@yh_desc",y.yh_desc),
                new SqlParameter("@yh_img",y.yh_img),
                new SqlParameter("@yh_imgodd",y.yh_imgodd),
                new SqlParameter("@yh_yaoqiu",y.yh_yaoqiu),
                new SqlParameter("@yh_zhenggaihou",y.yh_zhenggaihou)
            };
            int i= help.Count(sql,sp);
            if (i > 0)
            {
                if (y.yh_send_state==1) {
                    int upadmin = de.QueryAdminId(y.yh_deparment);//得到我要上报的领导ID编号
                    AddYinHuan_up(data, upadmin);//添加要上报通知的领导
                }

                obj = new
                {
                    code = 0,
                    msg = "保存成功"
                };
                
            }
            else {
                obj = new
                {
                    code = 1,
                    msg = "保存失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 添加审批上报表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponseMessage AddYinHuan_up(dynamic data,int touser)
        {
            Entity.yinhuan_down u = down.Value.SetData(data);
            string sql = "insert into yinhuan_up(yh_no,yh_to_userup,yh_state,yh_time) values(" +
                "@yh_no,"+touser+","+u.yh_state+","+u.yh_time+")";
            SqlParameter[] sp = {
                new SqlParameter("@yh_no",u.yh_no)
            };
            int i = help.Count(sql,sp);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "上报成功"
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "上报失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 上传整改表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponseMessage AddYinHuan_down(dynamic data)
        {
            Entity.yinhuan_down d = down.Value.SetData(data);
            string sql = "insert into yinhuan_down(yh_no,yh_to_userdown,yh_state,yh_time) values(@yh_no," +
                ""+d.yh_to_userdown+","+d.yh_state+","+d.yh_time+")";
            SqlParameter[] sp = {
                new SqlParameter("@yh_no",d.yh_no)
            };
            int i = help.Count(sql, sp);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "发送成功"
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "发送失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        #endregion

        #region =============================更改数据信息=================================
        /// <summary>
        /// 修改隐患信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="yh_no"></param>
        /// <returns></returns>
        public HttpResponseMessage UpdateYinHuan(dynamic data,string yh_no)
        {
            Deparment de = new Deparment();
            Entity.yinhuan_down y = down.Value.SetData(data);
            string sql = "update yinhuan set yh_applicationName=@yh_applicationName,yh_body=@yh_body,yh_create_time=" + y.yh_create_time + "," +
                "yh_area=@yh_area,yh_type=@yh_type,yh_dengji=" + y.yh_dengji + ",yh_desc=@yh_desc,yh_img=@yh_img," +
                "yh_imgodd=@yh_imgodd,yh_timestart=" + y.yh_timestart + ",yh_timeend=" + y.yh_timeend + "," +
                "yh_user_from=" + y.yh_user_from + ",yh_deparment=" + y.yh_deparment + ",yh_to_deparment=" + y.yh_to_deparment + "," +
                "yh_yaoqiu=@yh_yaoqiu,yh_to_user=" + y.yh_to_user + ",yh_shishi_time=" + y.yh_shishi_time + "," +
                "yh_zhenggaihou=@yh_zhenggaihou,yh_queren_user=" + y.yh_queren_user + ",yh_queren_time=" + y.yh_queren_time + "," +
                "yh_send_state=" + y.yh_send_state + " where yh_no=@yh_no";
            SqlParameter[] sp = {
                new SqlParameter("@yh_no",yh_no),
                new SqlParameter("@yh_applicationName",y.yh_applicationName),
                new SqlParameter("@yh_body",y.yh_body),
                new SqlParameter("@yh_area",y.yh_area),
                new SqlParameter("@yh_type",y.yh_type),
                new SqlParameter("@yh_desc",y.yh_desc),
                new SqlParameter("@yh_img",y.yh_img),
                new SqlParameter("@yh_imgodd",y.yh_imgodd),
                new SqlParameter("@yh_yaoqiu",y.yh_yaoqiu),
                new SqlParameter("@yh_zhenggaihou",y.yh_zhenggaihou)
            };
            int i = help.Count(sql, sp);
            if (i > 0)
            {
                if (y.yh_send_state==1) {
                    int upadmin = de.QueryAdminId(y.yh_deparment);//得到我要上报的领导ID编号
                    int t = Zh.Tool.Date_Tool.TimeToInt(DateTime.Now);
                    sql = "insert into yinhuan_up(yh_no,yh_to_userup,yh_state,yh_time) values " +
                        "('"+y.yh_no+"',"+upadmin+",0,"+t+")";
                    help.Count(sql);
                }
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
        /// 修改审批信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="yh_upid"></param>
        /// <returns></returns>
        public HttpResponseMessage UpdateYinHuan_up(dynamic data,int yh_upid)
        {
            Entity.yinhuan_down u = down.Value.SetData(data);
            string sql = "update yinhuan_up set yh_no=@yh_no,yh_to_user=" + u.yh_to_user + "," +
                "yh_state=" + u.yh_state + ",yh_time=" + u.yh_time + " where yh_upid="+yh_upid+"";
            SqlParameter[] sp = {
                new SqlParameter("@yh_no",u.yh_no)
            };
            int i = help.Count(sql, sp);
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
        /// 通过并转发给相关实施负责人
        /// </summary>
        /// <param name="yh_no"></param>
        /// <param name="yh_to_userdown">要通知的整改负责人ID</param>
        /// <returns></returns>
        public HttpResponseMessage UpdateYinHuan_up(string yh_no,int yh_to_userdown)
        {
            string sql = "update yinhuan_up set yh_state=2 where yh_no='"+yh_no+"'";
            int i= help.Count(sql);
            if (i > 0)
            {
                
                int yh_time = Zh.Tool.Date_Tool.TimeToInt(DateTime.Now);
                sql = "insert into yinhuan_down(yh_no,yh_to_userdown,yh_state,yh_time) " +
                    "values('" + yh_no + "'," + yh_to_userdown + ",0," + yh_time + ")";
                int ii= help.Count(sql);
                if (ii > 0)
                {
                    obj = new
                    {
                        code = 0,
                        msg = "审批成功,并已成功下达整改通知"
                    };
                }
                else {
                    obj = new
                    {
                        code = 0,
                        msg = "审批成功,但在下达整改通知时退出"
                    };
                }
                
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "审批失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 修改整改信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="yh_downid"></param>
        /// <returns></returns>
        public HttpResponseMessage UpdateYinHuan_down(dynamic data,int yh_downid)
        {
            Entity.yinhuan_down d = down.Value.SetData(data);
            string sql = "update yinhuan_down set yh_no=@yh_no,yh_to_user=" + d.yh_to_user + "," +
                "yh_state=" + d.yh_state + ",yh_time=" + d.yh_time + " where yh_downid="+ yh_downid + "";
            SqlParameter[] sp = {
                new SqlParameter("@yh_no",d.yh_no)
            };
            int i = help.Count(sql, sp);
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
        /// 修改鉴定信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="yh_no"></param>
        /// <returns></returns>
        public HttpResponseMessage UpdateJianDing(dynamic data,string yh_no)
        {
            int yh_queren_user = Convert.ToInt32(data.yh_queren_user);
            int yh_queren_time = Zh.Tool.Date_Tool.TimeToInt(data.yh_queren_time);
            int yh_send_state = Convert.ToInt32(data.yh_send_state);
            string sql = "update yinhuan set yh_queren_user="+ yh_queren_user + ",yh_queren_time="+ yh_queren_time + "," +
                "yh_send_state="+ yh_send_state + " where yh_no='"+yh_no+"'";
            int i= help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "鉴定完成"
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    msg = "鉴定失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }



        #endregion

        #region ======================撤消及删除=================================

        /// <summary>
        /// 删除我上报的隐患表单 同时删除已上报和已下发的表单
        /// </summary>
        /// <param name="yh_no"></param>
        /// <returns></returns>
        public HttpResponseMessage DelYinHuan(string yh_no)
        {
            string sql = "delete from yinhuan where yh_no=@yh_no;";
            sql += "delete from yinhuan_up where yh_no=@yh_no;";
            sql += "delete from yinhuan_down where yn_no=@yh_no;";
            SqlParameter[] sp = {
                new SqlParameter("@yh_no",yh_no)
            };
            int i = help.Count(sql, sp);
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
        /// 撤消我上报的隐患表单 （删除已上报和已下发的表单）并修改表单状态数据为 2 已撤消
        /// </summary>
        /// <param name="yh_no"></param>
        /// <returns></returns>
        public HttpResponseMessage BackYinHuan(string yh_no)
        {
            string sql = "delete from yinhuan_down where yh_no=@yh_no;";
            sql += "delete from yinhuan_up where yh_no=@yn_no;";
            sql += "update yinhuan set yh_send_state=2 where yh_no=@yn_no;";
            SqlParameter[] sp = {
                new SqlParameter("@yh_no",yh_no)
            };
            int i = help.Count(sql, sp);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "撤消成功"
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "撤消失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        #endregion

        #region ================数据查询相关=============================

        /// <summary>
        /// 查询相关数据的数量
        /// </summary>
        /// <param name="data">两个值 value 搜索关键词  userInfo 登录用户的信息 ｛｝</param>
        /// <param name="types">标识  1为只查询个人的 否则查询下属的</param>
        /// <param name="yh_send_state">隐患处理的状态</param>
        /// <returns></returns>
        public int QueryCount(dynamic data,int types,int yh_send_state)
        {
            JObject array = data.userInfo;
            string value = data.value;

            string usersql = SQLWhere.UserSql(array);
            int us_id = Convert.ToInt32(array["us_id"]);


            string where = "where a.yh_id>0 ";
            if (value!="" && value!=null) {
                where += " and a.yh_applicationName like '%" + value + "%' ";
            }
            if (types == 1)
            {//只查询我个人的信息
                where += " and a.yh_user_from=" + us_id + " ";
            }
            else {//查询我下属发布的信息
                where += " and (a.yh_user_from in (" + usersql + ") or a.yh_queren_user in (" + usersql + ") ) ";
                
            }
            where += " and a.yh_send_state=" + yh_send_state + " ";

            string sql = "select count(a.yh_id) from yinhuan a "+where+" ";
            return Convert.ToInt32(help.FirstRow(sql));
        }

        /// <summary>
        /// 查询隐患信息 只限于查询已发布的和已完成的 也就是说send_state==1 或者==3
        /// </summary>
        /// <param name="data">data里携带一个值value为用户检索的数据信息,userinfo用户的信息，是一个｛｝object类型</param>
        /// <returns></returns>
        public HttpResponseMessage QuerySend(dynamic data,int page,int count,int yh_send_state)
        {
            if (yh_send_state!=1 && yh_send_state!=3) {
                obj = new {
                    code=1,
                    msg="该接口只限于查询已发布和已完成的信息，对应值应该为1或3。"
                };
                return Zh.Tool.Json.GetJson(obj);
            }

            int p = (page - 1) * count + 1;
            int c = page * count;
            JObject array = data.userInfo;
            string where = "where a.yh_id>0 ";
            //where +=" and"= SQLWhere.UserSql(array);
            string usersql = SQLWhere.UserSql(array);

            string value = data.value;
            if (value!="" && value!=null) {
                where += " and a.yh_applicationName like '%" + value + "%' ";
            }
            where += " and (a.yh_user_from in (" + usersql+ ") or a.yh_queren_user in ("+usersql+ ") ) ";
            where += " and a.yh_send_state="+yh_send_state+" ";

            string sql = "select * from( "+
                        "select a.*,b.yh_to_userup,b.yh_state as state_up,b.yh_time as time_up, " +
                        "c.yh_state as state_down,c.yh_time as time_down,c.yh_to_userdown, " +
                        "row_number() over(order by yh_id desc) as row from yinhuan a " +
                        "left join yinhuan_up b on a.yh_no = b.yh_no " +
                        "left join yinhuan_down c on a.yh_no = c.yh_no " +
                        ""+where+") temp " +
                        "where row between "+p+" and "+c+" ";
            DataTable dt= help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "成功读取数据",
                    count=QueryCount(data,2,yh_send_state),
                    data = ConvertToEntity<Entity.yinhuan_down>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    msg = "没有查询到相关数据信息"
                };
            }

            return Zh.Tool.Json.GetJson(obj);

        }


        /// <summary>
        /// 查询隐患信息 只限于查询未发布的和已撤消的 也就是说send_state==0 或者==2
        /// </summary>
        /// <param name="data">data里携带一个值value为用户检索的数据信息,userinfo用户的信息，是一个｛｝object类型</param>
        /// <returns></returns>
        public HttpResponseMessage QueryNoSend(dynamic data, int page, int count, int yh_send_state)
        {
            if (yh_send_state != 0 && yh_send_state != 2)
            {
                obj = new
                {
                    code = 1,
                    msg = "该接口只限于查询已发布和已完成的信息，对应值应该为1或3。"
                };
                return Zh.Tool.Json.GetJson(obj);
            }

            int p = (page - 1) * count + 1;
            int c = page * count;
            JObject array = data.userInfo;
            int us_id =Convert.ToInt32(array["us_id"]);
            string sql = "select * from( " +
                        "select a.*,b.yh_to_userup,b.yh_state as state_up,b.yh_time as time_up, " +
                        "c.yh_state as state_down,c.yh_time as time_down,c.yh_to_userdown, " +
                        "row_number() over(order by yh_id desc) as row from yinhuan a " +
                        "left join yinhuan_up b on a.yh_no = b.yh_no " +
                        "left join yinhuan_down c on a.yh_no = c.yh_no " +
                        "where a.yh_user_from="+us_id+ " and a.yh_send_state="+yh_send_state+") temp " +
                        "where row between " + p + " and " + c + " ";
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    count= count = QueryCount(data, 1, yh_send_state),
                    msg = "成功读取数据",
                    data = ConvertToEntity<Entity.yinhuan_down>.Convert(dt)
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "没有查询到相关数据信息"
                };
            }

            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 查询需要我整改的项目
        /// </summary>
        /// <param name="data">data.value,data.userInfo</param>
        /// <param name="state">处理状态</param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryDown(dynamic data,int state, int page, int count)
        {
            int p = (page - 1) * count + 1;
            int c = page * count;
            JObject array = data.userInfo;
            string where = SQLWhere.UserSql(array);
            string sql = "select * from ( "+
                "select b.*,a.yh_state,a.yh_downid,a.yh_time, " +
                "row_number() over(order by a.yh_downid desc) as row from yinhuan_down a " +
                "left join yinhuan b on a.yh_no = b.yh_no " +
                "where a.yh_to_userdown in ("+where+ ") and a.yh_state="+state+" " +
                ")temp " +
                "where row between "+p+" and "+c+"";
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "",
                    data = ConvertToEntity<Entity.yinhuan_down>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    msg = "没有查询到待处理数据信息"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
            
        }


        /// <summary>
        /// 查询需要我整改的隐患单
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryDown(int us_id,int page,int count)
        {
            int p = (page - 1) * count + 1;
            int c = page * count;
            string sqlc = "select count(a.yh_downid) from yinhuan_down a " +
                "left join yinhuan b on a.yh_no=b.yh_no " +
                " where a.yh_to_userdown=" + us_id + " and a.yh_state<2 and (b.yh_send_state=1 or b.yh_send_state=4 or b.yh_send_state=5)" +
                "";
            int xc =Convert.ToInt32(help.FirstRow(sqlc));
            string sql = "select * from ( "+
                        "select b.*,a.yh_to_userdown,a.yh_state,a.yh_time,a.yh_downid,  " +
                        "row_number() over(order by a.yh_downid desc) as row from yinhuan_down a " +
                        "left join yinhuan b on a.yh_no = b.yh_no " +
                        "where a.yh_to_userdown = "+us_id+ " and a.yh_state < 2 and (b.yh_send_state=1 or b.yh_send_state=4 or b.yh_send_state=5)) temp " +
                        "where row between "+p+" and "+c+"";
            DataTable dt= help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    count=xc,
                    data = ConvertToEntity<Entity.yinhuan_down>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    msg="无数据"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }





        /// <summary>
        /// 查询需要我审批的项目
        /// </summary>
        /// <param name="data">data.value,data.userInfo</param>
        /// <param name="state">审批状态</param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryUp(dynamic data,int state, int page, int count)
        {
            int p = (page - 1) * count + 1;
            int c = page * count;
            JObject array = data.userInfo;
            string where = SQLWhere.UserSql(array);
            string sql = "select * from ( " +
                "select b.*,a.yh_state,a.yh_downid,a.yh_time, " +
                "row_number() over(order by a.yh_upid desc) as row from yinhuan_up a " +
                "left join yinhuan b on a.yh_no = b.yh_no " +
                "where a.yh_to_userup in (" + where + ") and a.yh_state="+state+" " +
                ")temp " +
                "where row between " + p + " and " + c + "";
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "",
                    data = ConvertToEntity<Entity.yinhuan_down>.Convert(dt)
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "没有查询到待审核数据信息"
                };
            }
            return Zh.Tool.Json.GetJson(obj);

        }

        /// <summary>
        /// 查询需要我审批的隐患单
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public HttpResponseMessage QueryUp(int us_id, int page, int count)
        {
            int p = (page - 1) * count + 1;
            int c = page * count;
            string sqlc = "select count(a.yh_upid) from yinhuan_up a " +
                " left join yinhuan b on a.yh_no=b.yh_no " +
                " where a.yh_to_userup=" + us_id + " and a.yh_state<2 and (b.yh_send_state=1 or b.yh_send_state=4 or b.yh_send_state=5)";
            int xc = Convert.ToInt32(help.FirstRow(sqlc));
            string sql = "select * from ( " +
                        "select b.*,a.yh_to_userup,a.yh_state,a.yh_time,a.yh_upid,  " +
                        "row_number() over(order by a.yh_upid desc) as row from yinhuan_up a " +
                        "left join yinhuan b on a.yh_no = b.yh_no " +
                        "where a.yh_to_userup = "+us_id+ " and a.yh_state < 2 and (b.yh_send_state=1 or b.yh_send_state=4 or b.yh_send_state=5)) temp " +
                        "where row between "+p+" and "+c+"";
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    count=xc,
                    data = ConvertToEntity<Entity.yinhuan_down>.Convert(dt)
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "无数据"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 查询单条隐患信息
        /// </summary>
        /// <param name="yh_no"></param>
        /// <returns></returns>
        public HttpResponseMessage Query(string yh_no)
        {
            string sql = "select *,(select real_name from users where us_id=a.yh_user_from) as yh_user_from_name,"+
                        "(select b_name from deparment where b_id = a.yh_deparment) as yh_deparment_name," +
                        "(select b_name from deparment where b_id = a.yh_to_deparment) as yh_to_deparment_name," +
                        "(select real_name from users where us_id = a.yh_to_user) as yh_to_user_name," +
                        "(select real_name from users where us_id = a.yh_queren_user) as yh_queren_user_name " +
                        "from yinhuan a " +
                        "left join yinhuan_down b on a.yh_no = b.yh_no " +
                        "left join yinhuan_up c on a.yh_no = c.yh_no " +
                        "where a.yh_no = '"+yh_no+"'";
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.yinhuan_down>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code = 0,
                    msg="您查询的信息不存在，或已被删除"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }



        #endregion
    }
}