using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
namespace GFDP.Sys.Service
{
    /// <summary>
    /// Opcode操作码是一个在 0 到 15（包括）之间的整数数字。
    /// </summary>
    public enum OpcodeType : byte
    {
        Contin = 0x0,   //表示连续消息片断
        Text = 0x1,     //表示文本消息片断
        Binary = 0x2,   //表未二进制消息片断
        // 0x3 - 0x7 非控制帧保留
        Close = 0x8,    //表示连接关闭
        Ping = 0x9,     //表示心跳检查的ping
        Pong = 0xA,     //表示心跳检查的pong
        // 0xB - 0xF 控制帧保留
        Unkown
    };
    /// <summary>
    /// 数据帧头,就是包头结构
    /// </summary>
    public struct DataFrame
    {
        /// <summary>0表示不是当前消息的最后一帧，后面还有消息,1表示这是当前消息的最后一帧；</summary>
        public bool FIN;
        /// <summary>1位，若没有自定义协议,必须为0,否则必须断开.</summary>
        public bool RSV1;
        /// <summary>1位，若没有自定义协议,必须为0,否则必须断开.</summary>
        public bool RSV2;
        /// <summary>1位，若没有自定义协议,必须为0,否则必须断开.</summary>
        public bool RSV3;
        /// <summary>4位操作码，定义有效负载数据，如果收到了一个未知的操作码，连接必须断开.</summary>
        public OpcodeType Opcode;
        /// <summary>1位，定义传输的数据是否有加掩码,如果有掩码则存放在MaskingKey</summary>
        public bool MASK;
        /// <summary>0或4个字节，客户端发送给服务端的数据，都是通过内嵌的一个32位值作为掩码的；掩码键只有在掩码位设置为1的时候存在。</summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] MaskingKey;
        /// <summary>传输数据的长度</summary>
        public int Payloadlen;
        /// <summary>数据起始位</summary>
        public int DataOffset;
    }

    public class WSProtocol
    {
        #region 握手
        /// <summary>
        /// 获取连接请求附带的参数
        /// </summary>
        public static NameValueCollection QueryString(byte[] recv)
        {
            //前端js如:  ws = new WebSocket("ws://127.0.0.1:8899/ws?id=1&session=a1b2c3")
            //该函数相当于ASP.NET中的Request.QueryString,就是取得参数 id 和 session 的
            NameValueCollection NV = new NameValueCollection();
            string n = string.Empty;
            string v = string.Empty;
            bool tf1 = false;
            bool tf2 = false;
            foreach (byte b in recv)
            {
                if (tf1)
                {
                    if (b == 32)
                        break;
                    else if (b == 61 && tf2 == false)//=
                        tf2 = true;
                    else if (b == 38)//&
                    {
                        tf2 = false;
                        if (!string.IsNullOrEmpty(n) && !string.IsNullOrEmpty(v))
                            NV.Add(n, v);
                        n = string.Empty;
                        v = string.Empty;
                    }
                    else
                    {
                        if (tf2)
                            v += (char)b;
                        else
                            n += (char)b;
                    }
                }
                else if (b == 63)//?
                    tf1 = true;
                else if (b == 10 || b == 13) break;
            }
            if (!string.IsNullOrEmpty(n) && !string.IsNullOrEmpty(v))
                NV.Add(n, v);
            return NV;
        }
        public static byte[] Handshake(string request)
        {
            string webSocketKey = getCilentWSKey(request, "Sec-WebSocket-Key:");
            string acceptKey = produceAcceptKey(webSocketKey);
            StringBuilder response = new StringBuilder(); //响应串
            response.Append("HTTP/1.1 101 Web Socket Protocol Handshake\r\n");
            response.Append("Upgrade: WebSocket\r\n");
            response.Append("Connection: Upgrade\r\n");
            response.AppendFormat("Sec-WebSocket-Accept: {0}\r\n", acceptKey);
            response.AppendFormat("WebSocket-Origin: {0}\r\n", getCilentWSKey(request, "Sec-WebSocket-Origin"));
            response.AppendFormat("WebSocket-Location: {0}\r\n", getCilentWSKey(request, "Host"));
            response.Append("\r\n");
            return Encoding.UTF8.GetBytes(response.ToString());
        }
        private static string getCilentWSKey(string request, string kname)
        {
            int i = CultureInfo.InvariantCulture.CompareInfo.IndexOf(request, kname, CompareOptions.IgnoreCase);
            if (i > 0)
            {
                i += kname.Length;
                int j = request.IndexOf("\r\n", i);
                if (j > 0)
                    return request.Substring(i, j - i).Trim();
            }
            return string.Empty;
        }
        // 根据Sec-WebSocket-Key和MagicKey生成AcceptKey
        private static string produceAcceptKey(string webSocketKey)
        {
            string MagicKey = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            Byte[] acceptKey = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(webSocketKey + MagicKey));
            return Convert.ToBase64String(acceptKey);
        }
        #endregion
        #region 数据帧解析
        /// <summary>
        /// 数据帧头的解析
        /// </summary>
        public static DataFrame AnalyzeHeader(byte[] data)
        {
            DataFrame df;
            df.FIN = (data[0] & 0x80) == 0x80 ? true : false;
            df.RSV1 = (data[0] & 0x40) == 0x40 ? true : false;
            df.RSV2 = (data[0] & 0x40) == 0x20 ? true : false;
            df.RSV3 = (data[0] & 0x40) == 0x10 ? true : false;
            byte[] b = { data[0] };
            BitArray bit = new BitArray(b);
            bit.Set(4, false);
            bit.Set(5, false);
            bit.Set(6, false);
            bit.Set(7, false);
            bit.CopyTo(b, 0);
            df.Opcode = (OpcodeType)b[0];

            df.MASK = (data[1] & 0x80) == 0x80 ? true : false;
            df.MaskingKey = new Byte[4];
            int len = data[1] & 0x7F;
            /// 0-125 表示传输数据的长度；
            /// 126   表示随后的两个字节是一个16进制无符号数，用来表示传输数据的长度；
            /// 127   表示随后的是8个字节是一个64位无符合数，这个数用来表示传输数据的长度。
            /// 多字节长度的数量是以网络字节的顺序表示。负载数据的长度为扩展数据及应用数据之和，扩展数据的长度可能为0,因而此时负载数据的长度就为应用数据的长度。
            switch (len)
            {
                case 126:
                    df.Payloadlen = (UInt16)(data[2] << 8 | data[3]);
                    if (df.MASK)
                    {
                        Buffer.BlockCopy(data, 4, df.MaskingKey, 0, 4);
                        df.DataOffset = 8;
                    }
                    else
                        df.DataOffset = 4;
                    break;
                case 127:
                    Byte[] byteLen = new Byte[8];
                    Buffer.BlockCopy(data, 4, byteLen, 0, 8);
                    df.Payloadlen = (int)BitConverter.ToUInt64(byteLen, 0);
                    if (df.MASK)
                    {
                        Buffer.BlockCopy(data, 10, df.MaskingKey, 0, 4);
                        df.DataOffset = 14;
                    }
                    else
                        df.DataOffset = 10;
                    break;
                default:
                    if (len < 126)
                    {
                        df.Payloadlen = len;
                        if (df.MASK)
                        {
                            Buffer.BlockCopy(data, 2, df.MaskingKey, 0, 4);
                            df.DataOffset = 6;
                        }
                        else
                            df.DataOffset = 2;
                    }
                    else
                    {
                        df.Payloadlen = 0;
                        df.DataOffset = 0;
                    }
                    break;
            }
            return df;
        }
        #endregion
        #region 处理数据--接收
        /*
         * PickDataV  方法是出于性能的考虑,用于有时数据只是为了接收,做一些逻辑判断,并不需要对数据块进行单独提炼
         * PickData   有两个重载就不赘述了...
        */
        /// <summary>
        /// 如果数据存在掩码就反掩码,具体的使用数据就
        /// </summary>
        public static void PickDataV(byte[] data, DataFrame dtype)
        {
            if (dtype.MASK)
            {
                int j = 0;
                for (int i = dtype.DataOffset; i < dtype.DataOffset + dtype.Payloadlen; i++)
                {
                    data[i] ^= dtype.MaskingKey[j++];
                    if (j == 4)
                        j = 0;
                }
            }
        }
        public static byte[] PickData(byte[] data, DataFrame dtype)
        {
            byte[] byt = new byte[dtype.Payloadlen];
            PickDataV(data, dtype);
            Buffer.BlockCopy(data, dtype.DataOffset, byt, 0, dtype.Payloadlen);
            return byt;
        }
        public static string PickData(byte[] data, DataFrame dtype, Encoding encode)
        {
            PickDataV(data, dtype);
            return encode.GetString(data, dtype.DataOffset, dtype.Payloadlen);
        }
        #endregion
        #region 处理数据--发送
        /*
         * PackData         两个重载,用于组装无掩码数据
         * PackMaskData     两个重载,用于将数据掩码后组装
        */
        /// <summary>
        /// 组装无掩码数据,一般用于服务端向客户端发送
        /// </summary>
        public static byte[] PackData(string data, Encoding encode, OpcodeType dwOpcode = OpcodeType.Text)
        {
            //字符默认用UTF8编码处理,如果有别的编码需求,自行处理后用下面的重载函数
            byte[] buff = encode.GetBytes(data);
            return PackData(buff, dwOpcode);
        }
        public static byte[] PackData(byte[] buff, OpcodeType dwOpcode = OpcodeType.Text)
        {
            List<byte> byt = new List<byte>();
            byt.Add((byte)(0x80 | (byte)dwOpcode));
            if (buff.Length < 126)
                byt.Add((byte)buff.Length);
            else if (buff.Length <= ushort.MaxValue)
            {
                ushort l = (ushort)buff.Length;
                byte[] bl = BitConverter.GetBytes(l);
                byt.Add(0x7e);
                byt.Add(bl[1]);
                byt.Add(bl[0]);
            }
            else
            {
                //由于用不到,未做测试
                ulong l = (ulong)buff.Length;
                byt.Add(0x7f);
                byt.AddRange(BitConverter.GetBytes(l));
            }
            byt.AddRange(buff);
            return byt.ToArray();
        }

        /// <summary>
        /// 将数据掩码后组装,一般是客户端向服务端发送
        /// </summary>
        public static byte[] PackMaskData(string str, Encoding encode, OpcodeType dwOpcode = OpcodeType.Text)
        {
            byte[] byt = encode.GetBytes(str);
            return PackMaskData(byt, dwOpcode);
        }
        public static byte[] PackMaskData(byte[] byt, OpcodeType dwOpcode = OpcodeType.Text)
        {
            List<byte> data = new List<byte>();
            //掩码我用的是固定值,有需要也可以自己做成随机的
            byte[] maskey = { 13, 113, 213, 177 };
            int j = 0;
            for (int i = 0; i < byt.Length; i++)
            {
                data[i] ^= maskey[j++];
                if (j > 3) j = 0;
            }
            data.Add((byte)(0x80 | (byte)OpcodeType.Text));//第一字节,FIN+RSV1+RSV2+RSV3+opcode
            if (byt.Length < 126)
            {
                data.Add((Byte)(0x80 | (Byte)byt.Length));
            }
            else if (byt.Length <= ushort.MaxValue)//65535
            {
                data.Add(254);//固定 掩码位+126
                byte[] b = BitConverter.GetBytes((ushort)byt.Length);
                data.Add(b[1]);//反转
                data.Add(b[0]);
            }
            else
            {
                //这部分没有经过实际测试,依靠协议文档推写出来的
                //我的需求只是聊天通信,若有传送文件等需求,请自行测试
                data.Add(255);//固定 掩码位+127
                byte[] b = BitConverter.GetBytes((ulong)byt.Length);
                Array.Reverse(b);//反转
                data.AddRange(b);
            }
            data.AddRange(byt);
            data.AddRange(maskey);
            return data.ToArray();
        }
        #endregion
        #region 常用控制帧
        /*
            下面的 Ping、Pong、Close 是非掩码信号,用于服务端向客户端发送,如果客户端想服务端发送就需要掩码
            使用的时候直接  socket.Send(WSProtocol.PingFrame, 0, WSProtocol.PingFrame.Length);
            我用的0长度,其实是可以包含数据的,但是附带数据客户端处理又麻烦了

            * 如果有附带信息的需求,也可以用上面[发送]里的函数,可选参数指定OpcodeType
            * 特别注意:收到ping的时候,回应pong.而收到pong的时候,回应还是pong
            * 在协议里,ping是最为要求应答信号的,而pong是作为单向心跳检测的
         */
        private static byte[] dwPing = { 0x89, 0x0 };
        private static byte[] dwPong = { 0x8a, 0x0 };
        private static byte[] dwClose = { 0x88, 0x0 };
        public static byte[] PingFrame { get { return dwPing; } }
        public static byte[] PongFrame { get { return dwPong; } }
        public static byte[] CloseFrame { get { return dwClose; } }
        #endregion
    }
}