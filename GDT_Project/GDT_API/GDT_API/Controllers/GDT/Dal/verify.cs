using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;

namespace GDT_API.Controllers.GDT.Dal
{
    public class verify
    {
        public static string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private List<Entity.user_detail> user = null;
        private static Zh.Tool.SqlHelper help = null;

        public verify()
        {
            if (help==null) {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }

        /// <summary>
        /// 通过usid查询登录会员的所有信息，包括总部的信息和权限等
        /// </summary>
        /// <returns></returns>
        public List<Entity.user_detail> GetUserVal(int usid)
        {
            string sql = "select * from users a " +
                "left join user_detail b " +
                "left join head_office c on b.head_id=c.head_id on a.us_id=b.us_id " +
                "where a.us_id="+usid;
            DataTable dt= help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
               user= GDT_API.ConvertToEntity<Entity.user_detail>.Convert(dt);
                
                return user;
            }
            else {
                return null;
            }
        }
    }
}