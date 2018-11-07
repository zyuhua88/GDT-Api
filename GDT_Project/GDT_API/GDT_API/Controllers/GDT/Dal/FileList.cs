using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Http;
using System.Data;

namespace GDT_API.Controllers.GDT.Dal
{
    public class FileList
    {
        private Lazy<Entity.studenttime> s=new Lazy<Entity.studenttime>();
        private Zh.Tool.SqlHelper help = null;
        private string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private object obj = null;

        public FileList()
        {
            if (help == null)
            {
                help = new Zh.Tool.SqlHelper(connstr);
            }
        }


        /// <summary>
        /// 把文件信息保存到数据库中
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponseMessage Add(dynamic data)
        {
            Entity.studenttime st= s.Value.SetData(data);
            
            string sql = "insert into filelist(title,img,filepath,updatetime,types,loaduser,studenttime,head_id,filetype,jibie) " +
                "values('"+st.title+"','"+st.img+"','"+st.filepath+"',"+st.updatetime+",'"+st.types+"',"+st.loaduser+"," +
                ""+st.studenttime+","+st.head_id+","+st.filetype+","+st.jibie+")";
            int i=help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "文件上传成功"
                };
            }
            else {
                obj = new {
                    code=1,
                    msg="文件发布失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);

        }

        /// <summary>
        /// 删除指定的文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Del(int id)
        {
            string sql = "delete from filelist where id="+id;
            int i = help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "文件删除成功"
                };
            }
            else
            {
                obj = new
                {
                    code = 1,
                    msg = "文件删除失败"
                };
            }
            return Zh.Tool.Json.GetJson(obj);
        }

        /// <summary>
        /// 查询文件列表信息
        /// </summary>
        /// <param name="value">搜索关键词</param>
        /// <param name="types">文件类型</param>
        /// <param name="page">当前页码</param>
        /// <param name="count">每页显示数量</param>
        /// <param name="filetype">文件类型 0 普通文件  1 三级教育文件</param>
        /// <param name="jibie">文件级别 0 未选择 1厂级  2 部门级 3 班组级</param>
        /// <returns></returns>
        public HttpResponseMessage Query(int head_id, string value,string types,int? filetype,int? jibie,int page,int count)
        {
            int p = (page - 1) * count + 1;
            int c = page * count;
            string where = "";
            if (value!="" && value!=null) {
                where += " and title like '%"+value+"%' ";
            }
            if (types!="" && types!=null) {
                where += " and types like '%" + types + "%' ";
            }
            if (filetype!=null) {
                where += " and filetype="+filetype+" ";
            }
            if (jibie!=null) {
                where += " and jibie="+jibie+" ";
            }
            string sqlc = "select count(id) from filelist where head_id="+head_id+" "+where;

            string sql = "select * from ( "+
                            "select *,row_number() over(order by id desc) as row,"+
                            "(select count(sid) from studenttime where sid = id) as lookcount, " +
                            "(select real_name from users where us_id=loaduser) as real_name " +
                            "from filelist where head_id="+head_id+" "+where+"" +
                            ") temp " +
                            "where row between "+p+" and "+c+"";
            
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                int noCount = (int)help.FirstRow(sqlc);
                obj = new
                {
                    code = 0,
                    count = noCount,
                    data = ConvertToEntity<Entity.studenttime>.Convert(dt)
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
        /// 查询文件列表信息
        /// </summary>
        /// <param name="value">搜索关键词</param>
        /// <param name="types">文件类型</param>
        /// <param name="page">当前页码</param>
        /// <param name="count">每页显示数量</param>
        /// <param name="filetype">文件类型 0 普通文件  1 三级教育文件</param>
        /// <param name="jibie">文件级别 0 未选择 1厂级  2 部门级 3 班组级</param>
        /// <returns></returns>
        public HttpResponseMessage Query(int head_id, string value, string types, int page, int count)
        {
            int p = (page - 1) * count + 1;
            int c = page * count;
            string where = "";
            if (value != "" && value != null)
            {
                where += " and title like '%" + value + "%' ";
            }
            if (types != "" && types != null)
            {
                where += " and types like '%" + types + "%' ";
            }
            
            string sqlc = "select count(id) from filelist where head_id=" + head_id + " " + where;

            string sql = "select * from ( " +
                            "select *,row_number() over(order by id desc) as row," +
                            "(select count(sid) from studenttime where sid = id) as lookcount, " +
                            "(select real_name from users where us_id=loaduser) as real_name " +
                            "from filelist where head_id=" + head_id + " " + where + "" +
                            ") temp " +
                            "where row between " + p + " and " + c + "";

            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                int noCount = (int)help.FirstRow(sqlc);
                obj = new
                {
                    code = 0,
                    count = noCount,
                    data = ConvertToEntity<Entity.studenttime>.Convert(dt)
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
        /// 查询指定文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Query(int id)
        {
            string sql = "select *,(select count(sid) from studenttime where sid=id) as lookcount," +
                "(select real_name from users where us_id=loaduser) as real_name " +
                "from filelist where id="+id;
            DataTable dt = help.Totable(sql);
            if (dt.Rows.Count > 0)
            {
                obj = new
                {
                    code = 0,
                    data = ConvertToEntity<Entity.studenttime>.Convert(dt)
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
        /// 添加用户看这个资料的时长
        /// </summary>
        /// <param name="fileid"></param>
        /// <param name="usid"></param>
        /// <param name="timelenght"></param>
        /// <returns></returns>
        public HttpResponseMessage AddStudent(int fileid,int usid,int timelenght)
        {
            string sql = "insert into studenttime(fileid,usid,timelenght) values(" +
                " "+fileid+","+usid+","+timelenght+")";
             int i=help.Count(sql);
            if (i > 0)
            {
                obj = new
                {
                    code = 0,
                    msg = "ok"
                };
            }
            else {
                obj = new
                {
                    code = 1,
                    msg = "no"
                };
            }

            return Zh.Tool.Json.GetJson(obj);
        }


        /// <summary>
        /// 修改用户的浏览时长
        /// </summary>
        /// <param name="usid"></param>
        /// <param name="fileid"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public HttpResponseMessage AddTimeLength(int usid,int fileid,int date)
        {
            string sql = "select count(sid) from studenttime where fileid="+fileid+" and usid="+usid+"";
            int tc =Convert.ToInt32(help.FirstRow(sql));
            if (tc == 0)
            {
                return AddStudent(fileid, usid, date);
            }
            else {
                sql = "update studenttime set timelenght=timelenght+" + date + " where fileid="+fileid+" and usid= "+usid+"";
                int i = help.Count(sql);
                if (i > 0)
                {
                    obj = new
                    {
                        code = 0,
                        msg = "ok"
                    };
                }
                else
                {
                    obj = new
                    {
                        code = 1,
                        msg = "no"
                    };
                }

                return Zh.Tool.Json.GetJson(obj);
            }
            
        }


        /// <summary>
        /// 查询用户已经浏览的时长
        /// </summary>
        /// <param name="usid"></param>
        /// <param name="fileid"></param>
        /// <returns></returns>
        public int QueryTimeLine(int usid, int fileid)
        {
            string sql = "select timelenght from studenttime where usid="+usid+" and fileid="+fileid+"";
            object objs = help.FirstRow(sql);
            if (objs == null)
            {
                return 0;
            }
            else {
                return Convert.ToInt32(objs);
            }
        }

        /// <summary>
        /// 查询这个文件要求的学习时长
        /// </summary>
        /// <param name="fileid"></param>
        /// <returns></returns>
        public int QueryFiletime(int fileid)
        {
            string sql = "select studenttime from filelist where id="+fileid+"";
            object objs = help.FirstRow(sql);
            if (objs == null)
            {
                return 0;

            }
            else {
                return Convert.ToInt32(objs);
            }
        }
        
    }
}