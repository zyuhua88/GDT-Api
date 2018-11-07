//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace GDT_API.Controllers.GDT.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class card_sign
    {
        public int card_id { get; set; }
        public int end_time { get; set; }
        public string com_sign { get; set; }
        public int com_time { get; set; }
        public string b_sign { get; set; }
        public int b_time { get; set; }
        public string c_sign { get; set; }
        public int c_time { get; set; }
        public int us_id { get; set; }
        public string real_name { get; set; }

        public card_sign SetData(dynamic data)
        {
            end_time = data.end_time != null ? Zh.Tool.Date_Tool.TimeToInt(data.end_time) : 0;
            com_sign = data.com_sign != null ? data.com_sign : "";
            com_time = data.com_time != null ? Zh.Tool.Date_Tool.TimeToInt(data.com_time) : 0;
            b_sign = data.b_sign != null ? data.b_sign : "";
            b_time = data.b_time != null ? Zh.Tool.Date_Tool.TimeToInt(data.b_time) : 0;
            c_sign = data.c_sign != null ? data.c_sign : "";
            c_time = data.c_time != null ? Zh.Tool.Date_Tool.TimeToInt(data.c_time) : 0;
            us_id = data.us_id != null ? Convert.ToInt32(data.us_id) : 0;

            return this;
        }
    }
}
