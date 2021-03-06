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
    
    public partial class studenttime:filelist
    {
        public int sid { get; set; }
        public int fileid { get; set; }
        public int usid { get; set; }
        public int timelenght { get; set; }
        public int lookcount { get; set; }
        public string real_name { get; set; }

        public studenttime SetData(dynamic data)
        {
            fileid = data.fileid != null ? Convert.ToInt32(data.fileid) : 0;
            usid = data.usid != null ? Convert.ToInt32(data.usid) : 0;
            timelenght = data.timelength != null ? Convert.ToInt32(data.timelength) : 0;

            title = data.title != null ? data.title : "";
            img = data.img != null ? data.img : "";
            filepath = data.filepath != null ? data.filepath : "";
            updatetime = data.updatetime != null ? Zh.Tool.Date_Tool.TimeToInt(data.updatetime) : Zh.Tool.Date_Tool.TimeToInt(DateTime.Now);
            types = data.types != null ? data.types : "";
            loaduser = data.types != null ? Convert.ToInt32(data.loaduser) : 0;
            studenttime = data.sdudenttime != null ? Convert.ToInt32(data.sdudenttime) : 30;
            head_id = data.head_id != null ? Convert.ToInt32(data.head_id) : 0;
            filetype = data.filetype != null ? Convert.ToInt32(data.filetype) : 0;
            jibie = data.jibie != null ? Convert.ToInt32(data.jibie):0;
            return this;
        }
    }
}
