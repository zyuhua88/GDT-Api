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
    
    public partial class test:test_classify
    {
        public new int id { get; set; }
        public int classify_id { get; set; }
        public string t_type { get; set; }
        public string t_desc { get; set; }
        public string t_title { get; set; }
        public string t_a { get; set; }
        public string t_b { get; set; }
        public string t_c { get; set; }
        public string t_d { get; set; }
        public string t_e { get; set; }
        public string t_f { get; set; }
        public string daan { get; set; }
        public int tcount { get; set; }

        public test SetData(dynamic data)
        {
            DateTime ct = DateTime.Now;
            this.t_name = data.t_name != null ? data.t_name : "";
            this.create_time = Zh.Tool.Date_Tool.TimeToInt(ct);
            this.us_id = data.us_id != null ? Convert.ToInt32(data.us_id) : 0;
            this.head_id = data.head_id != null ? Convert.ToInt32(data.head_id) : 0;
            this.type_a = data.type_a != null ? Convert.ToInt32(data.type_a) : 0;
            this.type_b = data.type_b != null ? Convert.ToInt32(data.type_b) : 0;
            this.type_c = data.type_c != null ? Convert.ToInt32(data.type_c) : 0;
            this.type_d = data.type_d != null ? Convert.ToInt32(data.type_d) : 0;
            this.is_send = data.is_send != null ? Convert.ToInt32(data.is_send) : 0;
            this.defen_a = data.defen_a != null ? Convert.ToInt32(data.defen_a) : "1";
            this.defen_b = data.defen_b != null ? Convert.ToInt32(data.defen_b) : "1";
            this.defen_c = data.defen_c != null ? Convert.ToInt32(data.defen_c) : "2";
            this.defen_d = data.defen_d != null ? Convert.ToInt32(data.defen_d) : "2";


            this.classify_id = data.classify_id != null ? Convert.ToInt32(data.classify_id) : 0;
            this.t_type = data.t_type != null ? data.t_type : "";
            this.t_types = data.t_types != null ?Convert.ToInt32(data.t_types) : 0;
            this.t_desc = data.t_desc != null ? data.t_desc : "";
            this.t_title = data.t_title != null ? data.t_title : "";
            this.t_a = data.t_a != null ? data.t_a : "";
            this.t_b = data.t_b != null ? data.t_b : "";
            this.t_c = data.t_c != null ? data.t_c : "";
            this.t_d = data.t_d != null ? data.t_d : "";
            this.t_e = data.t_e != null ? data.t_e : "";
            this.t_f = data.t_f != null ? data.t_f : "";
            this.daan = data.daan != null ? data.daan : "";
            

            return this;
        }
    }
}
