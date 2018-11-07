using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace GDT_API.Controllers.GDT.Dal
{
    public static class SQLWhere
    {
        public static string UserSql(JObject array)
        {

            ////查询出用户的级别
            int verify = Convert.ToInt32(array["verify"]);
            int us_id = Convert.ToInt32(array["us_id"]);
            int head_id = Convert.ToInt32(array["head_id"]);
            int com_id = Convert.ToInt32(array["com_id"]);
            int b_id = Convert.ToInt32(array["b_id"]);
            int c_id = Convert.ToInt32(array["c_id"]);

            string sql = "";
            //如果是总部管理员
            if (verify == 0)
            {
                sql = "select us_id from user_detail where head_id="+head_id+" ";
            }
            //如果是公司（厂）级管理员
            if (verify == 1)
            {
                sql = "select us_id from user_detail where com_id=" + com_id + " ";
            }
            //如果是部门（项目）级管理员
            if (verify == 2)
            {
                sql = "select us_id from user_detail where b_id=" + b_id + " ";
            }
            //如果是班组级管理员
            if (verify == 3)
            {
                sql = "select us_id from user_detail where c=" + c_id + " ";
            }
            //如果是一般员工
            if (verify == 3)
            {
                sql = "select us_id from user_detail where us_id=" + us_id + " ";
            }
            return sql;
        }
    }
}