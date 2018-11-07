using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDT_API.Controllers.GDT.Controller
{
    public class Card_SignController : ApiController
    {
        private Lazy<Dal.Card_Sign> sign = new Lazy<Dal.Card_Sign>();

        /// <summary>
        /// 为学员添加三级教育学习卡
        /// </summary>
        /// <param name="end_time"></param>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/card_sign/gdt/Add")]
        public int Add(string end_time, int us_id)
        {
            return sign.Value.Add(end_time,us_id);
        }

        /// <summary>
        /// 删除指定的三级教育卡
        /// </summary>
        /// <param name="card_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/card_sign/gdt/Del")]
        public int Del(int card_id)
        {
            return sign.Value.Del(card_id);
        }

        /// <summary>
        /// 查询员工所有没有过期的三级教育卡
        /// </summary>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/card_sign/gdt/Query")]
        public HttpResponseMessage Query(int us_id)
        {
            return sign.Value.Query(us_id);
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
        /// 
        [HttpPost]
        [Route("api/card_sign/gdt/Query")]
        public HttpResponseMessage Query(int verify, int head_id, int com_id, int b_id, int c_id, int us_id, int status)
        {
            return sign.Value.Query(verify,head_id,com_id,b_id,c_id,us_id,status);
        }

        /// <summary>
        /// 厂级签字
        /// </summary>
        /// <param name="data"></param>
        /// <param name="card_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/card_sign/gdt/Com_sign")]
        public int Com_sign(dynamic data, int card_id)
        {
            return sign.Value.Com_sign(data,card_id);
        }

        /// <summary>
        /// 部门级签字
        /// </summary>
        /// <param name="data"></param>
        /// <param name="card_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/card_sign/gdt/B_sign")]
        public int B_sign(dynamic data, int card_id)
        {
            return sign.Value.B_sign(data,card_id);
        }

        /// <summary>
        /// 班组级签字
        /// </summary>
        /// <param name="data"></param>
        /// <param name="card_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/card_sign/gdt/C_sign")]
        public int C_sign(dynamic data, int card_id)
        {
            return sign.Value.C_sign(data,card_id);
        }
    }
}
