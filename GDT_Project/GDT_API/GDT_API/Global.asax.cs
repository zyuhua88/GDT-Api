using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GDT_API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// 跨域设置
        /// </summary>
        protected void Application_BeginRequest()
        {
            //OPTIONS请求方法的主要作用：
            //1、获取服务器支持的HTTP请求方法；也是黑客经常使用的方法。
            //2、用来检查服务器的性能。如：AJAX进行跨域请求时的预检，需要向另外一个域名的资源发送一个HTTP OPTIONS请求头，用以判断实际发送的请求是否安全。
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            {
                //表示对输出的内容进行缓冲，执行page.Response.Flush()时，会等所有内容缓冲完毕，将内容发送到客户端。
                //这样就不会出错，造成页面卡死状态，让用户无限制的等下去
                Response.Flush();
            }
        }
    }
}
