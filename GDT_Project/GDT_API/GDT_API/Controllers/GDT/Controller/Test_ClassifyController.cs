using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDT_API.Controllers.GDT.Controller
{
    public class Test_ClassifyController : ApiController
    {
        private Lazy<Dal.Test_classify> t = new Lazy<Dal.Test_classify>();
        /// <summary>
        /// 上传试题分类名称
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test_classify/gdt/add")]
        public HttpResponseMessage Add(dynamic data)
        {
            return t.Value.Add(data);
        }

        /// <summary>
        /// 修改题库名称信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id">对应题库ID编号</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test_classify/gdt/update")]
        public HttpResponseMessage Update(dynamic data, int id)
        {
            return t.Value.Update(data,id);
        }

        /// <summary>
        /// 更变试题的发布状态 
        /// </summary>
        /// <param name="id">题库ID编号</param>
        /// <param name="is_send">0 为收回发布 1 发布给学员</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test_classify/gdt/sendtest")]
        public HttpResponseMessage SendTest(int id, int is_send)
        {
            return t.Value.SendTest(id,is_send);
        }

        /// <summary>
        /// 删除指定的题库信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test_classify/gdt/del")]
        public HttpResponseMessage Del(int id)
        {
            return t.Value.Del(id);
        }

        /// <summary>
        /// 删除指定的题库信息  多选择删除
        /// </summary>
        /// <param name="ids">格式为1,2,3,4,5</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test_classify/gdt/del")]
        public HttpResponseMessage Del(string ids)
        {
            return t.Value.Del(ids);
        }

        /// <summary>
        /// 查询指定的题库信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test_classify/gdt/query")]
        public HttpResponseMessage Query(int id)
        {
            return t.Value.Query(id);
        }

        /// <summary>
        /// 查询集团内部发布的题库列表
        /// </summary>
        /// <param name="head_id"></param>
        /// <param name="pages"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test_classify/gdt/query")]
        public HttpResponseMessage Query(int head_id, int pages, int count)
        {
            return t.Value.Query(head_id,pages,count);
        }
    }
}
