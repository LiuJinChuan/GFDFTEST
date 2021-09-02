using GFDF.Infrastruct.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.WebSockets;

namespace GFDP.Sys.Service
{
    public class WSHelper
    {
        /// <summary>
        /// 保存客户端的WebSocket对象
        /// </summary>
        public static readonly Dictionary<string, WebSocket> sWebs = new Dictionary<string, WebSocket>();

        #region 构建线程安全的单例模式
        private static WSHelper _instance;
        public WSHelper() { }
        public static WSHelper GetInstance()
        {
            if (_instance == null)
            {
                lock (sWebs)
                {
                    if (_instance == null)
                    {
                        _instance = new WSHelper();
                    }
                }
            }
            return _instance;
        }
        #endregion

        /// <summary>
        /// 和客户端建立WebSocket连接
        /// </summary>
        /// <param name="arg">客户端发送的WebSocket相关信息</param>
        /// <returns></returns>
        public async Task ProcessWSChat(AspNetWebSocketContext arg)
        {
            // 1.获取自定义的参数
            string tcAdminId = arg.QueryString["tcAdminId"];
            if (string.IsNullOrEmpty(tcAdminId) || tcAdminId == "undefined")
            {
                arg.WebSocket.Abort();
                return;
            }
            // 2.将用户编号作为标识客户端唯一性的Key，保存客户端的WebSocket对象
            if (!sWebs.ContainsKey(tcAdminId)) sWebs.Add(tcAdminId, arg.WebSocket);
            else
            {
                sWebs[tcAdminId].Abort();
                sWebs[tcAdminId] = arg.WebSocket;
            }
            try
            {
                while (true)
                {
                    ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024 * 10]);
                    WebSocketReceiveResult result = await sWebs[tcAdminId].ReceiveAsync(buffer, CancellationToken.None);
                    if (sWebs[tcAdminId].State != WebSocketState.Open)
                    {
                        sWebs.Remove(tcAdminId);
                        EventBus.Emit("WebSocket.Close", tcAdminId);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (sWebs.ContainsKey(tcAdminId)) sWebs.Remove(tcAdminId);
                EventBus.Emit("WebSocket.Close", tcAdminId);
                GFContext.logger.Error(ex);
            }
        }

        /// <summary>
        /// 服务端向客户端推送消息
        /// </summary>
        public async Task<bool> SendMsgAsync(string message, string tcAdminId)
        {
            if (!sWebs.ContainsKey(tcAdminId)) return false;
            //【重要】执行下面socket.State代码可能会抛异常"无法访问已经释放的对象"，
            // 因为客户端已经处于断电、断网、强制关闭、刷新等状态，当前的WebSocket对象已经失去价值，直接删除即可
            try
            {
                if (sWebs[tcAdminId].State != WebSocketState.Open) return false;
                ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                await sWebs[tcAdminId].SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                sWebs.Remove(tcAdminId);
                EventBus.Emit("WebSocket.Close", tcAdminId);
                GFContext.logger.Error(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取连接者
        /// </summary>
        public virtual dynamic GetLink() => sWebs.Select(o => new { userid = o.Key, state = o.Value?.State });
    }
}
