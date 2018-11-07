using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDT_API.Controllers.GDT.Controller
{
    public class TjController : ApiController
    {
        private Lazy<Dal.Tj> tj = new Lazy<Dal.Tj>();

        /// <summary>
        /// 查询用户每题的最高得分、最低得分、平均得分、在线时长（分钟）,累计参考次数、试题名称
        /// 用户id编号
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Tj/gdt/QueryUserTest")]
        public HttpResponseMessage QueryUserTest(int us_id)
        {
            return tj.Value.QueryUserTest(us_id);
        }

        /// <summary>
        /// 根据用户设定的成绩查询用户的合格率
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Tj/gdt/QueryUserTestAVG")]
        public HttpResponseMessage QueryUserTestAVG(int us_id, int score)
        {
            return tj.Value.QueryUserTestAVG(us_id,score);
        }


        /// <summary>
        /// 查询学习资料的学习情况 结果为文件名称、学习百分比、已学习时长、要求的总学习时长
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Tj/gdt/QueryFile")]
        public HttpResponseMessage QueryFile(int us_id)
        {
            return tj.Value.QueryFile(us_id);
        }


        /// <summary>
        /// 查询我发布的检查信息及我发布的培训计划信息  我发布的
        /// yhtable=检查信息  project=培训计划
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Tj/gdt/TjmySend")]
        public HttpResponseMessage TjmySend(int us_id)
        {
            return tj.Value.TjmySend(us_id);
        }

        /// <summary>
        /// 查询我等待处理信息及我负责的培训计划  等处理处理项
        /// yhtable=检查信息  project=培训计划
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Tj/gdt/waitWork")]
        public HttpResponseMessage waitWork(int us_id)
        {
            return tj.Value.waitWork(us_id);
        }


        /// <summary>
        /// 查询已审核的检查数量
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Tj/gdt/MyVerifyOver")]
        public object MyVerifyOver(int us_id)
        {
            return tj.Value.MyVerifyOver(us_id);
        }

        /// <summary>
        /// 查询整改完成的
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/Tj/gdt/MyZGover")]
        public object MyZGover(int us_id)
        {
            return tj.Value.MyZGover(us_id);
        }
    }
}
