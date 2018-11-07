using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDT_API.Controllers.GDT.Controller
{
    public class FileListController : ApiController
    {
        private Lazy<Dal.FileList> list = new Lazy<Dal.FileList>();

        /// <summary>
        /// 把文件信息保存到数据库中
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/filelist/gdt/Add")]
        public HttpResponseMessage Add(dynamic data)
        {
            return list.Value.Add(data);
        }

        /// <summary>
        /// 删除指定的文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/filelist/gdt/Del")]
        public HttpResponseMessage Del(int id)
        {
            return list.Value.Del(id);
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
        /// 
        [HttpPost]
        [Route("api/filelist/gdt/Query")]
        public HttpResponseMessage Query(int head_id, string value, string types,int? filetype,int? jibie, int page, int count)
        {
            return list.Value.Query(head_id, value,types,filetype,jibie,page,count);
        }


        /// <summary>
        /// 查询文件列表信息
        /// </summary>
        /// <param name="value">搜索关键词</param>
        /// <param name="types">文件类型</param>
        /// <param name="page">当前页码</param>
        /// <param name="count">每页显示数量</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/filelist/gdt/Query")]
        public HttpResponseMessage Query(int head_id, string value, string types, int page, int count)
        {
            return list.Value.Query(head_id,value,types,page,count);
        }


        /// <summary>
        /// 查询指定文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/filelist/gdt/Query")]
        public HttpResponseMessage Query(int id)
        {
            return list.Value.Query(id);
        }


        /// <summary>
        /// 修改用户的浏览时长
        /// </summary>
        /// <param name="usid"></param>
        /// <param name="fileid"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/filelist/gdt/AddTimeLength")]
        public HttpResponseMessage AddTimeLength(int usid, int fileid, int date)
        {
            return list.Value.AddTimeLength(usid,fileid,date);
        }


        /// <summary>
        /// 查询用户已经浏览的时长
        /// </summary>
        /// <param name="usid"></param>
        /// <param name="fileid"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/filelist/gdt/QueryTimeLine")]
        public int QueryTimeLine(int usid, int fileid)
        {
            return list.Value.QueryTimeLine(usid,fileid);
        }


        /// <summary>
        /// 查询这个文件要求的学习时长
        /// </summary>
        /// <param name="fileid"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/filelist/gdt/QueryFiletime")]
        public int QueryFiletime(int fileid)
        {
            return list.Value.QueryFiletime(fileid);
        }
    }
}
