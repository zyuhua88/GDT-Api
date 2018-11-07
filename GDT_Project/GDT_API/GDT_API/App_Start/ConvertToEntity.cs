using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;

namespace GDT_API
{
    public class ConvertToEntity<T> where T:new()
    {
        public static List<T> Convert(DataTable dt)
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows) {
                T t = new T();
                PropertyInfo[] p = t.GetType().GetProperties();
                foreach (PropertyInfo pro in p) {
                    if (dt.Columns.Contains(pro.Name)) {
                        object value = dr[pro.Name];
                        if (value != null && value != DBNull.Value)
                        {
                            pro.SetValue(t,value, null);
                        }
                    }
                }
                list.Add(t);
            }

            return list;
        }
    }
}