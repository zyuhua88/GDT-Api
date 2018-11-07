using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace LoginPayAndShare.App_Start
{
    public class XinXi
    {
        public static string PostUrl = ConfigurationManager.AppSettings["WebReference.Service.PostUrl"];


        public string X(string tel, string body)
        {
            try
            {
                //string account = this.Txtaccount.Text.Trim();
                //string password = this.Txtpassword.Text.Trim();
                //string mobile = this.Txtmobile.Text.Trim();
                //string content = this.Txtcontent.Text.Trim();
                string account = "zhongag01";
                string password = "Zhongangu8";
                string mobile = tel;
                string content = body;

                string postStrTpl = "account={0}&pswd={1}&mobile={2}&msg={3}&needstatus=true&extno=";

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] postData = encoding.GetBytes(string.Format(postStrTpl, account, password, mobile, content));

                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(PostUrl);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = postData.Length;

                Stream newStream = myRequest.GetRequestStream();
                // Send the data.
                newStream.Write(postData, 0, postData.Length);
                newStream.Flush();
                newStream.Close();
                var zhuangtai = "";
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                if (myResponse.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    //LabelRetMsg.Text = reader.ReadToEnd();
                    //反序列化upfileMmsMsg.Text
                    //实现自己的逻辑
                    zhuangtai = reader.ReadToEnd();// "ok";

                }
                else
                {
                    //访问失败
                }
                return zhuangtai;
            }
            catch {
                return "error";
            }
        }
    }
}