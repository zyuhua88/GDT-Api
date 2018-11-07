using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;

namespace GDT_API.Controllers.GDT.Controller
{
    public class TestController : ApiController
    {
        private Lazy<Dal.Test> test = new Lazy<Dal.Test>();

        //客户文件上传 读取excel文件并写入到数据库中
        [HttpPost]
        [Route("api/test/gdt/upload_test")]
        public string Upload_file(int classify_id)
        {
            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            return test.Value.Upload_file(file,classify_id);
        }

        /// <summary>
        /// 查询我的试题库信息
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="t_types">三级教育的类型 0 普通 1 厂级 2 车间  3班组</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test/gdt/Mytest")]
        public HttpResponseMessage Mytest(int us_id,int? t_types)
        {
            return test.Value.Mytest(us_id,t_types);
        }

        /// <summary>
        /// 查询我的试题库信息
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test/gdt/Mytest")]
        public HttpResponseMessage Mytest(int us_id)
        {
            return test.Value.Mytest(us_id);
        }


        /// <summary>
        /// 查询试题的配置信息
        /// </summary>
        /// <param name="testid"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test/gdt/GetTestConfig")]
        public HttpResponseMessage GetTestConfig(int testid)
        {
            return test.Value.GetTestConfig(testid);
        }

        /// <summary>
        /// 查询本套试题的所有试题编号及试题的类型
        /// </summary>
        /// <param name="testid">试题的分类ID</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test/gdt/GetAllTest")]
        public HttpResponseMessage GetAllTest(int testid)
        {
            return test.Value.GetAllTest(testid);
        }

        /// <summary>
        /// 根据试题的ID编号获取详细的试题内容
        /// </summary>
        /// <param name="testid">单个试题题目的id</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test/gdt/GetTest")]
        public HttpResponseMessage GetTest(int testid)
        {
            return test.Value.GetTest(testid);
        }


        /// <summary>
        /// 用户胶卷 并将错题加入到错题库中
        /// </summary>
        /// <param name="testid">试题的分类ID</param>
        /// <param name="score">成绩</param>
        /// <param name="us_id">用户编号</param>
        /// <param name="data">data.testid每题的ID编号</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test/gdt/JiaoJuan")]
        public HttpResponseMessage JiaoJuan(int testid, int score, int us_id, dynamic data)
        {
            return test.Value.JiaoJuan(testid,score,us_id,data);
        }



        /// <summary>
        /// 获取我出错的试题列表
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test/gdt/TestErrorList")]
        public HttpResponseMessage TestErrorList(int us_id)
        {
            return test.Value.TestErrorList(us_id);
        }



        /// <summary>
        /// 查询本题库我的所有错误试题
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="classify_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test/gdt/TestError")]
        public HttpResponseMessage TestError(int us_id, int classify_id)
        {
            return test.Value.TestError(us_id,classify_id);
        }


        /// <summary>
        /// 删除我的错题
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test/gdt/DelError")]
        public HttpResponseMessage DelError(int eid)
        {
            return test.Value.DelError(eid);
        }


        /// <summary>
        /// 统计用户的考试信息
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test/gdt/QueryTestCount")]
        public HttpResponseMessage QueryTestCount(int us_id)
        {
            return test.Value.QueryTestCount(us_id);
        }


        /// <summary>
        /// 统计
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test/gdt/QueryTj")]
        public HttpResponseMessage QueryTj(int us_id)
        {
            return test.Value.QueryTj(us_id);
        }


        /// <summary>
        /// 查询所有企业员工的最高成绩 并按从大到小的顺序排序
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/test/gdt/QueryAllUs_MaxScore")]
        public HttpResponseMessage QueryAllUs_MaxScore(int head_id)
        {
            return test.Value.QueryAllUs_MaxScore(head_id);
        }


    }
}
