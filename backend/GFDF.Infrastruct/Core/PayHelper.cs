using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using GFDF.Infrastruct.Extensions;

namespace GFDF.Infrastruct.Core
{
    public class PayHelper
    {
        #region 生成签名
        /// <summary>
        /// 获取签名数据
        ///</summary>
        /// <param name="strParam"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSignInfo(Dictionary<string, string> strParam, string key)
        {
            //排序
            strParam = strParam.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);
            //连接字段
            string sign = strParam.Aggregate("", (current, d) => current + (d.Key + "=" + d.Value + "&"));
            sign += "key=" + key;
            //MD5
            sign = sign.ToMd5().ToUpper();
            return sign;
        }
        #endregion

        #region XML 处理
        /// <summary>
        /// 获取XML值
        /// </summary>
        /// <param name="strXml">XML字符串</param>
        /// <param name="strData">字段值</param>
        /// <returns></returns>
        public static string GetXmlValue(string strXml, string strData)
        {
            string xmlValue = string.Empty;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(strXml);
            var selectSingleNode = xmlDocument.DocumentElement.SelectSingleNode(strData);
            if (selectSingleNode != null)
            {
                xmlValue = selectSingleNode.InnerText;
            }
            return xmlValue;
        }

        /// <summary>
        /// 集合转换XML数据 (拼接成XML请求数据)
        /// </summary>
        /// <param name="strParam">参数</param>
        /// <returns></returns>
        public static string CreateXmlParam(Dictionary<string, string> strParam)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (KeyValuePair<string, string> k in strParam)
            {
                if (k.Key == "attach" || k.Key == "body" || k.Key == "sign")
                {
                    sb.Append("<" + k.Key + "><![CDATA[" + k.Value + "]]></" + k.Key + ">");
                }
                else
                {
                    sb.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// XML数据转换集合（XML数据拼接成字符串)
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetFromXml(string xmlString)
        {
            Dictionary<string, string> sParams = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlElement root = doc.DocumentElement;
            int len = root.ChildNodes.Count;
            for (int i = 0; i < len; i++)
            {
                string name = root.ChildNodes[i].Name;
                if (!sParams.ContainsKey(name))
                {
                    sParams.Add(name.Trim(), root.ChildNodes[i].InnerText.Trim());
                }
            }
            return sParams;
        }

        /// <summary>
        /// 返回通知 XML
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="returnMsg"></param>
        /// <returns></returns>
        public static string GetReturnXml(string returnCode, string returnMsg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<return_code><![CDATA[" + returnCode + "]]></return_code>");
            sb.Append("<return_msg><![CDATA[" + returnMsg + "]]></return_msg>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetRandomString(int len)
        {
            string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < len; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(allCharArray.Length - 1);
                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }
                temp = t;
                randomCode += allCharArray[t];
            }

            return randomCode;
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        #endregion
    }
}
