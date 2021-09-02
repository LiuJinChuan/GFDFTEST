
using Newtonsoft.Json;

namespace GFDF.Infrastruct.Core
{
    /// <summary>
    /// 响应结果类
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 状态(1-default, 0-success, -1-error)
        /// </summary>
        public int code
        {
            get;
            set;
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 新ID值
        /// </summary>
        public dynamic NewID
        {
            get;
            set;
        }

        /// <summary>
        /// 消息额外数据
        /// </summary>
        public object data
        {
            get;
            set;
        }

        /// <summary>
        /// 总数
        /// </summary>
        public int count
        {
            get;
            set;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="code">状态:0-成功；1-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult Default(string message = null)
        {
            return Return(1, message);
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="Status">状态:0-成功；1-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult Success(string message = null, int count = 0)
        {
            return Return(0, message, null, null, count);
        }

        public static ResponseResult Success(dynamic data)
        {
            return Return(0, null, null, data);
        }

        /// <summary>
        /// 响应消息封装类，用于插入操作，返回新ID
        /// </summary>
        /// <param name="code">状态:0-成功; 1-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult Success(dynamic newId, string message = null, int count = 0)
        {
            return Return(0, message, newId, null, count);
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="code">状态:0-成功; 1-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult Return(int code = 0, string message = null, dynamic newId = null, dynamic data = null, int count = 0)
        {
            var result = new ResponseResult();
            result.code = code;
            result.NewID = newId;
            result.Message = message;
            result.data = data;
            result.count = count;
            return result;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="code">状态:0-成功；1-缺省; -1失败; 3未授权</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult Error(string errorMessage, int code = -1, int count = 0) => Return(code, errorMessage, null, null, count);

        /// <summary>
        /// 解析返回结果中的状态标志位
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Parse(string message)
        {
            var result = JsonConvert.DeserializeObject<ResponseResult>(message);
            var success = result.code == 0;

            return success;
        }

        public static ResponseResult Deserialize(string message)
        {
            var result = JsonConvert.DeserializeObject<ResponseResult>(message);
            return result;
        }
    }

    /// <summary>
    /// 响应消息类(泛型)
    /// </summary>
    public class ResponseResult<T> where T : class
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int code
        {
            get;
            set;
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 新GUID值
        /// </summary>
        public dynamic NewID
        {
            get;
            set;
        }

        /// <summary>
        /// 业务实体
        /// </summary>
        public T data
        {
            get;
            set;
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get;
            set;
        }

        /// <summary>
        /// 总行数
        /// </summary>
        public int TotalRowsCount
        {
            get;
            set;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="code">状态:0-成功; 1-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult<T> Default()
        {
            var result = new ResponseResult<T>();
            result.data = null;
            result.code = 1;
            result.Message = string.Empty;

            return result;
        }

        /// <summary>
        /// 响应消息封装类
        /// </summary>
        /// <param name="Status">状态:0-成功; 1-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult<T> Success(T t, string message = null)
        {
            var result = new ResponseResult<T>();
            result.data = t;
            result.code = 0;
            result.Message = message;

            return result;
        }

        /// <summary>
        /// 响应消息封装类，用于插入操作，返回新ID
        /// </summary>
        /// <param name="code">状态:0-成功; 1-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult<T> Success(int newId, string message = null)
        {
            var result = new ResponseResult<T>();
            result.NewID = newId;
            result.code = 0;
            result.Message = message;
            return result;
        }
        /// <summary>
        /// Http 响应消息封装类
        /// </summary>
        /// <param name="Status">状态:0-成功; 1-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult<T> Error(string message = null)
        {
            var result = new ResponseResult<T>();
            result.data = null;
            result.code = -1;
            result.Message = message;

            return result;
        }

        /// <summary>
        /// 响应消息封装类，用于插入操作，返回新ID
        /// </summary>
        /// <param name="code">状态:0-成功; 1-缺省; -1失败</param>
        /// <param name="Message">消息内容</param>
        /// <returns></returns>
        public static ResponseResult<T> Return(int code = 0, string message = null, dynamic newId = null, T data = null)
        {
            var result = new ResponseResult<T>();
            result.code = code;
            result.NewID = newId;
            result.Message = message;
            result.data = data;

            return result;
        }

        /// <summary>
        /// Http 响应消息反序列化类
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static ResponseResult<T> Deserialize(string message)
        {
            var response = JsonConvert.DeserializeObject<ResponseResult<T>>(message);
            return response;
        }
    }
}
