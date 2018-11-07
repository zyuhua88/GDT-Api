using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GDT_API.Controllers.GDT.Entity
{
    public class train_project
    {
        public int t_id { get; set; }
        public string t_name { get; set; }
        public int start_time { get; set; }
        public int end_time { get; set; }
        public string score_name { get; set; }
        public string b_id { get; set; }
        public int c_id { get; set; }
        public int com_id { get; set; }
        public int head_id { get; set; }
        public int time_length { get; set; }
        public string score_type { get; set; }
        public string descs { get; set; }
        public int create_time { get; set; }
        public int us_id { get; set; }
        public int work_usid { get; set; }
        public int status { get; set; }//完成状态  0 未完成  1 执行中 2 已完成
        public string real_name { get; set; }//创建人对应us_id
        public string work_name { get; set; }//负责人 对应work_usid
        public string head_name { get; set; }
        public string com_name { get; set; }
        public string b_name { get; set; }
        public string c_name { get; set; }
        public int count { get; set; }

        
    }

    
}