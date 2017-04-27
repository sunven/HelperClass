using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClass
{
    public class FileHelper
    {
        public static string ReadUseFs(string path)
        {
            var byData = new byte[100];
            var file = new FileStream(path, FileMode.Open);
            file.Seek(0, SeekOrigin.Begin);
            var sb = new StringBuilder();
            int length;
            do
            {
                length = file.Read(byData, 0, byData.Length);
                //byData传进来的字节数组,用以接受FileStream对象中的数据,第2个参数是字节数组中开始写入数据的位置,它通常是0,表示从数组的开端文件中向数组写数据,最后一个参数规定从文件读多少字符.
                sb.Append(Encoding.Default.GetString(byData, 0, length));
            } while (length == byData.Length);
            return sb.ToString();
        }

        public static string ReadUseSr(string path)
        {
            var sr = new StreamReader(path, Encoding.Default);
            var sb = new StringBuilder();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                sb.Append(line);
            }
            return sb.ToString();
        }

        public static void WriteUseFs(string path, string txt)
        {
            var fs = new FileStream(path, FileMode.Create);
            //获得字节数组
            var data = Encoding.Default.GetBytes(txt);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }

        public void WriteUseSw(string path, string txt)
        {
            var fs = new FileStream(path, FileMode.Create);
            var sw = new StreamWriter(fs);
            //开始写入
            sw.Write(txt);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
    }
}
