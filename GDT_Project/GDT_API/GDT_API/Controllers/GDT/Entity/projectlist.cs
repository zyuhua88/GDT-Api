using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GDT_API.Controllers.GDT.Entity
{
    public class projectlist:train_project
    {
        public new int t_id { get; set; }//对应的培训计划
        public new int create_time { get; set; }//创建时间
        public int work_time { get; set; }//执行时间
        public string project_body { get; set; }//执行的内容
        public new int work_usid { get; set; }//执行者id
        public string l_desc { get; set; }//备注信息
        public int l_us_id { get; set; }//上传者ID
        public string work_length { get; set; }

        public projectlist SetData(dynamic data)
        {
            t_name = data.t_name != null ? data.t_name : "";
            start_time = data.start_time != null ? Zh.Tool.Date_Tool.TimeToInt(data.start_time) : Zh.Tool.Date_Tool.TimeToInt(DateTime.Now);
            end_time = data.end_time != null ? Zh.Tool.Date_Tool.TimeToInt(data.end_time) : Zh.Tool.Date_Tool.TimeToInt(DateTime.Now);
            create_time =data.create_time!=null? Zh.Tool.Date_Tool.TimeToInt(data.create_time):Zh.Tool.Date_Tool.TimeToInt(DateTime.Now);
            score_name = data.score_name != null ? data.score_name : "";
            b_id = data.b_id != null ? data.b_id : "";
            head_id = data.head_id != null ? Convert.ToInt32(data.head_id) : 0;
            com_id = data.com_id != null ? Convert.ToInt32(data.com_id) : 0;
            c_id = data.c_id != null ? Convert.ToInt32(data.c_id) : 0;
            time_length = data.time_length != null ? data.time_length :0;
            score_type = data.score_type != null ? data.score_type : "";
            descs = data.descs != null ? data.descs : "";
            us_id = data.us_id != null ? Convert.ToInt32(data.us_id) : 0;
            work_usid = data.work_usid != null ? Convert.ToInt32(data.work_usid) : 0;
            status = data.status != null ? Convert.ToInt32(data.status) : 0;

            t_id = data.t_id != null ? Convert.ToInt32(data.t_id) : 0;
            work_time = data.work_time != null ? Zh.Tool.Date_Tool.TimeToInt( data.work_time) : 0;
            project_body = data.project_body != null ? data.project_body : "";
            l_desc = data.l_desc != null ? data.l_desc : "";
            l_us_id = data.l_us_id != null ? Convert.ToInt32(data.l_us_id) : 0;
            work_length = data.work_length != null ? data.work_length:"";



            return this;
        }
    }
}