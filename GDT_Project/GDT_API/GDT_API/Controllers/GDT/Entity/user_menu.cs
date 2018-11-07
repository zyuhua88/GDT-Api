using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GDT_API.Controllers.GDT.Entity
{
    public class user_menu:user_detail
    {
        public int id { get; set; }
        public new int us_id { get; set; }
        public string menu { get; set; }


    }
}