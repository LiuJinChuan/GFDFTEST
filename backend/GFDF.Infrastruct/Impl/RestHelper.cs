using System;
using System.Linq;
using RestSharp;

namespace GFDF.Infrastruct.Impl
{
    public class RestHelper
    {
        private RestClient _client;

        public RestClient Client { get { return _client; } }

        public RestHelper(string baseUri)
        {
            this._client = new RestClient(baseUri);
            //使用CookieContainer自动管理cookie
            this._client.CookieContainer = new System.Net.CookieContainer();
        }

        /// <summary>
        /// 发送一个HTTP请求
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="resource">资源地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="body">请求参数</param>
        /// <param name="setRequest">设置请求参数委托</param>
        /// <returns>返回T对象</returns>
        /// <remarks>当Method为Get的时候，body只能是简单的匿名对象，即匿名对象中不能在包含匿名对象</remarks>
        public T Execute<T>(string resource, Method method, object body = null, Action<IRestRequest> setRequestParam = null)
            where T : new()
        {
            return ExecuteRaw<T>(resource, method, body, setRequestParam).Data;
        }

        /// <summary>
        /// 发送一个HTTP请求
        /// </summary>
        /// <param name="resource">资源地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="body">请求参数</param>
        /// <param name="setRequest">设置请求参数委托</param>
        /// <returns>返回一个包括所有服务器响应信息的原始对象</returns>
        public IRestResponse ExecuteRaw(string resource, Method method, object body = null, Action<IRestRequest> setRequestParam = null)
        {
            var request = BuildRequest(resource, method, body, setRequestParam);
            return _client.Execute(request);
        }

        /// <summary>
        /// 发送一个异步HTTP请求
        /// </summary>
        /// <param name="resource">资源地址</param>
        /// <param name="callback">异步回调函数</param>
        /// <param name="method">请求方式</param>
        /// <param name="body">请求参数</param>
        /// <param name="setRequest">设置请求参数委托</param>
        /// <returns></returns>
        public RestRequestAsyncHandle ExecuteRawAsync(string resource, Action<IRestResponse> callback, Method method, object body = null, Action<IRestRequest> setRequestParam = null)
        {
            var request = BuildRequest(resource, method, body, setRequestParam);
#pragma warning disable CS0618 // “RestClientExtensions.ExecuteAsync(IRestClient, IRestRequest, Action<IRestResponse>)”已过时:“Use ExecuteAsync that returns Task”
            return _client.ExecuteAsync(request, callback);
#pragma warning restore CS0618 // “RestClientExtensions.ExecuteAsync(IRestClient, IRestRequest, Action<IRestResponse>)”已过时:“Use ExecuteAsync that returns Task”
        }

        /// <summary>
        /// 发送一个HTTP请求
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="resource">资源地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="body">请求参数</param>
        /// <param name="setRequest">设置请求参数委托</param>
        /// <returns>返回一个包括所有服务器响应信息的原始对象及反序列化的T对象Data</returns>
        public IRestResponse<T> ExecuteRaw<T>(string resource, Method method, object body = null, Action<IRestRequest> setRequestParam = null)
            where T : new()
        {
            var request = BuildRequest(resource, method, body, setRequestParam);
            return _client.Execute<T>(request);
        }

        /// <summary>
        /// 发送一个异步HTTP请求
        /// </summary>
        /// <param name="resource">资源地址</param>
        /// <param name="callback">回调函数</param>
        /// <param name="method">请求方式</param>
        /// <param name="body">请求参数</param>
        /// <param name="setRequest">设置请求参数委托</param>
        /// <returns></returns>
        public RestRequestAsyncHandle ExecuteRawAsync<T>(string resource, Action<IRestResponse<T>> callback, Method method, object body = null, Action<IRestRequest> setRequestParam = null)
            where T : new()
        {
            var request = BuildRequest(resource, method, body, setRequestParam);
#pragma warning disable CS0618 // “RestClientExtensions.ExecuteAsync<T>(IRestClient, IRestRequest, Action<IRestResponse<T>>)”已过时:“Use ExecuteAsync that returns Task”
            return _client.ExecuteAsync<T>(request, callback);
#pragma warning restore CS0618 // “RestClientExtensions.ExecuteAsync<T>(IRestClient, IRestRequest, Action<IRestResponse<T>>)”已过时:“Use ExecuteAsync that returns Task”
        }

        /// <summary>
        /// 组装请求
        /// </summary>
        /// <param name="resource">请求URL</param>
        /// <param name="method">请求类型</param>
        /// <param name="body">请求体</param>
        /// <param name="setRequest">设置请求参数委托</param>
        /// <returns></returns>
        private RestRequest BuildRequest(string resource, Method method, object body = null, Action<IRestRequest> setRequestParam = null)
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                throw new ArgumentNullException("resource", "请求资源url不能为空。");
            }

            var request = new RestRequest(method)
            {
                Resource = resource,
                RequestFormat = DataFormat.Json,
                //JsonSerializer = new RestJsonSerializer("application/json")
            };
            //默认传递数据格式及响应格式都为json
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            //添加请求体
            if (body != null && new Method[] { Method.PUT, Method.POST }.Contains(method))
            {
                //该方法只能用于POST或PUT请求
                request.AddJsonBody(body);
            }
            else
            {
                //Get或其他请求，body只能是一个简单匿名对象
                request.AddObject(body);
            }

            //执行设置请求参数委托
            if (setRequestParam != null)
            {
                setRequestParam.Invoke(request);
            }
            return request;
        }
    }
}
