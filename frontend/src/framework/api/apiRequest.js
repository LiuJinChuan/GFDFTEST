/**
 * 全站http配置
 *
 * axios参数说明
 * isSerialize是否开启form表单提交
 * isToken是否需要token
 */
import axios from 'axios'
import router from '@/framework/router/router'//动态router
import { Message } from 'element-ui'
import website from '@/framework/config/website';
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
axios.defaults.timeout = 10000;
//返回其他状态吗
axios.defaults.validateStatus = function (status) {
  return status >= 200 && status <= 500; // 默认的
};
//跨域请求，允许保存cookie
axios.defaults.withCredentials = true;
// NProgress Configuration
NProgress.configure({
  showSpinner: false
});
//HTTPrequest拦截
axios.interceptors.request.use(config => {
  NProgress.start() // start progress bar
  config.headers['Authorization'] = window.GF.context.get('token') || "" // 让每个请求携带token--['Authorization']为自定义key 请根据实际情况自行修改
  return config
}, error => {
  return Promise.reject(error)
});
var flag = true
//HTTPresponse拦截
axios.interceptors.response.use(res => {
  NProgress.done();
  const status = Number(res.status) || 200;
  const statusWhiteList = website.statusWhiteList || [];
  const message = res.data.Message || '未知错误';
  //如果在白名单里则自行catch逻辑处理

  if (statusWhiteList.includes(status)) return Promise.reject(res);
  //如果是401则跳转到登录页面
  //TODO:/login中进行loginout消息，以预先清除原用户信息.
  if (status === 401) router.push({ path: '/login' });
  // 如果请求为非200否者默认统一处理
  if (status !== 200) {
    Message({
      message: message,
      type: 'error'
    })
    return Promise.reject(new Error(message))
  }
  else {
    if (res.data.code == -1) {
      Message({
        message: message,
        type: 'error',
        duration: 1000
      });
      return Promise.reject(res.data)
    }
    if (res.data.code == 0) {
      return res.data;
    }
    if (res.data.code == 1 && flag) {
      //falg 解决页面多个请求 多个提示语句
      flag = false
      Message({
        message: res.data.Message,
        type: 'error',
        duration: 1000,
        onClose() {
          flag = true
          window.GF.emitter.emit('gf-logout')
          //store.dispatch('FedLogOut').then(() => router.push({ path: '/login' }));
          router.push({ path: '/login' });
        }
      })
    }
  }
}, error => {
  NProgress.done();
  return Promise.reject(new Error(error));
})

/**
 * 对axios进行二次封装
 *
 * 拦截器中根据mate的不同配置做出了响应
 * 目前响应字段有isToken、isSerialize
 *
 */
import config from "@/framework/config";
var baseURL = config.baseURL;


function apiRequest(url, method, params, mate = null) {

  switch (method) {
    case "get":
      return axios({
        url: baseURL + url,
        method: method,
        mate,
        params: params,
      });
    case "post":
    case "put":
    case "delete":
    default:
      return axios({
        url: baseURL + url,
        method: method,
        mate,
        data: params,
      });
  }
}

class BaseApi {
  constructor(url) {
    this.url = url;
  }

  //根据id进行获取, 返回单个对象
  get(id) {
    return apiRequest(`${this.url}/view/${id}`, "get")
  }

  //根据callcode进行获取, 返回单个对象
  view(callcode) {
    return apiRequest(`${this.url}/view/${callcode}`, "get")
  }

  load() {
    return apiRequest(`${this.url}/list`, "get")
  }

  search(obj) {
    return apiRequest(`${this.url}/list`, "get", { json: JSON.stringify(obj) })
  }

  add(obj) {
    return apiRequest(this.url, "post", obj)
  }

  edit(obj) {
    return apiRequest(this.url, 'put', obj)
  }

  del(id) {
    return apiRequest(`${this.url}/${id}`, "delete")
  }
}

export {
  axios,
  apiRequest,
  BaseApi,
}