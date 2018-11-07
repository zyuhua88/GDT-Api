using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDT_API.Controllers.GDT.Controller
{
    public class yhTableController : ApiController
    {
        private Lazy<Dal.yhTable> y = new Lazy<Dal.yhTable>();


        /// <summary>
        /// 生成自动编号
        /// </summary>
        /// <param name="y_headid"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/SetNo")]
        public string SetNo(int y_headid)
        {
            return y.Value.SetNo(y_headid);
        }

        /// <summary>
        /// 创建检查表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/CreateYh")]
        public int CreateYh(dynamic data)
        {
            return y.Value.CreateYh(data);
        }

        /// <summary>
        /// 删除指定的数据信息
        /// </summary>
        /// <param name="y_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/Del")]
        public int Del(int y_id)
        {
            return y.Value.Del(y_id);
        }

        /// <summary>
        /// 检查部门负责人签字
        /// </summary>
        /// <param name="y_headuser">检查部门负责人的id编号</param>
        /// <param name="y_headtype">检查部门负责人的意见 0 待批示 1 不同意 2 同意整改</param>
        /// <param name="y_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/HeadSign")]
        public int HeadSign(int y_headuser, int y_headtype, int y_id)
        {
            return y.Value.HeadSign(y_headuser,y_headtype,y_id);
        }

        /// <summary>
        /// 整改后 整改负责人的回复信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="y_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/ZhengGai")]
        public int ZhengGai(dynamic data, int y_id)
        {
            return y.Value.ZhengGai(data,y_id);
        }


        /// <summary>
        /// 整改确认
        /// </summary>
        /// <param name="data">确认说明，整改后图片，检查表单状态，确认人ID，确认时间</param>
        /// <param name="y_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/QueRen")]
        public int QueRen(dynamic data, int y_id)
        {
            return y.Value.QueRen(data,y_id);
        }


        /// <summary>
        /// 修改检查单  只限检查人修改
        /// </summary>
        /// <param name="data"></param>
        /// <param name="y_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/JianChaUpdate")]
        public int JianChaUpdate(dynamic data, int y_id)
        {
            return y.Value.JianChaUpdate(data,y_id);
        }

        /// <summary>
        /// 查询查询单
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="verify"></param>
        /// <param name="head_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/Query")]
        public HttpResponseMessage Query(int us_id, int verify, int head_id, int page, int count)
        {
            return y.Value.Query(us_id, verify, head_id, page, count);
        }

        /// <summary>
        /// 通过隐患表的id查询隐患详细信息
        /// </summary>
        /// <param name="y_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/Query")]
        public HttpResponseMessage Query(int y_id)
        {
            return y.Value.Query(y_id);
        }


        /// <summary>
        /// 数据统计
        /// </summary>
        /// <param name="us_id">用户的id</param>
        /// <param name="verify">用户的权限</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/QueryGroup")]
        public HttpResponseMessage QueryGroup(int us_id, int verify,int head_id)
        {
            return y.Value.QueryGroup(us_id,verify,head_id);
        }


        /// <summary>
        /// 统计用户的检查数量 按年月进行分组，并查询最近12个月的数拓信息
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="verify"></param>
        /// <param name="head_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/QueryUnix")]
        public HttpResponseMessage QueryUnix(int us_id,int verify,int head_id)
        {
            return y.Value.QueryUnix(us_id, verify, head_id);
        }


        /// <summary>
        /// 查询出所有检查出来的隐患分类
        /// </summary>
        /// <param name="head_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/QueryType")]
        public HttpResponseMessage QueryType(int head_id)
        {
            return y.Value.QueryType(head_id);
        }


        /// <summary>
        /// 查询需要负责人签字的表单
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="y_headtype">上级领导签字状态 0 待确认 1 不同意 2 同意</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/QueryHeads")]
        public HttpResponseMessage QueryHeads(int us_id, int y_headtype, int page, int count)
        {
            return y.Value.QueryHeads(us_id,y_headtype,page,count);
        }

        /// <summary>
        /// 查询我要整改或已整改的
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="y_status">0待整改 1 重新整改 2 完成 3已整改待确认</param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/QueryZhengGai")]
        public HttpResponseMessage QueryZhengGai(int us_id, int y_status, int page, int count)
        {
            return y.Value.QueryZhengGai(us_id,y_status,page, count);
        }


        /// <summary>
        /// 查询我要整改或已整改的
        /// </summary>
        /// <param name="us_id"></param>
        /// <param name="y_status">0待整改 1 重新整改 2 完成 3已整改待确认</param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/QueryQueRen")]
        public HttpResponseMessage QueryQueRen(int us_id, int page, int count)
        {
            return y.Value.QueryQueRen(us_id,page,count);
        }


        /// <summary>
        /// 查询近十天的隐患类型
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/todayTj")]
        public HttpResponseMessage todayTj(int verify, int head_id, int com_id, int b_id, int c_id, int us_id)
        {
            return y.Value.todayTj(verify,head_id,com_id,b_id,c_id,us_id);
        }

        /// <summary>
        /// 查询每年十二个月的数据
        /// </summary>
        /// <param name="verify"></param>
        /// <param name="head_id"></param>
        /// <param name="com_id"></param>
        /// <param name="b_id"></param>
        /// <param name="c_id"></param>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/MonthTj")]
        public HttpResponseMessage MonthTj(int verify, int head_id, int com_id, int b_id, int c_id, int us_id, string year)
        {
            return y.Value.MonthTj(verify, head_id, com_id, b_id, c_id, us_id,year);
        }


        /// <summary>
        /// 查询近十天的每天的隐患数据量
        /// </summary>
        /// <param name="verify"></param>
        /// <param name="head_id"></param>
        /// <param name="com_id"></param>
        /// <param name="b_id"></param>
        /// <param name="c_id"></param>
        /// <param name="us_id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/yhtable/gdt/YhCountTen")]
        public HttpResponseMessage YhCountTen(int verify, int head_id, int com_id, int b_id, int c_id, int us_id)
        {
            return y.Value.YhCountTen(verify, head_id, com_id, b_id, c_id, us_id);
        }



    }
}
