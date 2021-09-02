using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFDF.Infrastruct.Core
{
    public class Utils
    {

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string GenRandom(int Length, bool Sleep = false)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }


        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        public static string GetCheckCode(int codeCount)
        {
            string str = string.Empty;
            int rep = 0;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }


        /// <summary>
        /// 将时间戳转换为日期类型，并格式化
        /// </summary>
        /// <param name="longDateTime"></param>
        /// <returns></returns>
        public static string LongDateTimeToDateTimeString(long longDateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            //用来格式化long类型时间的,声明的变量
            DateTime start;
            DateTime date;
            //ENd

            start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            date = start.AddMilliseconds(longDateTime).ToLocalTime();
            return date.ToString(format);
        }

        /// <summary>
        /// 将数据Csv
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="filePath">csv路径</param>
        /// <param name="dic">输出标题</param>
        public static bool TableToCsv(List<Dictionary<string, object>> dt, string filePath, Dictionary<string, string> dic)
        {
            FileInfo fi = new FileInfo(filePath);
            string path = fi.DirectoryName;
            string name = fi.Name;
            //\/:*?"|
            //把文件名和路径分别取出来处理
            name = name.Replace(@"\", "＼");
            name = name.Replace(@"/", "／");
            name = name.Replace(@":", "：");
            name = name.Replace(@"*", "＊");
            name = name.Replace(@"?", "？");
            name = name.Replace(@"|", "｜");
            try
            {
                using (FileStream fs = new FileStream(path + "\\" + name, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.Default))
                    {
                        string title = "序号,";
                        foreach (var item in dic)
                        {
                            if (!dt[0].ContainsKey(item.Key.ToLower())) continue;
                            title += item.Value + ",";
                        }
                        title = title.Substring(0, title.Length - 1) + "\n";
                        sw.Write(title);
                        int num = 0;
                        foreach (dynamic row in dt)
                        {
                            num++;
                            string line = num + ",";
                            foreach (var item in dic)
                            {
                                if (!row.ContainsKey(item.Key.ToLower())) continue;
                                string value;
                                switch (row[item.Key.ToLower()].ToString().Replace(",", ""))
                                {
                                    case "false":
                                    case "False":
                                        value = "否";
                                        break;
                                    case "true":
                                    case "True":
                                        value = "是";
                                        break;
                                    default:
                                        value = row[item.Key.ToLower()].ToString().Replace(",", "");
                                        break;
                                }
                                line += "\t" + value + ",";
                            }
                            line = line.Substring(0, line.Length - 1) + "\n";
                            sw.Write(line);
                        }
                        sw.Close();
                        fs.Close();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                new GFDF.Infrastruct.Impl.LogHelper().Error(ex);
                if (File.Exists(filePath)) File.Delete(filePath);
                return false;
            }
        }


        /// <summary>
        /// 将Csv读入DataTable（方法二）
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable OpenCsv(string fileName)
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool isFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split(',');
                if (isFirst == true)
                {
                    isFirst = false;
                    columnCount = aryLine.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(aryLine[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j].Replace("'", "").Replace("\"", "").Replace("\t", "");
                        if (dr[j].ToString().ToLower().Trim() == "true") dr[j] = "1";
                        if (dr[j].ToString().ToLower().Trim() == "false") dr[j] = "0";
                    }
                    dt.Rows.Add(dr);
                }
            }

            sr.Close();
            fs.Close();
            return dt;
        }

        /// <summary> 
        /// 数据表转键值对集合
        /// 把DataTable转成 List集合, 存每一行 
        /// 集合中放的是键值对字典,存每一列 
        /// </summary> 
        /// <param name="dt">数据表</param>
        /// <returns>哈希表数组</returns>
        public static List<Dictionary<object, object>> DataTableToListObj(DataTable dt)
        {
            List<Dictionary<object, object>> list = new List<Dictionary<object, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<object, object> dic = new Dictionary<object, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.DataType == typeof(DateTime))
                    {
                        dic.Add(dc.ColumnName, Convert.IsDBNull(dr[dc.ColumnName]) ? "" : Convert.ToDateTime(dr[dc.ColumnName]).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                    }
                }
                list.Add(dic);
            }
            return list;
        }
    }
}
