using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Drawing;
using NPOI.SS.UserModel;
using System.Web;
using Spire.Pdf;
using word = Microsoft.Office.Interop.Word;
using System.Net.Http;
using Newtonsoft.Json;

namespace Zh.Tool
{
    public class SqlHelper
    {
        private string connstr;
        public SqlHelper(string _connstr)
        {
            if (connstr == null) {
                connstr = _connstr;
            }
        }

        /// <summary>
        /// 返回查询结果一行一列的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object FirstRow(string sql)
        {
            SqlConnection conn = new SqlConnection(connstr);
            try {
                conn.Open();
                SqlCommand comm = new SqlCommand(sql, conn);
                object obj = comm.ExecuteScalar();
                return obj;
            }
            catch (SqlException ex)
            { 
                return ex.Message;
            }
            finally {
                conn.Close();
            }

        }
        /// <summary>
        /// 返回查询数据一行一列的信息  加入防注入
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public object FirstRow(string sql, SqlParameter[] sp)
        {
            SqlConnection conn = new SqlConnection(connstr);
            try {
                conn.Open();
                SqlCommand comm = new SqlCommand(sql, conn);
                comm.Parameters.AddRange(sp);
                object obj = comm.ExecuteScalar();
                return obj;
            }
            catch (SqlException ex) {
                return ex.Message;
            }
            finally {
                conn.Close();
            }
        }



        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Count(string sql)
        {
            SqlConnection conn = new SqlConnection(connstr);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(sql, conn);
                int obj = comm.ExecuteNonQuery();
                return obj;
            }
            catch (SqlException ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }


        /// <summary>
        /// 返回受影响的行数 防注入并加入事务处理
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public int Count(string sql, SqlParameter[] sp)
        {
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            try {
                SqlCommand comm = new SqlCommand(sql, conn);
                comm.Parameters.AddRange(sp);
                comm.Transaction = tran;
                int obj = comm.ExecuteNonQuery();
                tran.Commit();
                return obj;
            }
            catch (SqlException ex){
                tran.Rollback();
                return 0;
            }
            finally {
                conn.Close();
            }
        }


        /// <summary>
        /// 查询数据，并返回数据表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable Totable(string sql)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connstr);
            try
            {
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(ds);
                dt = ds.Tables[0];
                return dt;
            }
            catch (SqlException ex)
            {
                DataRow row = dt.NewRow();
                dt.Rows.Add(row);
                DataColumn col = new DataColumn();
                dt.Columns.Add(col);
                dt.Rows[0][0] = ex.Message;
                return dt;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 返回dataset
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet ToDataSet(string sql)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connstr);
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(ds);
                return ds;
            }
            catch (SqlException ex)
            {
                return ds;
            }
            finally {
                conn.Close();
                
            }
        }

        /// <summary>
        /// 查询数据表记录  加入防注入
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public DataTable Totable(string sql, SqlParameter[] sp)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connstr);
            try
            {
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.InsertCommand.Parameters.AddRange(sp);
                da.Fill(ds);
                dt = ds.Tables[0];
                return dt;
            }
            catch (SqlException ex)
            {
                DataRow row = dt.NewRow();
                dt.Rows.Add(row);
                DataColumn col = new DataColumn();
                dt.Columns.Add(col);
                dt.Rows[0][0] = ex.Message;
                return dt;
            }
            finally
            {
                conn.Close();
            }
        }

        

        /// <summary>
        /// 返回json数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage ToJson(object obj)
        {
            string s = JsonConvert.SerializeObject(obj);
            HttpResponseMessage m = new HttpResponseMessage();
            m.Content = new StringContent(s,System.Text.Encoding.GetEncoding("UTF-8"),"application/json");
            return m;
        }

    }

    
    public static class Date_Tool
    {
        /// <summary>
        /// 返回时间戳
        /// </summary>
        /// <param name="timeplet"></param>
        /// <returns></returns>
        public static int TimeToInt()
        {
            DateTime stime = new DateTime(1970, 01, 01);
            DateTime etime = DateTime.Now;
            int tp = Convert.ToInt32((etime - stime).TotalSeconds);
            return tp;
        }

        public static int TimeToInt(string date)
        {
            DateTime stime = new DateTime(1970,01,01);
            DateTime etime = DateTime.Parse(date);
            int tp=Convert.ToInt32((etime-stime).TotalSeconds);
            return tp;
        }

        public static int TimeToInt(object date)
        {
            DateTime stime = new DateTime(1970, 01, 01);
            DateTime etime = DateTime.Parse(date.ToString());
            int tp = Convert.ToInt32((etime - stime).TotalSeconds);
            return tp;
        }

        public static DateTime IntToTime(int timeplet)
        {
            DateTime dt = new DateTime(1970,01,01);
            dt = dt.AddSeconds(timeplet);
            
            return dt;
        }
    }


    public static class Excel_Tool
    {
        /// <summary>
        /// 读取excel表到table
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataTable ReadExcel(string path)
        {
            FileStream fs = new FileStream(path,FileMode.OpenOrCreate,FileAccess.ReadWrite);
            DataTable table = new DataTable();

            IWorkbook book = null;
           
            // 2007版本  
            if (path.IndexOf(".xlsx") > 0)
                book = new XSSFWorkbook(fs);
            // 2003版本  
            else if (path.IndexOf(".xls") > 0)
                book = new HSSFWorkbook(fs);

            ISheet sheet= book.GetSheetAt(0);
            for (int i=0;i<sheet.LastRowNum;i++) {
                DataRow dr = table.NewRow();
                table.Rows.Add(dr);
                IRow row= sheet.GetRow(i);
                for (int ii=0;ii<row.LastCellNum;ii++) {
                    DataColumn coll = new DataColumn();
                    table.Columns.Add(coll);
                    ICell cel = row.GetCell(ii);
                    table.Rows[i][ii] = cel;
                }
            }
            return table;
        }

        /// <summary>
        /// 将数据写入excel文件 并返回path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string WriteExcel(string path,DataTable table)
        {
            string p = AppDomain.CurrentDomain.BaseDirectory + path;
            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet= book.CreateSheet("Sheet1");
            for (int i=0;i<table.Rows.Count;i++) {
                IRow row = sheet.CreateRow(i);
                for (int ii=0;ii<table.Columns.Count;ii++) {
                    ICell cell = row.CreateCell(ii);
                    cell.SetCellValue(table.Rows[i][ii].ToString());
                }
            }
            FileStream fs = new FileStream(p,FileMode.OpenOrCreate,FileAccess.ReadWrite);
            book.Write(fs);
            fs.Close();
            book.Close();
            return path;
        }
    }

    public static class Word_Tool
    {
        /// <summary>
        /// 读取word文档内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Read_Word(string path)
        {
            word.Application app = new Microsoft.Office.Interop.Word.Application();
            word.Document doc = null;
            app.Visible = true;
            object unknow = Type.Missing;
            object file = path;
            doc=app.Documents.Open(ref file,ref unknow,
                ref unknow, ref unknow, ref unknow, ref unknow,
                ref unknow, ref unknow, ref unknow, ref unknow,
                ref unknow, ref unknow, ref unknow, ref unknow,
                ref unknow, ref unknow);
            string str= doc.Content.ToString();
            return str;
        }
    }

    public static class File_Tool
    {
        /// <summary>
        /// 大文件上传 返回BOOL类型和提示信息
        /// </summary>
        /// <param name="str">文件流stream</param>
        /// <param name="path">保存文件的绝对路径</param>
        /// <param name="msg">返回的状态信息</param>
        /// <returns></returns>
        public static bool File_Upload(Stream str,string path,out string msg)
        {
            try {
                byte[] buffer = new byte[1024 * 10];
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    while (true)
                    {
                        int len = str.Read(buffer, 0, buffer.Length);
                        fs.Write(buffer, 0, len);
                        if (len != buffer.Length)
                        {
                            break;
                        }
                    }
                }
                msg = "A0000";
                return true;
            }
            catch(Exception ex) {
                msg = ex.Message;
                return false;
            }
        }


        /// <summary>
        /// 图片文件上传 返回BOOL类型 同时返回状态信息msg
        /// </summary>
        /// <param name="file">文件输入流</param>
        /// <param name="path">保存的绝对路径</param>
        /// <param name="msg">返回的状态信息</param>
        /// <returns></returns>
        public static bool Image_Upload(HttpPostedFile file,string path,out string msg)
        {
            try
            {
                file.SaveAs(path);
                msg = "A0000";
                return true;
            }
            catch(Exception ex) {
                msg = ex.Message;
                return false;
            }
        }


        /// <summary>
        /// 图片上传并为图片添加水印 如果原始图片大于800像素，则将图片保存为800像素的宽
        /// </summary>
        /// <param name="str">图片流</param>
        /// <param name="FilePath">指定保存路径</param>
        /// <param name="IconPath">指定水印图片路径</param>
        /// <param name="msg">返回的状态信息</param>
        /// <returns></returns>
        public static bool Image_Shadow(Stream str,string FilePath,string IconPath,out string msg)
        {
            try {
                Image img = Image.FromStream(str);

                int x = 0;
                int y = 0;
                int width = img.Width;
                int height = img.Height;
                if (img.Width > 800)
                {
                    int bs = (img.Width - 800) / img.Width;
                    width = img.Width - img.Width * bs;
                    height = img.Height - img.Height * bs;
                }
                //
                Bitmap bit = new Bitmap(img, width, height);
                Graphics gra = Graphics.FromImage(bit);

                Bitmap icon = new Bitmap(IconPath);
                //设置水印的位置
                if (width > icon.Width + 50)
                {
                    x = width - icon.Width - 50;
                }
                if (height > icon.Height + 50)
                {
                    y = height - icon.Height - 50;
                }
                // 设置生成图片的质量
                gra.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gra.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gra.DrawImage(icon, x, y, icon.Width, icon.Height);
                // 保存位图图像
                bit.Save(FilePath);
                // 释放内存
                icon.Dispose();
                gra.Dispose();
                bit.Dispose();
                img.Dispose();
                msg = "A0000";
                return true;
            }
            catch (Exception ex) {
                msg = ex.Message;
                return false;
            }
            
        }

        /// <summary>
        /// 删除指定的文件
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Del_File(string FilePath,out string msg)
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                msg = "A0000";
            }
            else {
                msg = "A0001";
            }
            return true;
            
        }


        /// <summary>
        /// 删除指定路径的文件
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="directory_name"></param>
        /// <returns></returns>
        public static FileInfo[] FileList(string directory,out string directory_name)
        {
            DirectoryInfo rool = new DirectoryInfo(directory);
            FileInfo[] files = rool.GetFiles();
            directory_name = rool.FullName;
            return files;
        }

        /// <summary>
        /// PDF转成图片文件 同时为图片添加水印
        /// </summary>
        /// <param name="Pdf_path">pdf所在服务器的绝对路径</param>
        /// <param name="icon">水印图片所在服务器的绝对径</param>
        /// <param name="directory_name">将图片保存在程序的哪个文件夹里，格式为“directory_name/”</param>
        /// <returns>返回文件相对路径</returns>
        public static List<string> PdfToImage(string Pdf_path, string icon,string directory_name)
        {
            List<string> list = new List<string>();
            Image con = Image.FromFile(icon);
            PdfDocument doc = new PdfDocument();

            doc.LoadFromFile(Pdf_path);

            string filename = DateTime.Now.ToFileTime().ToString();
            for (int i = 0; i < doc.Pages.Count; i++)
            {
                Image img = doc.SaveAsImage(i);
                int w = img.Width;
                int h = img.Height;


                //添加水印
                Graphics gra = Graphics.FromImage(img);
                Bitmap bit = new Bitmap(con);
                int x = 0; int y = 0;
                if (img.Width > bit.Width + 50)
                {
                    x = img.Width - bit.Width - 50;
                }

                if (img.Height > bit.Height + 50)
                {
                    y = img.Height - bit.Height - 50;
                }
                gra.DrawImage(con, x, y, bit.Width, bit.Height);
                gra.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                gra.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                
                string Spat = AppDomain.CurrentDomain.BaseDirectory + directory_name+"/" + filename + "_" + i + ".png";

                img.Save(Spat);
                list.Add("/"+directory_name+"/" + filename + "_" + i + ".png");
                img.Dispose();
                bit.Dispose();
                gra.Dispose();


            }
            con.Dispose();
            doc.Clone();


            return list;
        }


        /// <summary>
        /// 文件下载  
        /// </summary>
        /// <param name="addressUrl"></param>
        /// <returns></returns>
        public static string DownLoad(string addressUrl)
        {
            try
            {
                //FileName--要下载的文件名 
                FileInfo DownloadFile = new FileInfo(addressUrl);
                if (DownloadFile.Exists)
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Buffer = false;
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFile.FullName, System.Text.Encoding.ASCII));
                    HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                    HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                    return "A0000";
                }
                else
                {
                    return "A0003";
                }
            }
            catch
            {
                return "A0001";
            }
        }

    }

    public static class Json
    {
        public static HttpResponseMessage GetJson(object obj)
        {
            string s = JsonConvert.SerializeObject(obj);
            HttpResponseMessage m = new HttpResponseMessage();
            m.Content = new StringContent(s, System.Text.Encoding.GetEncoding("UTF-8"), "application/json");
            return m;
        }
    }

    
}
