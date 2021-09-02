using System;
using System.Collections.Generic;
using System.Text;

namespace GFDF.Infrastruct.Core
{
    [System.Serializable]
    public class BaseException : Exception
    {
        public ExEnum Code;
        public dynamic BussData;
        private string _exMsg;
        public string exMsg
        {
            get { return string.IsNullOrEmpty(_exMsg) ? base.Message : _exMsg; }
            set { }
        }
        public BaseException() { }
        public BaseException(string message) : base(message) { }
        public BaseException(string message, Exception inner) : base(message, inner) { }
        protected BaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        public BaseException(ExEnum code, string message = "", dynamic bussData = null) : base(message)
        {
            Code = code;
            _exMsg = string.IsNullOrEmpty(message) ? getMsg(code) : message;
            BussData = bussData;
        }
        public void Handle()
        {
            //LogHelper.WriteLog(this.GetType(), $"{exMsg}{Message}:-{Source}", LogHelper.LogEnum.ERROR);
        }

        public string getMsg(ExEnum code)
        {
            string _exMsg = "";
            switch (code)
            {
                case ExEnum.ArgNullEx:
                    _exMsg = "参数不能为空";
                    break;
                case ExEnum.ArgFormatEx:
                    _exMsg = "参数格式不正确";
                    break;
                case ExEnum.ArgRangeEx:
                    _exMsg = "参数不是有效值";
                    break;
                case ExEnum.AccessDeniedEx:
                    _exMsg = "访问被拒绝";
                    break;
                case ExEnum.ResultNullEx:
                    _exMsg = "获取结果数据为空";
                    break;
                case ExEnum.ReturnNullEx:
                    _exMsg = "路径无返回结果";
                    break;
                case ExEnum.ExistEx:
                    _exMsg = "Already existed";
                    break;
                case ExEnum.NotExistEx:
                    _exMsg = "Not found";
                    break;
                default:
                    _exMsg = "未知异常";
                    break;
            }
            return _exMsg;
        }
    }

    public enum ExEnum
    {
        ArgNullEx,
        ArgFormatEx,
        AccessDeniedEx,
        ArgRangeEx,
        ResultNullEx,
        ReturnNullEx,
        ExistEx,
        NotExistEx,
        Other = 99
    }
}
