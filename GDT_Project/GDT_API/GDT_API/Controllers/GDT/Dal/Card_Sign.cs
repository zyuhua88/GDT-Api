using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Net.Http;

namespace GDT_API.Controllers.GDT.Dal
{
    public class Card_Sign
    {
        private string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private Lazy<Entity.card_sign> sign = new Lazy<Entity.card_sign>();
        private Zh.Tool.SqlHelper help = null;
        private object obj = null;

        public Card_Sign() {
            if (help==null) {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }


        /// <summary>
        /// 为学员添加三级教育学习卡
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(dynamic data)
        {
            Entity.card_sign si = sign.Value.SetData(data);
            string sql = "insert into card_sign(end_time,com_sign,com_time,b_sign,b_time,c_sign,c_time,us_id) values(" +
                ""+si.end_time+",'"+si.com_sign+"',"+si.com_time+",'"+si.b_sign+"',"+si.b_time+",'"+si.c_sign+"',"+si.c_time+"," +
                ""+si.us_id+")";
            int i= help.Count(sql);
            return i;
        }

        /// <summary>
        /// 为学员添加三级教育学习卡
        /// </summary>
        /// <param name="end_time"></param>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public int Add(string end_time,int us_id)
        {
            int e_time = 0;
            if (end_time!="" && end_time!=null) {
                e_time = Zh.Tool.Date_Tool.TimeToInt(end_time);
            }
            string sql = "insert into card_sign(end_time,com_sign,com_time,b_sign,b_time,c_sign,c_time,us_id) values(" +
                "" + e_time + ",'',0,'',0,'',0," +
                "" + us_id + ")";
            int i = help.Count(sql);
            return i;
        }

        /// <summary>
        /// 删除指定的三级教育卡
        /// </summary>
        /// <param name="card_id"></param>
        /// <returns></returns>
        public int Del(int card_id)
        {
            string sql = "delete from card_sign where card_id=" + card_id;
            return help.Count(sql);
        }


        /// <summary>
        /// 查询员工所有没有过期的三级教育卡
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        public HttpResponseMessage Query(int us_id)
        {
            int today = Zh.Tool.Date_Tool.TimeToInt(DateTime.Now);
            string sql = "select a.*,b.real_name from card_sign a " +
                            "left join users b on a.us_id = b.us_id " +
                            "where a.us_id = "+us_id+" and(end_time = 0 or end_time > " + today + ")";
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.card_sign>.Convert(dt)
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="无数据"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询员工的三级教育列表
        /// </summary>
        /// <param name="verify">查看者的权限</param>
        /// <param name="head_id">查看者对应的集团ID</param>
        /// <param name="com_id">查看者对应的子公司ID</param>
        /// <param name="b_id">查看者对应的部门ID</param>
        /// <param name="c_id">查看者对应的班组ID</param>
        /// <param name="us_id">查看者的ID</param>
        /// <param name="status">三级教育完成状态</param>
        /// <returns></returns>
        public HttpResponseMessage Query(int verify,int head_id,int com_id,int b_id,int c_id,int us_id,int status)
        {
            string sql = "select a.us_id, b.real_name,c.* from user_detail a "+
                        "left join users b on a.us_id = b.us_id " +
                        "left join card_sign c on a.us_id = c.us_id " +
                        "where a.head_id="+head_id+" ";
            string where = "";
            if (verify==1) {
                where += " and a.com_id=" + com_id + " ";
            }
            if (verify==2) {
                where += " and a.b_id=" + b_id + " ";
            }
            if (verify==3) {
                where += " and a.c_id=" + c_id + " ";
            }
            if (verify == 4)
            {
                where += " and a.us_id=" + us_id + " ";
            }
            if (status==1) {//查看已完成的 如果为0的话 只查看全部
                where += " and  c.c_time>0 ";
            }
            if (status==2) {//进行中的
                where += " and c.c_sign='' and c.c_time=0 ";
            }
            if (status==3) {//未分配的
                where += " and c.c_sign is null and c.b_sign is null and com_sign is null ";
            }
            where += " order by a.us_id desc ";

            DataTable dt= help.Totable(sql+where);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.card_sign>.Convert(dt)
                };
            }
            else {
                obj = new
                {
                    code=1,
                    msg="无数据"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
            

        }

        /// <summary>
        /// 厂级签字
        /// </summary>
        /// <param name="data"></param>
        /// <param name="card_id"></param>
        /// <returns></returns>
        public int Com_sign(dynamic data,int card_id)
        {
            Entity.card_sign s = sign.Value.SetData(data);
            string sql = "update card_sign set com_sign='"+s.com_sign+"',com_time="+s.com_time+" where card_id="+card_id;
            return help.Count(sql);
        }

        /// <summary>
        /// 部门级签字
        /// </summary>
        /// <param name="data"></param>
        /// <param name="card_id"></param>
        /// <returns></returns>
        public int B_sign(dynamic data, int card_id)
        {
            Entity.card_sign s = sign.Value.SetData(data);
            string sql = "update card_sign set b_sign='" + s.b_sign + "',b_time=" + s.b_time + " where card_id=" + card_id;
            return help.Count(sql);
        }

        /// <summary>
        /// 班组级签字
        /// </summary>
        /// <param name="data"></param>
        /// <param name="card_id"></param>
        /// <returns></returns>
        public int C_sign(dynamic data, int card_id)
        {
            Entity.card_sign s = sign.Value.SetData(data);
            string sql = "update card_sign set c_sign='" + s.c_sign + "',c_time=" + s.c_time + " where card_id=" + card_id;
            return help.Count(sql);
        }
    }
}